using System;
using System.Collections.Generic;

namespace DiabloInterface.Logging
{
    public class Logger
    {
        static Logger instance;

        /// <summary>
        /// Get the currect active logger.
        /// </summary>
        public static Logger Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        object writeLock = new object();
        IEnumerable<ILogWriter> logWriters;

        /// <summary>
        /// Create a new logger with any writers.
        /// </summary>
        /// <param name="writers">The log writers used.</param>
        public Logger(IEnumerable<ILogWriter> writers)
        {
            if (writers == null)
            {
                throw new NullReferenceException("Log writers are null.");
            }

            logWriters = writers;
        }

        /// <summary>
        /// Writes a raw message to the log.
        /// This does not include timestamp information.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteLineRaw(string message)
        {
            // Aquite the write lock for thread safety.
            lock (writeLock)
            {
                foreach (ILogWriter writer in logWriters)
                {
                    writer.LogMessage(message + '\n');
                }
            }
        }

        /// <summary>
        /// Writes a formatted raw message to the log.
        /// This does not include timestamp information.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteLineRaw(string format, params object[] args)
        {
            string message = string.Format(format, args);
            WriteLineRaw(message);
        }

        /// <summary>
        /// Writes a line to the log.
        /// This includes timestamp information.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteLine(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            WriteLineRaw(string.Format("[{0}]: {1}", timestamp, message));
        }

        /// <summary>
        /// Writes a formatted line to the log.
        /// This includes timestamp information.
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            string message = string.Format(format, args);
            WriteLine(message);
        }
    }
}
