namespace Zutatensuppe.DiabloInterface.Lib.Plugin
{
    public interface IPlugin
    {
        string Name { get; }

        void SetConfig(IPluginConfig config);

        void Initialize(IDiabloInterface di);

        void Reset();

        void Dispose();

        T GetRenderer<T>() where T : IPluginRenderer;
    }
}
