namespace Zutatensuppe.DiabloInterface.Settings
{
    using System;

    public interface ISettingsWriter : IDisposable
    {
        void Write(ApplicationSettings settings);
    }
}
