using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x22)]
    class D2CharacterClassSkillIconStruct
    {
        [ExpectOffset(0x00)] int unknownInt; // this is really an int. not sure about the other members.. 
        [ExpectOffset(0x04)] int unknown2;
        [ExpectOffset(0x08)] int unknown3;
        [ExpectOffset(0x0C)] int unknown4;
        [ExpectOffset(0x10)] int unknown5;
        [ExpectOffset(0x14)] int unknown6;
        [ExpectOffset(0x18)] int unknown7;
        [ExpectOffset(0x1C)] int unknown8;
        [ExpectOffset(0x20)] short unknown9;
    }
}
