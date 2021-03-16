using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Newtonsoft.Json;
using Zutatensuppe.DiabloInterface.Core.Logging;
using System.Reflection;
using Zutatensuppe.DiabloInterface.Lib.Plugin;
using Zutatensuppe.DiabloInterface.Lib;
using DiabloInterface.Plugin.HttpClient;

namespace Zutatensuppe.DiabloInterface.Plugin.HttpClient
{
    public class Plugin : BasePlugin
    {
        private readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public override string Name => "HttpClient";

        protected override Type ConfigEditRendererType => typeof(ConfigEditRenderer);

        private IDiabloInterface di;

        private static readonly System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient();

        private bool SendingData = false;

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

        async private Task PostJson(RequestBody body)
        {
            try
            {
                body.Headers = Config.Headers;
                var json = JsonConvert.SerializeObject(
                    body,
                    Formatting.None,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
                );

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

        private void Game_ProcessFound(object sender, ProcessFoundEventArgs e)
        {
            if (!Config.Enabled) return;

            // always send complete information for this kind of event
            var body = RequestBody.FromProcessFoundEventArgs(e, di);
            PostJson(body).Wait();
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            if (!Config.Enabled) return;

            if (SendingData) return;

            SendingData = true;

            var body = RequestBody.FromDataReadEventArgs(e, di);
            var bodyDiff = RequestBody.GetDiff(body, PrevData);

            if (bodyDiff != null)
            {
                PostJson(bodyDiff).Wait();
            }

            PrevData = body;

            SendingData = false;
        }
    }
}
