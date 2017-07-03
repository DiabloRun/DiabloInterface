namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    public interface ILegacySettingsResolver
    {
        ApplicationSettings ResolveSettings(ApplicationSettings settings, ILegacySettingsObject obj);
    }
}
