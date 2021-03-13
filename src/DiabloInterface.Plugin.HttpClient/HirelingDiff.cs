using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.D2Reader.Models;

[assembly: InternalsVisibleTo("DiabloInterface.Plugin.HttpClient.Test")]
namespace DiabloInterface.Plugin.HttpClient
{
    internal class HirelingDiff
    {
        public static readonly List<string> AutocompareProps = new List<string> {
            "Name",
            "Class",
            "Level",
            "Experience",
            "Strength",
            "Dexterity",
            "FireResist",
            "ColdResist",
            "LightningResist",
            "PoisonResist"
        };

        public string Name { get; set; }
        public int? Class { get; set; }
        public int? Level { get; set; }
        public int? Experience { get; set; }
        public int? Strength { get; set; }
        public int? Dexterity { get; set; }
        public int? FireResist { get; set; }
        public int? ColdResist { get; set; }
        public int? LightningResist { get; set; }
        public int? PoisonResist { get; set; }
        public List<ItemInfo> Items { get; set; }
        public List<ItemInfo> AddedItems { get; set; }
        public List<int> RemovedItems { get; set; }
        public List<uint> SkillIds { get; set; }

        internal static HirelingDiff GetDiff(HirelingDiff newVal, HirelingDiff prevVal)
        {
            if (prevVal == null && newVal == null)
                return null;

            if (prevVal == null || newVal == null)
                return newVal;

            var diff = new HirelingDiff();
            var hasDiff = false;

            var hirelingItemDiff = DiffUtil.ItemsDiff(newVal.Items, prevVal.Items);
            diff.AddedItems = hirelingItemDiff.Item1;
            diff.RemovedItems = hirelingItemDiff.Item2;

            if (!DiffUtil.ListsEqual(prevVal.SkillIds, newVal.SkillIds))
                diff.SkillIds = newVal.SkillIds;

            foreach (string propertyName in AutocompareProps)
            {
                var property = typeof(HirelingDiff).GetProperty(propertyName);
                var prevValue = property.GetValue(prevVal);
                var newValue = property.GetValue(newVal);
                if (!DiffUtil.ObjectsEqual(prevValue, newValue))
                {
                    hasDiff = true;
                    property.SetValue(diff, newValue);
                }
            }

            hasDiff = hasDiff
                || diff.AddedItems != null
                || diff.RemovedItems != null
                || diff.SkillIds != null;

            return hasDiff ? diff : null;
        }
    }
}
