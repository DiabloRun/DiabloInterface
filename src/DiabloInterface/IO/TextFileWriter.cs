using System.IO;

namespace Zutatensuppe.DiabloInterface.IO
{
    internal class TextFileWriter : ITextFileWriter
    {
        public void WriteFile(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }
    }
}
