using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace Terrain
{
    public static class TerrainBuilder
    {
        private static int[] _firstTwoTriangleIndices =
            new[]
            {
                0, 3, 1,
                0, 2, 3
            };
        private static int[] _secondTwoTriangleIndices =
            new[]
            {
                1, 5, 4,
                1, 3, 5
            };

        public static void BuildMeshFor(TerrainField field)
        {
            //mesh.name = "random";
            field.mesh.uv = null;
            field.mesh.triangles = null;
            field.mesh.vertices = null;
            field.mesh.vertices = GetVerticesFor(field);
            field.mesh.triangles = GetTrianglesFor(field);
            field.mesh.uv = GetUvFor(field);
            field.GetComponent<MeshFilter>().mesh = field.mesh;
        }

        private static Vector3[] GetVerticesFor(TerrainField field)
        {
            return field.PathSegments.SelectMany((seg, idx) =>
            {
                if (idx == 0)
                {
                    return new Vector3[]
                    {
                        seg.P1.ToVector3(), 
                        seg.P2.ToVector3(),
                        seg.P1.ToVector3() + new Vector3(0, 0.5f),
                        seg.P2.ToVector3() + new Vector3(0, 0.5f)
                    };
                }
                return new Vector3[]
                    {
                        seg.P2.ToVector3(),
                        seg.P2.ToVector3() + new Vector3(0, 0.5f)
                    };
            })
            .ToArray();
        }

        private static int[] GetTrianglesFor(TerrainField field)
        {
            var previousIndices = new int[_firstTwoTriangleIndices.Length];
            _firstTwoTriangleIndices.CopyTo(previousIndices, 0);

            return field.PathSegments.SelectMany((seg, idx) =>
                {
                    if (idx == 0)
                    {
                        var result = new int[_firstTwoTriangleIndices.Length];
                        _firstTwoTriangleIndices.CopyTo(result, 0);
                        return result;
                    }
                    if (idx == 1)
                    {
                        var result = new int[_secondTwoTriangleIndices.Length];
                        _secondTwoTriangleIndices.CopyTo(result, 0);
                        return result;
                    }
                    return new int[]
                    {
                        idx * 2, idx * 2 + 3, idx * 2 + 2,
                        idx * 2, idx * 2 + 1, idx * 2 + 3
                    };
                })
                .ToArray();
        }

        private static Vector2[] GetUvFor(TerrainField field)
        {
            return field.mesh.vertices.Select(v => v.ToVector2() / 2).ToArray();
        }
    }
}
