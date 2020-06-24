using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Lib.Plugin
{
    public interface IPluginRenderer
    {
        Control CreateControl();

        // update controls with current data
        void ApplyChanges();

        // update controls with current config
        void ApplyConfig();
    }

    public interface IPluginConfigEditRenderer : IPluginRenderer
    {
        bool IsDirty();
        IPluginConfig GetEditedConfig();
    }

    public interface IPluginDebugRenderer : IPluginRenderer
    {
    }
}
