using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace DiabloInterface.Serialization
{
    public class JsonSettingsWriter : ISettingsWriter
    {
        StreamWriter writer;

        public JsonSettingsWriter(string filename)
        {
            writer = new StreamWriter(filename);
        }

        public JsonSettingsWriter(string filename, Encoding encoding)
        {
            writer = new StreamWriter(filename, false, encoding);
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
            if (disposing)
            {
                writer.Close();
            }
        }

        public void Write(ApplicationSettings settings)
        {
            if (settings == null) return;

            string jsonData = JsonConvert.SerializeObject(settings, Formatting.Indented);
            writer.Write(jsonData);
            writer.Flush();
        }
    }
}
