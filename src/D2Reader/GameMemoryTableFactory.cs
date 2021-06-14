using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Zutatensuppe.D2Reader
{
    public interface IGameMemoryTableFactory
    {
        GameMemoryTable CreateForReader(IProcessMemoryReader reader);
    }

    public class GameVersionUnsupportedException : Exception
    {
        public string GameVersion;
        public GameVersionUnsupportedException(string gameVersion) :
            base(string.Format("Failed to create memory table for game version {0}", gameVersion))
        {
            GameVersion = gameVersion;
        }
    }

    public class ModuleNotLoadedException : Exception
    {
        public string ModuleName;
        public ModuleNotLoadedException(string moduleName) :
            base(string.Format("Failed to create memory table, module not loaded {0}", moduleName))
        {
            ModuleName = moduleName;
        }
    }

    public class GameMemoryTableFactory : IGameMemoryTableFactory
    {
        static readonly ILogger Logger = Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private GameMemoryTable CreateForVersion(
            string gameVersion,
            IntPtr BaseAddress,
            Dictionary<string, IntPtr> moduleBaseAddresses
        ) {
            // Refer to the wiki at https://github.com/Zutatensuppe/DiabloInterface/wiki/Finding-memory-table-addresses
            // for information about how to find addresses for a different version of the game.
            switch (gameVersion)
            {
                case "1.14d":
                case "1.14.3.71":
                    return CreateMemoryTableForVersion114D(BaseAddress);
                case "1.14c":
                case "1.14.2.70":
                    return CreateMemoryTableForVersion114C(BaseAddress);
                case "1.14b":
                case "1.14.1.68":
                    return CreateMemoryTableForVersion114B(BaseAddress);
                case "1.13d":
                case "1, 0, 13, 64":
                    return CreateMemoryTableForVersion113D(moduleBaseAddresses);
                case "1.13c":
                case "v 1.13c":
                case "1, 0, 13, 60":
                case "1, 0, 0, 0": // D2Loader-high.exe
                    return CreateMemoryTableForVersion113C(moduleBaseAddresses);
                default:
                    throw new GameVersionUnsupportedException(gameVersion);
            }
        }

        private GameMemoryTable CreateMemoryTableForVersion114D(IntPtr BaseAddress)
        {
            return new GameMemoryTable()
            {
                Loading                     = BaseAddress + 0x30F2C0,
                Saving                      = BaseAddress + 0x3792F8,
                Saving2                     = BaseAddress + 0x3786D0,
                InGame                      = BaseAddress + 0x30EE8C,
                InMenu                      = BaseAddress + 0x379970,
                GlobalData                  = BaseAddress + 0x344304, // game.744304
                World                       = BaseAddress + 0x483D38, // set to something in Game.exe + 12C0A4
                                                                      // set to 0 in Game.exe + 12C030
                PlayersX                    = BaseAddress + 0x483D70,
                GameId                      = BaseAddress + 0x482D0C,
                LowQualityItems             = BaseAddress + 0x56CC58,
                ItemDescriptions            = BaseAddress + 0x56CA58,
                MagicModifierTable          = BaseAddress + 0x56CA7C,
                RareModifierTable           = BaseAddress + 0x56CAA0,
                Units113                    = null,
                Units114                    = BaseAddress + 0x3A5E70,
                PlayerUnit                  = BaseAddress + 0x3A5E74,
                Area                        = BaseAddress + 0x3A3140,
                Pets                        = BaseAddress + 0x3BB5BC,
                InventoryTab                = BaseAddress + 0x3BCC4C,
                StringIndexerTable          = BaseAddress + 0x4829B4,
                StringAddressTable          = BaseAddress + 0x4829B8,
                PatchStringIndexerTable     = BaseAddress + 0x4829D0,
                PatchStringAddressTable     = BaseAddress + 0x4829BC,
                ExpansionStringIndexerTable = BaseAddress + 0x4829D4,
                ExpansionStringAddressTable = BaseAddress + 0x4829C0,
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion114C(IntPtr BaseAddress)
        {
            return new GameMemoryTable()
            {
                Loading                     = BaseAddress + 0x30DF7C,
                Saving                      = BaseAddress + 0x36F760,
                Saving2                     = BaseAddress + 0x370380,
                InGame                      = BaseAddress + 0x30DBC4,
                InMenu                      = BaseAddress + 0x478884,
                GlobalData                  = BaseAddress + 0x33FD78,
                World                       = BaseAddress + 0x47ACC0,
                PlayersX                    = BaseAddress + 0x47ACF8,
                GameId                      = BaseAddress + 0x479C94,
                LowQualityItems             = BaseAddress + 0x563BE0,
                ItemDescriptions            = BaseAddress + 0x5639E0,
                MagicModifierTable          = BaseAddress + 0x563A04,
                RareModifierTable           = BaseAddress + 0x563A28,
                Units113                    = null,
                Units114                    = BaseAddress + 0x39CEF8,
                PlayerUnit                  = BaseAddress + 0x39CEFC,
                Area                        = BaseAddress + 0x39A1C8,
                Pets                        = BaseAddress + 0x3B2644,
                InventoryTab                = BaseAddress + 0x3B3CD4,
                StringIndexerTable          = BaseAddress + 0x479A3C,
                StringAddressTable          = BaseAddress + 0x479A40,
                PatchStringIndexerTable     = BaseAddress + 0x479A58,
                PatchStringAddressTable     = BaseAddress + 0x479A44,
                ExpansionStringIndexerTable = BaseAddress + 0x479A5C,
                ExpansionStringAddressTable = BaseAddress + 0x479A48,
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion114B(IntPtr BaseAddress)
        {
            return new GameMemoryTable()
            {
                Loading                     = BaseAddress + 0x30EF7C,
                Saving                      = BaseAddress + 0x370760,
                Saving2                     = BaseAddress + 0x371380,
                InGame                      = BaseAddress + 0x30EBC4,
                InMenu                      = BaseAddress + 0x47993C,
                GlobalData                  = BaseAddress + 0x340D78,
                World                       = BaseAddress + 0x47BD78,
                PlayersX                    = BaseAddress + 0x47BDB0,
                GameId                      = BaseAddress + 0x47AD4C,
                LowQualityItems             = BaseAddress + 0x564C98,
                ItemDescriptions            = BaseAddress + 0x564A98,
                MagicModifierTable          = BaseAddress + 0x564ABC,
                RareModifierTable           = BaseAddress + 0x564AE0,
                Units113                    = null,
                Units114                    = BaseAddress + 0x39DEF8,
                PlayerUnit                  = BaseAddress + 0x39DEFC,
                Area                        = BaseAddress + 0x39B1C8,
                Pets                        = BaseAddress + 0x3B3644,
                InventoryTab                = BaseAddress + 0x3B4CD4,
                StringIndexerTable          = BaseAddress + 0x47AAF4,
                StringAddressTable          = BaseAddress + 0x47AAF8,
                PatchStringIndexerTable     = BaseAddress + 0x47AB10,
                PatchStringAddressTable     = BaseAddress + 0x47AAFC,
                ExpansionStringIndexerTable = BaseAddress + 0x47AB14,
                ExpansionStringAddressTable = BaseAddress + 0x47AB00,
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion113D(Dictionary<string, IntPtr> moduleBaseAddresses)
        {
            IntPtr d2CommonAddress = GetModuleAddress("d2common.dll", moduleBaseAddresses);
            IntPtr d2LangAddress = GetModuleAddress("d2lang.dll", moduleBaseAddresses);
            IntPtr d2NetAddress = GetModuleAddress("d2net.dll", moduleBaseAddresses);
            IntPtr d2GameAddress = GetModuleAddress("d2game.dll", moduleBaseAddresses);
            IntPtr d2ClientAddress = GetModuleAddress("d2client.dll", moduleBaseAddresses);

            return new GameMemoryTable()
            {
                Loading                     = d2ClientAddress + 0x11D364,
                Saving                      = null,
                Saving2                     = null,
                InGame                      = null,
                InMenu                      = null,
                GlobalData                  = d2CommonAddress + 0x0A33F0,
                World                       = d2GameAddress   + 0x111C10,
                PlayersX                    = d2GameAddress   + 0x111C44,
                GameId                      = d2NetAddress    + 0x00B420, // and the pointer to that address is: d2NetAddress + 0x0070A8
                LowQualityItems             = d2CommonAddress + 0x0A4EB0,
                ItemDescriptions            = d2CommonAddress + 0x0A4CB0,
                MagicModifierTable          = d2CommonAddress + 0x0A4CD4,
                RareModifierTable           = d2CommonAddress + 0x0A4CF8,
                Units113                    = d2ClientAddress + 0x1049B8,
                Units114                    = null,
                PlayerUnit                  = d2ClientAddress + 0x101024,
                Area                        = d2ClientAddress + 0x08F66C,
                Pets                        = d2ClientAddress + 0x11CE30,
                InventoryTab                = d2ClientAddress + 0x11CB84,
                StringIndexerTable          = d2LangAddress   + 0x010A84,
                StringAddressTable          = d2LangAddress   + 0x010A88,
                PatchStringIndexerTable     = d2LangAddress   + 0x010AA0,
                PatchStringAddressTable     = d2LangAddress   + 0x010A8C,
                ExpansionStringIndexerTable = d2LangAddress   + 0x010AA4,
                ExpansionStringAddressTable = d2LangAddress   + 0x010A90,
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion113C(Dictionary<string, IntPtr> moduleBaseAddresses)
        {
            IntPtr d2CommonAddress = GetModuleAddress("d2common.dll", moduleBaseAddresses);
            IntPtr d2LangAddress = GetModuleAddress("d2lang.dll", moduleBaseAddresses);
            IntPtr d2NetAddress = GetModuleAddress("d2net.dll", moduleBaseAddresses);
            IntPtr d2GameAddress = GetModuleAddress("d2game.dll", moduleBaseAddresses);
            IntPtr d2ClientAddress = GetModuleAddress("d2client.dll", moduleBaseAddresses);

            return new GameMemoryTable()
            {
                Loading                     = d2ClientAddress + 0x0FAE08,
                Saving                      = null,
                Saving2                     = null,
                InGame                      = null,
                InMenu                      = null,
                GlobalData                  = d2CommonAddress + 0x099E1C,

                // world + players x dont work for closed bnet
                World                       = d2GameAddress   + 0x111C24,
                PlayersX                    = d2GameAddress   + 0x111C1C,

                GameId                      = d2NetAddress    + 0x00B428,
                LowQualityItems             = d2CommonAddress + 0x09FD98,
                ItemDescriptions            = d2CommonAddress + 0x09FB94,
                MagicModifierTable          = d2CommonAddress + 0x09FBB8,
                RareModifierTable           = d2CommonAddress + 0x09FBDC,
                Units113                    = d2ClientAddress + 0x10A808,
                Units114                    = null,
                PlayerUnit                  = d2ClientAddress + 0x10A60C,
                Area                        = d2ClientAddress + 0x11C310,
                Pets                        = d2ClientAddress + 0x11C4D4,
                InventoryTab                = d2ClientAddress + 0x11BC94,
                StringIndexerTable          = d2LangAddress   + 0x010A64,
                StringAddressTable          = d2LangAddress   + 0x010a68,
                PatchStringIndexerTable     = d2LangAddress   + 0x010A80,
                PatchStringAddressTable     = d2LangAddress   + 0x010A6C,
                ExpansionStringIndexerTable = d2LangAddress   + 0x010A84,
                ExpansionStringAddressTable = d2LangAddress   + 0x010A70,
            };
        }

        private IntPtr GetModuleAddress(
            string moduleName,
            Dictionary<string, IntPtr> moduleBaseAddresses
        ) {
            if (!moduleBaseAddresses.TryGetValue(moduleName, out IntPtr address))
                throw new ModuleNotLoadedException(moduleName);
            return address;
        }

        public GameMemoryTable CreateForReader(IProcessMemoryReader reader)
        {
            try
            {
                Logger.Info($"Check version: {reader.ProcessInfo.FileVersion}");
                return CreateForVersion(
                    reader.ProcessInfo.FileVersion,
                    reader.ProcessInfo.BaseAddress,
                    reader.ProcessInfo.ModuleBaseAddresses
                );
            }
            catch (GameVersionUnsupportedException)
            {
                try
                {
                    return CreateForReaderD2SEFallback1(reader);
                }
                catch (GameVersionUnsupportedException)
                {
                    return CreateForReaderD2SEFallback2(reader);
                }
            }
        }

        private GameMemoryTable CreateForReaderD2SEFallback1(IProcessMemoryReader reader)
        {
            // we try to detect version for D2SE
            IntPtr baseAddress;
            try
            {
                baseAddress = GetModuleAddress(
                    "fog.dll",
                    reader.ProcessInfo.ModuleBaseAddresses
                );
            } catch (ModuleNotLoadedException)
            {
                throw new GameVersionUnsupportedException("Unknown");
            }
            var pointer = new Pointer() {
                Base = baseAddress + 0x04AFE0,
                Offsets = new int[] { 0x0, 0xe00 }
            };
            var versionAddress = reader.ResolvePointer(pointer);
            var version = reader.ReadNullTerminatedString(versionAddress, 20, Encoding.ASCII);
            Logger.Info($"Check D2SE version: {version}");
            return CreateForVersion(
                version,
                reader.ProcessInfo.BaseAddress,
                reader.ProcessInfo.ModuleBaseAddresses
            );
        }

        private GameMemoryTable CreateForReaderD2SEFallback2(IProcessMemoryReader reader)
        {
            // pre DiabloInterface v0.4.11 check for 1.13c in D2SE
            //
            // the check just reads the string displayed in D2SE,
            // so this is not a safe check, but apparently neither is the one above.
            // Until we can safely determine the D2 version with a single
            // method, we keep the legacy check...
            //
            // this should be 1.13c, but most likely isn't, in many cases
            var versionAddress = reader.ProcessInfo.BaseAddress + 0x1A049;
            var version = reader.ReadNullTerminatedString(versionAddress, 5, Encoding.ASCII);
            Logger.Info($"Check D2SE version (Fallback check): {version}");
            return CreateForVersion(
                version,
                reader.ProcessInfo.BaseAddress,
                reader.ProcessInfo.ModuleBaseAddresses
            );
        }
    }
}
