using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DiabloInterface
{
    // from d2exp.mpq/data/excel/Levels.txt
    class Level
    {
        public int id;
        public string name;
        private static List<Level> levels;

        public Level(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public static List<Level> getAll ()
        {
            if (levels == null)
            {
                levels = readAll();
            }
            return levels;
        }

        public static List<Level> readAll ()
        {

            List<Level> list = new List<Level>();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DiabloInterface.Resources.Levels.txt";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                string[] lineArray;
                bool first = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    lineArray = line.Split('\t');
                    if (lineArray[0] == "Expansion")
                    {
                        continue;
                    }
                    try
                    {

                        list.Add(new Level(Int32.Parse(lineArray[1]), lineArray[152]));
                        //Console.Write(lineArray[1] + ":" + lineArray[0] + "\n") ;

                    }
                    catch (System.FormatException e )
                    {
                        Console.Write(e);
                        break;
                    }
                }
            }
            return list;
        }
    }
}
