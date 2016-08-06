using System;

namespace Zutatensuppe.DiabloInterface.Logging
{
    public class ConsoleLogWriter : ILogWriter
    {
        public void LogMessage(string message)
        {
            Console.Write(message);
        }
    }
}
