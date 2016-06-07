using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DiabloInterface.Server
{
    class JsonStreamReader
    {
        BinaryReader reader;
        Encoding encoding;

        public JsonStreamReader(Stream stream, Encoding encoding)
        {
            reader = new BinaryReader(stream);
            this.encoding = encoding;
        }

        string ReadJsonString()
        {
            int length = reader.ReadInt32();
            byte[] buffer = new byte[length];
            reader.Read(buffer, 0, length);
            return encoding.GetString(buffer);
        }

        public object ReadJson()
        {
            string jsonData = ReadJsonString();
            return JsonConvert.DeserializeObject(jsonData);
        }

        public T ReadJson<T>()
        {
            string jsonData = ReadJsonString();
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
