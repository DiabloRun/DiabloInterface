using System;

namespace Zutatensuppe.DiabloInterface.Core.Logging
{
    public class LogServiceLocator
    {
        public static ILogger Get(Type type)
        {
            return new Log4NetLogger(type);
        }
    }
}
