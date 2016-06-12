using System;
using System.IO;

namespace DiabloInterface.Logging
{
    public class FileLogWriter : ILogWriter
    {
        string filename;

        public FileLogWriter(string filename)
        {
            // We recreate log files with
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            // Create directory if it doesn't exist.
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            this.filename = filename;
        }

        public void LogMessage(string message)
        {
            File.AppendAllText(filename, message);
        }

        public static string TimedLogFilename()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            return string.Format("DI_{0}.log", timestamp);
        }
    }
}
