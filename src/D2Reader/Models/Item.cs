using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Item;

namespace Zutatensuppe.D2Reader.Models
{
    public class Item
    {
        public D2Unit Unit;
        public D2ItemData ItemData;

        internal bool IsEquipped() => ItemData.IsEquipped();
        internal bool IsEquippedInSlot(BodyLocation loc) => ItemData.IsEquippedInSlot(loc);
        public ItemQuality ItemQuality() => ItemData.Quality;
        private bool HasFlag(ItemFlag flag) => ItemData.ItemFlags.HasFlag(flag);
        internal BodyLocation BodyLocation() => ItemData.BodyLoc;

        internal bool IsIdentified() => HasFlag(ItemFlag.Identified);
        internal bool IsEthereal() => HasFlag(ItemFlag.Ethereal);
        internal bool HasRuneWord() => HasFlag(ItemFlag.Runeword);
    }
}
