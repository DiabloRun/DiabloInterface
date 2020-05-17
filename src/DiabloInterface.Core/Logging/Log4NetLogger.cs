using System;
using log4net;
using log4net.Config;

namespace Zutatensuppe.DiabloInterface.Core.Logging
{
    public class Log4NetLogger : ILogger
    {
        readonly ILog logger;

        public Log4NetLogger(Type type) => logger = LogManager.GetLogger(type);
        public Log4NetLogger(string name) => logger = LogManager.GetLogger(name);

        public void Debug(object message) => logger.Debug(message);
        public void Info(object message) => logger.Info(message);
        public void Warn(object message) => logger.Warn(message);
        public void Error(object message) => logger.Error(message);
        public void Fatal(object message) => logger.Fatal(message);

        public void Debug(object message, Exception e) => logger.Debug(message, e);
        public void Info(object message, Exception e) => logger.Info(message, e);
        public void Warn(object message, Exception e) => logger.Warn(message, e);
        public void Error(object message, Exception e) => logger.Error(message, e);
        public void Fatal(object message, Exception e) => logger.Fatal(message, e);

        public static void Initialize() => XmlConfigurator.Configure();
    }
}
