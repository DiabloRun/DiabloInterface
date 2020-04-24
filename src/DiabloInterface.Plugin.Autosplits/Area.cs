namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    
    public class Area
    {
        public int Id { get; set; }
        public int Act { get; set; }
        public string Name { get; set; }

        private static List<Area> areaList;

        public override string ToString()
        {
            return $"Act {Act} - {Name}";
        }

        public static List<Area> getAreaList()
        {
            if (areaList == null)
            {
                areaList = readAreaList();
            }
            return areaList;
        }

        private static List<Area> readAreaList ()
        {
            List<Area> areas = new List<Area>();
            JsonSerializer serializer = new JsonSerializer();

            using (MemoryStream stream = new MemoryStream(Properties.Resources.areas))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        areas = serializer.Deserialize<List<Area>>(reader);
                    }
                }
            }
            
            return areas;
        }
    }
}
