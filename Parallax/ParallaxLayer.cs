using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Parallax
{
    [Serializable]
    public class ParallaxLayer
    {
        public Vector2 scrollRate;
        public GameObject layerContent;
    }
}
