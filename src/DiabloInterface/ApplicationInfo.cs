using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.DiabloInterface.Lib;

namespace Zutatensuppe.DiabloInterface
{
    class ApplicationInfo : IApplicationInfo
    {
        public string Version { get; internal set; }
        public string OS { get; internal set; }
        public string DotNet { get; internal set; }
    }
}
