using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zutatensuppe.D2Reader.Struct.Monster
{
    // information from https://d2mods.info/forum/viewtopic.php?f=8&t=47809&p=360419

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class D2MonsterData
    {
        #region structure (sizeof = 0x060)
        [ExpectOffset(0x00)] public DataPointer pMonStats; // record in monstats.txt

        // bytes holding the component Ids for each component;
        // Order: HD, TR, LG, RA, LA, RH, LH, SH, S1, S2, S3, S4, S5, S6, S7, S8
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        [ExpectOffset(0x04)] public byte[] Components;

        [ExpectOffset(0x14)] public short NameSeed;

        [ExpectOffset(0x16)] public MonsterTypeFlags TypeFlags;

        [ExpectOffset(0x17)] public byte eLastMode;
        [ExpectOffset(0x18)] public int dwDuriel; // set only for duriel
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        [ExpectOffset(0x1C)] public byte[] MonUModList; // nine bytes holding the Ids for each MonUMod assigned to the unit
        [ExpectOffset(0x25)] public byte __unknown25;
        [ExpectOffset(0x26)] public short bossNo; // hcIdx from superuniques.txt for superuniques(word)
        [ExpectOffset(0x28)] public DataPointer pAiGeneral;

        // server side 
        // [ExpectOffset(0x2C)] public int pAiParams;
        // client side
        [ExpectOffset(0x2C)] public DataPointer szMonName; //  (ptr to wchar_t string, 300 chars long)

        [ExpectOffset(0x30)] public DataPointer pUnknownAiStructure;
        [ExpectOffset(0x34)] public int __unknown34;
        [ExpectOffset(0x38)] public int __unknown38;
        [ExpectOffset(0x3C)] public int __unknown3C;
        [ExpectOffset(0x40)] public int dwNecroPet;
        [ExpectOffset(0x44)] public int __unknown44;
        [ExpectOffset(0x48)] public int __unknown48;
        [ExpectOffset(0x4C)] public int __unknown4C;
        [ExpectOffset(0x50)] public DataPointer pVision; // this may be polymorphic, the way this is used seams to depend on the monster type, used in LOS evaluation
        [ExpectOffset(0x54)] public int AiState; // this is used to tell monsters what special state has been set, this tells them they just got attacked etc
        [ExpectOffset(0x58)] public int lvlNo; // the Id from levels.txt of the level they got spawned in (used to access pGame -> pMonsterRegion[...])
        [ExpectOffset(0x5C)] public int SummonerFlags; // used only by the summoner
        #endregion
    }

    [Flags]
    public enum MonsterTypeFlags : byte
    {
        None = 0x00000000,
        // 00000001 = 001 = 0x00000001 - MONTYPE_OTHER
        // (set for some champs, uniques)
        Other = 0x00000001,
        // 00000010 = 002 = 0x00000002 - MONTYPE_SUPERUNIQUE
        // (eg. BISHIBOSH)
        SuperUnique = 0x00000002,
        // 00000100 = 004 = 0x00000004 - MONTYPE_CHAMPION
        Champion = 0x00000004,
        // 00001000 = 008 = 0x00000008 - MONTYPE_UNIQUE
        // (eg. BISHIBOSH, BLOODRAVEN, random bosses)
        Unique = 0x00000008,
        // 00010000 = 016 = 0x00000010 - MONTYPE_MINION      
        Minion = 0x00000010,
        // 00100000 = 032 = 0x00000020 - MONTYPE_POSSESSED
        Possessed = 0x00000020,
        // 01000000 = 064 = 0x00000040 - MONTYPE_GHOSTLY
        Ghostly = 0x00000040,
        // 10000000 = 128 = 0x00000080 - MONTYPE_MULTISHOT
        Multishot = 0x00000080,
    }
}
