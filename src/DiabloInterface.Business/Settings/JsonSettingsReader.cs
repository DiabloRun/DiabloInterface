namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;
    using System.IO;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JsonSettingsReader : ISettingsReader
    {
        readonly ILegacySettingsResolver resolver;
        readonly string jsonData;

        public JsonSettingsReader(ILegacySettingsResolver resolver, string filename)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            jsonData = File.ReadAllText(filename);
        }

        public JsonSettingsReader(ILegacySettingsResolver resolver, string filename, Encoding encoding)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
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
                var data = JObject.Parse(jsonData);
                var settings = data.ToObject<ApplicationSettings>();
                var legacyObject = new JsonLegacySettings(data);
                return resolver.ResolveSettings(settings, legacyObject);
            }
            catch (JsonException)
            {
                throw new IOException("Failed to read JSON");
            }
        }
    }
}
