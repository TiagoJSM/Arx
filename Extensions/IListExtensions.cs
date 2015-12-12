using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class IListExtensions
    {
        public static void RemoveFirst(this IList list)
        {
            list.RemoveAt(0);
        }

        public static void RemoveLast(this IList list)
        {
            list.RemoveAt(list.Count - 1);
        }
    }
}
