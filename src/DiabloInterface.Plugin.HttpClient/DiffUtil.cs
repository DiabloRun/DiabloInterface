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
            List<ItemInfo> newItems,
            List<ItemInfo> prevItems
        )
        {
            var addedItems = new List<ItemInfo>();
            var removedItems = new List<ItemInfo>();

            if (newItems != null)
            {
                foreach (var newItem in newItems)
                {
                    if (
                        prevItems == null
                        || !prevItems.Any(prevItem => ItemInfo.AreEqual(prevItem, newItem))
                    ) {
                        addedItems.Add(newItem);
                    }
                }
            }

            if (prevItems != null)
            {
                foreach (var prevItem in prevItems)
                {
                    if (
                        newItems == null
                        || !newItems.Any(newItem => ItemInfo.AreEqual(prevItem, newItem))
                    ) {
                        removedItems.Add(prevItem);
                    }
                }
            }

            return new Tuple<List<ItemInfo>, List<ItemInfo>>(
                addedItems.Count > 0 ? addedItems : null,
                removedItems.Count > 0 ? removedItems : null
            );
        }

        internal static Dictionary<GameDifficulty, List<QuestId>> CompletedQuestsDiff(
            Dictionary<GameDifficulty, List<QuestId>> newVal,
            Dictionary<GameDifficulty, List<QuestId>> prevVal
        )
        {
            if (prevVal == null && newVal == null)
                return null;

            if (prevVal == null || newVal == null)
                return newVal;

            var diff = new Dictionary<GameDifficulty, List<QuestId>>();
            var hasDiff = false;

            foreach (var pair in Zutatensuppe.D2Reader.Models.Quests.DefaultCompleteQuestIds)
            {
                var completed = newVal[pair.Key].FindAll(id => !prevVal[pair.Key].Contains(id));

                if (completed.Count() > 0)
                {
                    hasDiff = true;
                    diff.Add(pair.Key, completed);
                }
            }
            return hasDiff ? diff : null;
        }
    }
}
