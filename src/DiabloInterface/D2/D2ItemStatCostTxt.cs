using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DiabloInterface
{
    class D2ItemStatCostTxt
    {
        public string name;
        public int id;
        private static List<D2ItemStatCostTxt> suffixes;

        public D2ItemStatCostTxt(string[] lineArray)
        {
            name = lineArray[0];
            id = Int32.Parse(lineArray[1]);
        }

        public static List<D2ItemStatCostTxt> getAll()
        {
            if (suffixes == null)
            {
                suffixes = readAll();
            }
            return suffixes;
        }

        public static List<D2ItemStatCostTxt> readAll()
        {

            List<D2ItemStatCostTxt> list = new List<D2ItemStatCostTxt>();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DiabloInterface.Resources.ItemStatCost.txt";
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
                    if (line.Trim() == "")
                    {
                        continue;
                    }
                    lineArray = line.Split('\t');
                    if (lineArray[0] == "Expansion")
                    {
                        continue;
                    }
                    try
                    {
                        list.Add(new D2ItemStatCostTxt(lineArray));
                        //Console.Write(lineArray[1] + ":" + lineArray[0] + "\n") ;
                    }
                    catch (FormatException e)
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
