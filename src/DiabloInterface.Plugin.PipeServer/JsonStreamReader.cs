using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    internal class JsonStreamReader
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

        public T ReadJson<T>()
        {
            return JsonConvert.DeserializeObject<T>(ReadJsonString());
        }
    }
}
