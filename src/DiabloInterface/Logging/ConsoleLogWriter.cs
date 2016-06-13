using System;

namespace DiabloInterface.Logging
{
    public class ConsoleLogWriter : ILogWriter
    {
        public void LogMessage(string message)
        {
            Console.Write(message);
        }
    }
}
