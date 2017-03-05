using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Terrain.Editor.Decorator.Models
{
    [Serializable]
    public class DecoratorLayer
    {
        public int minDensity = 5;
        public int maxDensity = 5;
        public DecoratorObject[] decoratorObjects;
    }
}
