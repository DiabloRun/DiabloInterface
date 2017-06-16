using System;
using System.Collections;
using Microsoft.Win32;

namespace Zutatensuppe.DiabloInterface.Framework
{
    internal static class NetFrameworkVersionComparator
    {
        public static NetFrameworkVersion NewestFrameworkVersion
        {
            get
            {
                NetFrameworkVersion[] versions = InstalledFrameworkVersions;
                if (versions == null || versions.Length == 0)
                    return NetFrameworkVersion.Unknown;

                var mostRecentVersion = NetFrameworkVersion.Unknown;
                foreach (var version in versions)
                    if ((int) version > (int) mostRecentVersion)
                        mostRecentVersion = version;

                return mostRecentVersion;
            }
        }

        static NetFrameworkVersion[] InstalledFrameworkVersions
        {
            get
            {
                var frameworkVersions = FindOldNetFrameworkVersions();

                var modernFrameworkVersion = FindModernNetFrameworkVersion();
                if (modernFrameworkVersion != NetFrameworkVersion.Unknown)
                    frameworkVersions.Add(modernFrameworkVersion);

                return (NetFrameworkVersion[]) frameworkVersions.ToArray(typeof(NetFrameworkVersion));
            }
        }

        public static bool IsFrameworkVersionSupported(NetFrameworkVersion targetedVersion)
        {
            return (int) targetedVersion <= (int) NewestFrameworkVersion;
        }

        static ArrayList FindOldNetFrameworkVersions()
        {
            try
            {
                return TryFindOldNetFrameworkVersions();
            }
            catch (Exception)
            {
                return new ArrayList();
            }
        }

        static ArrayList TryFindOldNetFrameworkVersions()
        {
            const string ndpKeyName = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\";
            using (var ndpKey = Registry.LocalMachine.OpenSubKey(ndpKeyName))
            {
                if (ndpKey == null) return new ArrayList();

                var versions = new ArrayList();
                foreach (var versionKeyName in ndpKey.GetSubKeyNames())
                {
                    var frameworkVersion = ParseOldNetFrameworkVersionKey(ndpKey, versionKeyName);
                    if (frameworkVersion != NetFrameworkVersion.Unknown)
                        versions.Add(frameworkVersion);
                }

                return versions;
            }
        }

        static NetFrameworkVersion ParseOldNetFrameworkVersionKey(RegistryKey ndpKey, string versionKeyName)
        {
            if (versionKeyName == null || !versionKeyName.StartsWith("v"))
                return NetFrameworkVersion.Unknown;

            using (var versionKey = ndpKey.OpenSubKey(versionKeyName))
            {
                if (versionKey == null) return NetFrameworkVersion.Unknown;

                var install = versionKey.GetValue("Install", "0").ToString();
                var servicePack = versionKey.GetValue("SP")?.ToString();

                if (servicePack != null && install == "1")
                    return TranslateOldFrameworkVersionNumber(versionKeyName);
            }

            return NetFrameworkVersion.Unknown;
        }

        static NetFrameworkVersion FindModernNetFrameworkVersion()
        {
            try
            {
                return TryFindNetFrameworkVersion();
            }
            catch (Exception)
            {
                return NetFrameworkVersion.Unknown;
            }
        }

        static NetFrameworkVersion TryFindNetFrameworkVersion()
        {
            const string ndpKeyName = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            using (var ndpKey = Registry.LocalMachine.OpenSubKey(ndpKeyName))
            {
                if (ndpKey == null || ndpKey.GetValue("Install", "0").ToString() != "1")
                    return NetFrameworkVersion.Unknown;

                var releaseKey = Convert.ToInt32(ndpKey.GetValue("Release", "0"));
                return TranslateModernFrameworkVersionNumber(releaseKey);
            }
        }

        static NetFrameworkVersion TranslateOldFrameworkVersionNumber(string versionKeyName)
        {
            if (versionKeyName == null) return NetFrameworkVersion.Unknown;

            var majorMinorVersion = versionKeyName.Substring(1, 3);
            switch (majorMinorVersion)
            {
                case "1.1": return NetFrameworkVersion.Version_1_1;
                case "2.0": return NetFrameworkVersion.Version_2_0;
                case "3.0": return NetFrameworkVersion.Version_3_0;
                case "3.5": return NetFrameworkVersion.Version_3_5;

                default: return NetFrameworkVersion.Unknown;
            }
        }

        static NetFrameworkVersion TranslateModernFrameworkVersionNumber(int releaseKey)
        {
            switch (releaseKey)
            {
                case 378389:
                    return NetFrameworkVersion.Version_4_5;
                case 378675: // Windows 8.1 or Windows Server 2012 R2.
                case 378758: // Windows 8, Windows 7 SP1, or Windows Vista SP2.
                    return NetFrameworkVersion.Version_4_5_1;
                case 379893:
                    return NetFrameworkVersion.Version_4_5_2;
                case 393295: // Windows 10.
                case 393297: // All other OS versions.
                    return NetFrameworkVersion.Version_4_6;
                case 394254: // Windows 10 November Update.
                case 394271: // All other OS versions.
                    return NetFrameworkVersion.Version_4_6_1;
                case 394802: // Windows 10 Anniversary Update.
                case 394806: // All other OS versions.
                    return NetFrameworkVersion.Version_4_6_2;
                case 460798: // Windows 10 Creators Update
                case 460805: // All other OS versions.
                    return NetFrameworkVersion.Version_4_7;
                default:
                    if (releaseKey > 460805)
                        return NetFrameworkVersion.Version_Future;
                    return NetFrameworkVersion.Unknown;
            }
        }
    }
}
