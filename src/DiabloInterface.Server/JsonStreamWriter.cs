using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Zutatensuppe.DiabloInterface.Server
{
    internal class JsonStreamWriter
    {
        BinaryWriter writer;
        Encoding encoding;
        JsonConverter[] converters;

        public JsonStreamWriter(Stream stream, Encoding encoding, params JsonConverter[] converters)
        {
            writer = new BinaryWriter(stream);
            this.encoding = encoding;
            this.converters = converters;
        }

        void WriteJsonString(string data)
        {
            var buffer = encoding.GetBytes(data);
            writer.Write(buffer.Length);
            writer.Write(buffer);
        }

        public void WriteJson(object json)
        {
            WriteJsonString(JsonConvert.SerializeObject(json, converters));
        }

        public void Flush()
        {
            writer.Flush();
        }
    }
}
