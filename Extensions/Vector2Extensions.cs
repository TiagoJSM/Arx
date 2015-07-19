using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Extensions
{
    public static class Vector2Extensions
    {
        public static Vector3 ToVector3(this Vector2 vec2)
        {
            return new Vector3(vec2.x, vec2.y);
        }
    }
}
