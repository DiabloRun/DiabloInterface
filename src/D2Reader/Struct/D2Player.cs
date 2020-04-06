using System.Runtime.InteropServices;
using Zutatensuppe.D2Reader.Readers;
using Zutatensuppe.D2Reader.Struct.Stat;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xF4)]
    class D2Player : D2Unit
    {
        public bool IsNewPlayer(UnitReader reader)
        {
            return ReadExperience(reader) == 0 && ReadLevel(reader) == 1;
        }

        public bool HasReset(Character character, UnitReader reader)
        {
            int experience = ReadExperience(reader);

            // We were just in the title screen and came back to a new character.
            if (character.WasInTitleScreen && experience == 0)
            {
                return true;
            }

            // If we lost experience on level 1 we have a reset. Level 1 check is important or
            // this might think we reset when losing experience in nightmare or hell after dying.
            if (character.Level == 1 && experience < character.Experience)
            {
                return true;
            }

            if (ReadLevel(reader) < character.Level)
            {
                return true;
            }

            return false;
        }

        private int ReadLevel(UnitReader reader)
        {
            return reader.GetStatValue(this, StatIdentifier.Level) ?? 0;
        }

        private int ReadExperience(UnitReader reader)
        {
            return reader.GetStatValue(this, StatIdentifier.Experience) ?? 0;
        }

    }
}
