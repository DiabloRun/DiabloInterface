using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zutatensuppe.DiabloInterface.Lib
{
    public interface IApplicationInfo
    {
        string Version { get; }
        string OS { get; }
        string DotNet { get; }
    }
}
