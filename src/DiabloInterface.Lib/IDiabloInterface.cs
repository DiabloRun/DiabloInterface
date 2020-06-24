using Zutatensuppe.DiabloInterface.Lib.Services;

namespace Zutatensuppe.DiabloInterface.Lib
{
    public interface IDiabloInterface
    {
        IGameService game { get; }
        IConfigService configService { get; }
        IPluginService plugins { get; }
    }
}
