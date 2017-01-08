using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace _2DDynamicCamera.Interfaces
{
    public interface ICameraTarget
    {
        Vector3 Position { get; }
        Vector2? Damping { get; }
    }
}
