using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Terrain.Editor.Decorator.Models
{
    [Serializable]
    public class DecoratorObject
    {
        public GameObject asset;
        public float rotationVariation;
        public Vector2 scaleVariation = Vector2.one;
    }
}
