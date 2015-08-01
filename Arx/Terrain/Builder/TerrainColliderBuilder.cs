using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace Terrain.Builder
{
    public static class TerrainColliderBuilder
    {
        public static void BuildColliderFor(TerrainField field)
        {
            var collider = field.GetComponent<EdgeCollider2D>();
            if (collider == null)
            {
                collider = field.gameObject.AddComponent<EdgeCollider2D>();
            }

            var colliderPoints = field.PathNodes.ToArray();
            var idx = 0;
            var previous = default(LineSegment2D?);
            foreach (var pathSegment in field.OriginPathSegments)
            {
                float radians;
                if (previous.HasValue)
                {
                    radians = (Mathf.Atan(pathSegment.Slope.Value) + Mathf.Atan(previous.Value.Slope.Value)) / 2;
                }
                else
                {
                    radians = Mathf.Atan(pathSegment.Slope.Value);
                }
                colliderPoints[idx] = 
                    (colliderPoints[idx] + new Vector2(0, field.colliderOffset))
                        .RotateAround(colliderPoints[idx], radians);
                previous = pathSegment;
                idx++;
            }

            if (previous.HasValue)
            {
                var radians = Mathf.Atan(previous.Value.Slope.Value);
                colliderPoints[idx] =
                    (colliderPoints[idx] + new Vector2(0, field.colliderOffset))
                        .RotateAround(colliderPoints[idx], radians);
            }

            collider.points = colliderPoints;
        }
    }
}
