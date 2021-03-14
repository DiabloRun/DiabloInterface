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
    internal class DiffUtil
    {
        internal static bool ListsEqual<T>(List<T> listA, List<T> listB)
        {
            if (listA == null && listB == null)
                return true;
            if (listA == null || listB == null)
                return false;
            return listA.SequenceEqual(listB);
        }

        internal static bool ObjectsEqual(object objA, object objB)
        {
            if (objA == null && objB == null)
                return true;
            if (objA == null || objB == null)
                return false;
            return objA.Equals(objB);
        }

        // returns a list for added items and a list with the GUID of removed items
        internal static Tuple<List<ItemInfo>, List<ItemInfo>> ItemsDiff(
            List<ItemInfo> curr,
            List<ItemInfo> prev
        )
        {
            var added = ExcessiveItems(prev, curr);
            var removed = ExcessiveItems(curr, prev);

            return new Tuple<List<ItemInfo>, List<ItemInfo>>(
                added.Count > 0 ? added : null,
                removed.Count > 0 ? removed : null
            );
        }

        private static List<ItemInfo> ExcessiveItems(
            List<ItemInfo> basis,
            List<ItemInfo> added
        )
        {
            if (added == null)
                return new List<ItemInfo>();

            if (basis == null)
                return added;

            return added.FindAll(a => !basis.Any(b => ItemInfo.AreEqual(b, a)));
        }

        internal static Dictionary<GameDifficulty, List<QuestId>> CompletedQuestsDiff(
            Dictionary<GameDifficulty, List<QuestId>> curr,
            Dictionary<GameDifficulty, List<QuestId>> prev
        )
        {
            if (prev == null && curr == null)
                return null;

            if (prev == null || curr == null)
                return curr;

            var diff = new Dictionary<GameDifficulty, List<QuestId>>();

            foreach (var difficulty in Quests.DefaultCompleteQuestIds.Keys)
            {
                if (!curr.ContainsKey(difficulty))
                    continue;

                var completed = curr[difficulty].FindAll(id => !prev.ContainsKey(difficulty) || !prev[difficulty].Contains(id));
                if (completed.Count() > 0)
                {
                    diff.Add(difficulty, completed);
                }
            }
            return diff.Count > 0 ? diff : null;
        }
    }
}
