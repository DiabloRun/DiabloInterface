using System;

namespace Zutatensuppe.DiabloInterface.Settings
{
    public interface ISettingsWriter : IDisposable
    {
        void Write(ApplicationSettings settings);
    }
}
