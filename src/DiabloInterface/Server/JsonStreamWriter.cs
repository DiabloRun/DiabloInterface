using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace DiabloInterface.Server
{
    class JsonStreamWriter
    {
        BinaryWriter writer;
        Encoding encoding;

        public JsonStreamWriter(Stream stream, Encoding encoding)
        {
            writer = new BinaryWriter(stream);
            this.encoding = encoding;
        }

        void WriteJsonString(string data)
        {
            var buffer = encoding.GetBytes(data);
            writer.Write(buffer.Length);
            writer.Write(buffer);
        }

        public void WriteJson(object json)
        {
            string jsonData = JsonConvert.SerializeObject(json);
            WriteJsonString(jsonData);
        }

        public void Flush()
        {
            writer.Flush();
        }
    }
}
