namespace Zutatensuppe.DiabloInterface.Settings
{
    using System;

    public interface ISettingsReader : IDisposable
    {
        ApplicationSettings Read();
    }
}
