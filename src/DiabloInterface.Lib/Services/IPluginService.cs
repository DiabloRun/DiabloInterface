using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Lib.Services
{
    public interface IPluginService : IDisposable
    {
        void Initialize();

        void Reset();

        Dictionary<string, Control> CreateControls<T>() where T : IPluginRenderer;

        bool EditedConfigsDirty { get; }

        Dictionary<string, IPluginConfig> GetEditedConfigs { get; }
    }
}
