using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Terrain.Editor.Decorator.Models
{
    public class DecoratorForm : ScriptableObject
    {
        public TerrainField terrain;
        public GameObject decorationContainer;

        public DecoratorLayer floorLayer;
        public DecoratorLayer slopeLayer;
        public DecoratorLayer ceilingLayer;
    }
}
