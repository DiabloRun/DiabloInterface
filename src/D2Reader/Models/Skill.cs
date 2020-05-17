namespace Zutatensuppe.D2Reader.Models
{
    public enum Skill
    {
        UNKNOWN, // 0x00
        KICK, // 0x01
        THROW, // 0x02
        UNSUMMON, // 0x03
        LEFT_HAND_THROW, // 0x04
        LEFT_HAND_SWING, // 0x05

        FIREBOLT = 0x24, // 0x24

        RAISE_SKELETON = 0x46, // 0x46

        SCROLL_IDENT = 0xd9, // 0xd9
        TOME_IDENT, // 0xda
        SCROLL_TP, // 0xdb
        TOME_TP, // 0xdc
    }
}
