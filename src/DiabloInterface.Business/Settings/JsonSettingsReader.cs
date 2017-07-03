namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;
    using System.IO;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JsonSettingsReader : ISettingsReader
    {
        ILegacySettingsResolver resolver;
        string jsonData;

        public JsonSettingsReader(ILegacySettingsResolver resolver, string filename)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));

            this.resolver = resolver;
            jsonData = File.ReadAllText(filename);
        }

        public JsonSettingsReader(ILegacySettingsResolver resolver, string filename, Encoding encoding)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));

            this.resolver = resolver;
            jsonData = File.ReadAllText(filename, encoding);
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
                var settings = JsonConvert.DeserializeObject<ApplicationSettings>(jsonData);
                var legacyObject = new JsonLegacySettings(JObject.Parse(jsonData));
                return resolver.ResolveSettings(settings, legacyObject);
            }
            catch (JsonException)
            {
                throw new IOException("Failed to read JSON");
            }
        }
    }
}
