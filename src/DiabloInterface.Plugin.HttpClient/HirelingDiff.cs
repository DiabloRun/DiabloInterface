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
        private static readonly List<string> AutocompareProps = new List<string> {
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
        public List<ItemInfo> RemovedItems { get; set; }
        public List<SkillInfo> Skills { get; set; }

        internal static HirelingDiff GetDiff(HirelingDiff curr, HirelingDiff prev)
        {
            if (prev == null && curr == null)
                return null;

            if (prev == null || curr == null)
                return curr;

            var diff = new HirelingDiff();
            var hasDiff = false;

            var itemsDiff = DiffUtil.ItemsDiff(curr.Items, prev.Items);
            diff.AddedItems = itemsDiff.Item1;
            diff.RemovedItems = itemsDiff.Item2;

            if (!DiffUtil.ListsEqual(curr.Skills, prev.Skills))
                diff.Skills = curr.Skills;

            foreach (string propertyName in AutocompareProps)
            {
                var property = typeof(HirelingDiff).GetProperty(propertyName);
                var prevValue = property.GetValue(prev);
                var newValue = property.GetValue(curr);
                if (!DiffUtil.ObjectsEqual(prevValue, newValue))
                {
                    hasDiff = true;
                    property.SetValue(diff, newValue);
                }
            }

            hasDiff = hasDiff
                || diff.AddedItems != null
                || diff.RemovedItems != null
                || diff.Skills != null;

            return hasDiff ? diff : null;
        }
    }
}
