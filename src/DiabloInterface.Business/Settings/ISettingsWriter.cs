namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;

    public interface ISettingsWriter : IDisposable
    {
        void Write(ApplicationSettings settings);
    }
}
