using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiabloInterface
{
    class D2MagicSuffix
    {
        public string name;
        private static List<D2MagicSuffix> suffixes;

        public D2MagicSuffix(string[] lineArray)
        {
            name = lineArray[0];
        }

        public static List<D2MagicSuffix> getAll()
        {
            if (suffixes == null)
            {
                suffixes = readAll();
            }
            return suffixes;
        }

        public static List<D2MagicSuffix> readAll()
        {

            List<D2MagicSuffix> list = new List<D2MagicSuffix>();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DiabloInterface.Resources.MagicSuffix.txt";
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
                        list.Add(new D2MagicSuffix(lineArray));
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
