namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    using System.IO;

    internal class TextFileWriter : ITextFileWriter
    {
        public void WriteFile(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }
    }
}
