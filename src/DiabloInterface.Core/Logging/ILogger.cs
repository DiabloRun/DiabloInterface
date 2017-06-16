using System;

namespace Zutatensuppe.DiabloInterface.Core.Logging
{
    public interface ILogger
    {
        void Debug(object message);
        void Info(object message);
        void Warn(object message);
        void Error(object message);
        void Fatal(object message);

        void Debug(object message, Exception e);
        void Info(object message, Exception e);
        void Warn(object message, Exception e);
        void Error(object message, Exception e);
        void Fatal(object message, Exception e);
    }
}