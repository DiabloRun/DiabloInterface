namespace Zutatensuppe.DiabloInterface.Plugin
{
    public interface IPlugin
    {
        string Name { get; }

        void SetConfig(IPluginConfig config);

        void Initialize(DiabloInterface di);

        void Reset();

        void Dispose();

        T GetRenderer<T>() where T : IPluginRenderer;
    }
}
