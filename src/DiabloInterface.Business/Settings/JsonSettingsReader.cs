namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;
    using System.IO;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JsonSettingsReader : ISettingsReader
    {
        readonly string filename;

        public JsonSettingsReader(string filename)
        {
            this.filename = filename;
        }

        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Close();
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public ApplicationSettings Read()
        {
            try
            {
                var jsonData = File.ReadAllText(filename);
                return JObject.Parse(jsonData).ToObject<ApplicationSettings>();
            }
            catch (JsonException)
            {
                throw new IOException("Failed to read JSON");
            }
        }
    }
}
