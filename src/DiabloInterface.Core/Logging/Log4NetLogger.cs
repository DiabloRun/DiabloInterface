using System;
using log4net;
using log4net.Config;
using Newtonsoft.Json;

namespace Zutatensuppe.DiabloInterface.Core.Logging
{
    public class Log4NetLogger : ILogger
    {
        readonly ILog logger;

        public Log4NetLogger(Type type) => logger = LogManager.GetLogger(type);
        public Log4NetLogger(string name) => logger = LogManager.GetLogger(name);

        public void Debug(object message) => logger.Debug(Conv(message));
        public void Info(object message) => logger.Info(Conv(message));
        public void Warn(object message) => logger.Warn(Conv(message));
        public void Error(object message) => logger.Error(Conv(message));
        public void Fatal(object message) => logger.Fatal(Conv(message));

        public void Debug(object message, Exception e) => logger.Debug(Conv(message), e);
        public void Info(object message, Exception e) => logger.Info(Conv(message), e);
        public void Warn(object message, Exception e) => logger.Warn(Conv(message), e);
        public void Error(object message, Exception e) => logger.Error(Conv(message), e);
        public void Fatal(object message, Exception e) => logger.Fatal(Conv(message), e);
        
        private string Conv(string message) => message;
        private string Conv(object message) => JsonConvert.SerializeObject(message, Formatting.Indented);

        public static void Initialize() => XmlConfigurator.Configure();
    }
}
