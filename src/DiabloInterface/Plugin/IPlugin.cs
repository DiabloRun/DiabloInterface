namespace Zutatensuppe.DiabloInterface.Plugin
{
    public interface IPlugin
    {
        string Name { get; }

        PluginConfig Config { get; set; }

        void Initialize(DiabloInterface di);

        void Reset();

        void Dispose();

        T GetRenderer<T>() where T : IPluginRenderer;
    }
}
