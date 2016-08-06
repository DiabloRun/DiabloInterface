using System;

namespace Zutatensuppe.DiabloInterface.Settings
{
    public interface ISettingsReader : IDisposable
    {
        ApplicationSettings Read();
    }
}
