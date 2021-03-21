using System;
using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Lib.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var item in items)
                action(item);
        }
    }
}
