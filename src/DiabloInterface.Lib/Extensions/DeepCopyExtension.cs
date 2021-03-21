using Newtonsoft.Json;

namespace Zutatensuppe.DiabloInterface.Lib.Extensions
{
    public static class DeepCopyExtension
    {
        public static T DeepCopy<T>(this T source)
        {
            if (ReferenceEquals(source, null))
                return default(T);

            var settings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                TypeNameHandling = TypeNameHandling.Auto,
            };

            var serialized = JsonConvert.SerializeObject(source, settings);
            return JsonConvert.DeserializeObject<T>(serialized, settings);
        }
    }
}
