using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using MathHelper.Extensions;

namespace Terrain.Builder
{
    public static class TerrainColliderBuilder
    {
        public static void BuildColliderFor(TerrainField field)
        {
            var collider = field.GetComponent<EdgeCollider2D>();
            if (!field.generateCollider)
            {
                if (collider != null)
                {
                    UnityEngine.Object.DestroyImmediate(collider);
                }
                return;
            }
            
            if (collider == null)
            {
                collider = field.gameObject.AddComponent<EdgeCollider2D>();
            }

            var colliderPoints = field.PathNodes.ToArray();
            var idx = 0;
            var previous = default(LineSegment2D?);
            foreach (var pathSegment in field.OriginPathSegments)
            {
                float radians = 0;
                if (previous.HasValue)
                {
                    var currentRad = pathSegment.GetOrientationInRadians();
                    var previousRad = previous.Value.GetOrientationInRadians();
                    radians = (currentRad + previousRad) / 2;

                    if ((Mathf.Abs(currentRad - previousRad)).NormalizeRadians() > Mathf.PI)
                    {
                        radians -= Mathf.PI;
                    }
                }


                colliderPoints[idx] = 
                    (colliderPoints[idx] + new Vector2(0, field.colliderOffset))
                        .RotateAround(colliderPoints[idx], radians);
                previous = pathSegment;
                idx++;
            }

            if (previous.HasValue)
            {
                var radians = previous.Value.GetOrientationInRadians();

                colliderPoints[idx] = (colliderPoints[idx] + new Vector2(0, field.colliderOffset));
            }

            collider.points = colliderPoints;
        }
    }
}
