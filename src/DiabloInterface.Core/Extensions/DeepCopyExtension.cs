namespace Zutatensuppe.DiabloInterface.Core.Extensions
{
    using Newtonsoft.Json;

    public static class DeepCopyExtension
    {
        public static T DeepCopy<T>(this T source)
        {
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            var settings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), settings);
        }
    }
}
