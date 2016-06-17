namespace DiabloInterface.Serialization
{
    public interface ILegacySettingsResolver
    {
        ApplicationSettings ResolveSettings(ApplicationSettings settings, ILegacySettingsObject obj);
    }
}
