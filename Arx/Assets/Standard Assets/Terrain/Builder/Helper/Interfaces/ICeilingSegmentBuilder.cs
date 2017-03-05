using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Terrain.Builder.Helper.Interfaces
{
    public interface ICeilingSegmentBuilder
    {
        ICeilingSegmentBuilder AddCeilingSegment(LineSegment2D segment);
        ITerrainBuilderHelper AddCeilingSegmentEnd(Vector2 endPoint, float rotationInRadians);
    }
}
