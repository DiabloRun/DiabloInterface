namespace Zutatensuppe.DiabloInterface.Settings
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

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
                return JsonConvert.DeserializeObject<ApplicationSettings>(
                    jsonData,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
                );
            }
            catch (JsonException)
            {
                throw new IOException("Failed to read JSON");
            }
        }
    }
}
