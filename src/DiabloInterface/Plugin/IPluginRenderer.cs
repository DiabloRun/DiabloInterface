using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin
{
    public interface IPluginRenderer
    {
        Control Render();

        // update controls with current data
        void ApplyChanges();

        // update controls with current config
        void ApplyConfig();
    }

    public interface IPluginSettingsRenderer : IPluginRenderer
    {
        bool IsDirty();
        PluginConfig GetEditedConfig();
    }

    public interface IPluginDebugRenderer : IPluginRenderer
    {
    }
}
