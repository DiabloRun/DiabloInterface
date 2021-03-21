using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Newtonsoft.Json;
using System.Reflection;
using Zutatensuppe.DiabloInterface.Lib.Plugin;
using Zutatensuppe.DiabloInterface.Lib;
using DiabloInterface.Plugin.HttpClient;

namespace Zutatensuppe.DiabloInterface.Plugin.HttpClient
{
    public class Plugin : BasePlugin
    {
        static readonly Lib.ILogger Logger = Lib.Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override string Name => "HttpClient";

        protected override Type ConfigEditRendererType => typeof(ConfigEditRenderer);

        private IDiabloInterface di;

        private static readonly System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient();

        private bool SendingDataRead = false;

        internal string content;

        internal Config Config { get; private set; } = new Config();

        private RequestBody PrevData = new RequestBody()
        {
            Items = new List<ItemInfo>(),
            Quests = Quests.DefaultCompleteQuestIds,
        };
        
        public override void SetConfig(IPluginConfig c)
        {
            Config = c == null ? new Config() : c as Config;
            ApplyConfig();

            if (Config.Enabled)
                Logger.Info($"HTTP client enabled: {Config.Url}");
            else
                Logger.Info("HTTP client disabled");
        }

        public override void Initialize(IDiabloInterface di)
        {
            this.di = di;
            SetConfig(di.configService.CurrentConfig.PluginConf(Name));
            di.game.ProcessFound += Game_ProcessFound;
            di.game.DataRead += Game_DataRead;
        }

        async private Task PostJson(string json)
        {
            try
            {
                var response = await Client.PostAsync(
                    Config.Url,
                    new StringContent(json, Encoding.UTF8, "application/json")
                );
                content = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                content = "Request failed";
            }
            finally
            {
                ApplyChanges();
            }
        }

        private string RequestBodyToJsonString(RequestBody body)
        {
            return JsonConvert.SerializeObject(
                body,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
            );
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            if (!Config.Enabled) return;
            _ = SendDataReadAsync(e);
        }

        async private Task SendDataReadAsync(DataReadEventArgs e)
        {
            if (SendingDataRead) return;

            var newData = RequestBody.FromDataReadEventArgs(e, di);
            var diff = RequestBody.GetDiff(newData, PrevData);

            if (diff != null)
            {
                diff.Headers = Config.Headers;
                SendingDataRead = true;
                await PostJson(RequestBodyToJsonString(diff));
                SendingDataRead = false;
            }

            PrevData = newData;
        }

        private void Game_ProcessFound(object sender, ProcessFoundEventArgs e)
        {
            if (!Config.Enabled) return;
            _ = SendProcessFoundAsync(e);
        }

        async private Task SendProcessFoundAsync(ProcessFoundEventArgs e)
        {
            // always send complete information for this kind of event
            var body = RequestBody.FromProcessFoundEventArgs(e, di);
            body.Headers = Config.Headers;
            await PostJson(RequestBodyToJsonString(body));
        }
    }
}
