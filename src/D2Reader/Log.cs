using System;

namespace Zutatensuppe.D2Reader
{
    public class Logging
    {
        static ILogService service = null;

        public static void SetLogService(ILogService s)
        {
            service = s;
        }

        internal static ILogger CreateLogger(Type type)
        {
            return service == null
                ? new VoidLogger()
                : service.CreateLogger(type);
        }
    }

    public interface ILogService
    {
        ILogger CreateLogger(Type type);
    }

    public interface ILogger
    {
        void Debug(object message);
        void Debug(object message, Exception e);
        void Info(object message);
        void Info(object message, Exception e);
        void Warn(object message);
        void Warn(object message, Exception e);
        void Error(object message);
        void Error(object message, Exception e);
        void Fatal(object message);
        void Fatal(object message, Exception e);
    }

    internal class VoidLogger : ILogger
    {
        public void Debug(object message) { }
        public void Debug(object message, Exception e) { }
        public void Info(object message) { }
        public void Info(object message, Exception e) { }
        public void Warn(object message) { }
        public void Warn(object message, Exception e) { }
        public void Error(object message) { }
        public void Error(object message, Exception e) { }
        public void Fatal(object message) { }
        public void Fatal(object message, Exception e) { }
    }
}
