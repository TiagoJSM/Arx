using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

        public static void AddIfDoesntContain(this IList<Collider2D> colliders, Collider2D collider)
        {
            if (colliders.Contains(collider))
            {
                return;
            }
            colliders.Add(collider);
        }
    }
}
