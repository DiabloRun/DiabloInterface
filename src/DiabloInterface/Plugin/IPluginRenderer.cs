using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin
{
    public interface IPluginRenderer
    {
        Control CreateControl();

        // update controls with current data
        void ApplyChanges();

        // update controls with current config
        void ApplyConfig();
    }

    public interface IPluginSettingsRenderer : IPluginRenderer
    {
        bool IsDirty();
        IPluginConfig GetEditedConfig();
    }

    public interface IPluginDebugRenderer : IPluginRenderer
    {
    }
}
