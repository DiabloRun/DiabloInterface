using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Item;

namespace Zutatensuppe.D2Reader.Models
{
    public class Item
    {
        public D2Unit Unit;
        public D2ItemData ItemData;
        public ItemLocation Location;

        internal bool IsEquipped() => ItemData.IsEquipped();
        internal bool IsEquippedInSlot(BodyLocation loc) => ItemData.IsEquippedInSlot(loc);
        public ItemQuality ItemQuality() => ItemData.Quality;
        private bool HasFlag(ItemFlag flag) => ItemData.ItemFlags.HasFlag(flag);
        internal BodyLocation BodyLocation() => ItemData.BodyLoc;

        internal bool IsIdentified() => HasFlag(ItemFlag.Identified);
        internal bool IsEthereal() => HasFlag(ItemFlag.Ethereal);
        internal bool HasRuneWord() => HasFlag(ItemFlag.Runeword);

        internal bool IsInCube() => ItemData.InvPage == InventoryPage.HoradricCube;
        internal bool IsInStash() => ItemData.InvPage == InventoryPage.Stash;
        internal bool IsInInventory() => ItemData.InvPage == InventoryPage.Inventory;
    }

    public class ItemLocation
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public BodyLocation BodyLocation;
        public ItemContainer Container;

        public static bool operator ==(ItemLocation left, ItemLocation right)
        {
            if ((object)left == null)
                return (object)right == null;

            return left.Equals(right);
        }

        public static bool operator !=(ItemLocation left, ItemLocation right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            var other = (ItemLocation)obj;
            return this.X            == other.X
                && this.Y            == other.Y
                && this.Width        == other.Width
                && this.Height       == other.Height
                && this.BodyLocation == other.BodyLocation
                && this.Container    == other.Container;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode()
                 ^ Y.GetHashCode()
                 ^ Width.GetHashCode()
                 ^ Height.GetHashCode()
                 ^ BodyLocation.GetHashCode()
                 ^ Container.GetHashCode();
        }
    }
}
