using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DiabloInterface
{
    // from d2exp.mpq/data/excel/Levels.txt
    class D2Level
    {
        public int id;
        public string name;
        private static List<D2Level> levels;

        public D2Level(string[] lineArray)
        {
            id = Int32.Parse(lineArray[1]);
            name = lineArray[152];
        }

        public static List<D2Level> getAll ()
        {
            if (levels == null)
            {
                levels = readAll();
            }
            return levels;
        }

        public static List<D2Level> readAll ()
        {

            List<D2Level> list = new List<D2Level>();
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
                        list.Add(new D2Level(lineArray));
                        //Console.Write(lineArray[1] + ":" + lineArray[0] + "\n") ;
                    }
                    catch (FormatException e )
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
