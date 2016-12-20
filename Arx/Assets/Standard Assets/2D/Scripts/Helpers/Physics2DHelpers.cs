using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Helpers
{
    public static class Physics2DHelpers
    {
        public static TTarget[] OverlapAreaAll<TTarget>(Vector2 point1, Vector2 point2/*, LayerMask layers = new LayerMask()*/)
        {
            return
                Physics2D
                    .OverlapAreaAll(point1, point2/*, layers*/)
                    .Select(c => c.GetComponent<TTarget>())
                    .Where(c => c != null)
                    .Distinct()
                    .ToArray();
        }
    }
}
