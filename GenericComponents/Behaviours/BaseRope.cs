using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    public abstract class BaseRope : MonoBehaviour
    {
        public abstract Vector2[] Points { get; }
    }
}
