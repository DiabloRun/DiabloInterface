namespace Zutatensuppe.DiabloInterface.Settings
{
    public interface ILegacySettingsResolver
    {
        ApplicationSettings ResolveSettings(ApplicationSettings settings, ILegacySettingsObject obj);
    }
}
