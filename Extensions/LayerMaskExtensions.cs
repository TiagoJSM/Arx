using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool IsInAnyLayer(this LayerMask mask, GameObject go)
        {
            int objLayerMask = (1 << go.layer);
            return (mask.value & objLayerMask) > 0;
        }
    }
}
