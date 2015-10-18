using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Interfaces
{
    public interface IPath
    {
        IEnumerable<LineSegment2D> PathSegments { get; }
        IEnumerable<Vector2> PathNodes { get; }
    }
}
