namespace Zutatensuppe.DiabloInterface
{
    using System;
    using System.Collections.Generic;

    // from d2exp.mpq/data/excel/Levels.txt
    public class D2Level
    {
        public int id;
        public string name;
        private static List<D2Level> levels;

        public D2Level(string[] lineArray)
        {
            id = Int32.Parse(lineArray[1]);
            name = "Act " + (Int32.Parse(lineArray[3])+1) + " - " +  lineArray[152];
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

            string levelsString = Properties.Resources.Levels;
            string[] lines = levelsString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string[] lineArray;
            bool first = true;

            foreach (string line in lines)
            {
                if (first)
                {
                    first = false;
                    continue;
                }
                if ( line.Equals(""))
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
                    list.Add(new D2Level(lineArray));
                    //Console.Write(lineArray[1] + ":" + lineArray[0] + Environment.NewLine) ;
                }
                catch (FormatException e )
                {
                    Console.Write(e);
                    break;
                }
            }
            return list;
        }
    }
}
