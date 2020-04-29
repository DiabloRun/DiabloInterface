namespace Zutatensuppe.DiabloInterface.Plugin
{
    public interface IPlugin
    {
        string Name { get; }

        PluginConfig Config { get; set; }

        void Initialize();

        void Reset();

        void Dispose();

        T GetRenderer<T>() where T : IPluginRenderer;
    }
}
