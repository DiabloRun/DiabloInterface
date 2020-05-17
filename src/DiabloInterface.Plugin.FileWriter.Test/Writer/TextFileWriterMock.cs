using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter.Writer.Test
{
    internal class TextFileWriterMock : ITextFileWriter
    {
        readonly Dictionary<string, string> fileData = new Dictionary<string, string>();

        public IReadOnlyDictionary<string, string> FileContents => fileData;

        public void WriteFile(string path, string contents)
        {
            fileData[path] = contents;
        }
    }
}
