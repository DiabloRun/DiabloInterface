using System;

namespace Zutatensuppe.DiabloInterface.Core.Logging
{
    public class ConsoleLogWriter : ILogWriter
    {
        public void LogMessage(string message)
        {
            Console.Write(message);
        }
    }
}
