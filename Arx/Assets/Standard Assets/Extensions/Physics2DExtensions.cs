using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GC = GenericComponents.Controllers.Characters;

namespace Assets.Standard_Assets.Extensions
{
    public static class Physics2DExtensions
    {
        public static IEnumerable<TComponent> GetComponents<TComponent>(this Collider2D[] colliders)
        {
            return colliders.Select(c => c.GetComponent<TComponent>()).Where(c => c != null);
        }

        public static IEnumerable<Tuple<RaycastHit2D, TComponent>> GetComponents<TComponent>(this RaycastHit2D[] hits)
        {
            return 
                hits
                    .Select(hit => 
                        new Tuple<RaycastHit2D, TComponent>(hit, hit.collider.GetComponent<TComponent>()))
                    .Where(t => t.Item2 != null);
        }

        public static GC.CharacterController[] GetCharacters(this Collider2D[] colliders)
        {
            return colliders.GetComponents<GC.CharacterController>().ToArray();
        }

        public static Tuple<RaycastHit2D, GC.CharacterController>[] GetCharacters(this RaycastHit2D[] hits)
        {
            return hits.GetComponents<GC.CharacterController>().ToArray();
        }
    }
}
