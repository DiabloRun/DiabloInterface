namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;

    public interface ISettingsReader : IDisposable
    {
        ApplicationSettings Read();
    }
}
