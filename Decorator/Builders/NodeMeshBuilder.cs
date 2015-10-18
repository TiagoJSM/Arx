using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using MathHelper.Extensions;
using Decorator.NodeMeshDecoration;
using MathHelper;

namespace Decorator.Builders
{
    public static class NodeMeshBuilder
    {
        public static void BuildMeshFor(NodeMesh nodeMesh)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var indices = new List<int>();

            var segments = nodeMesh.PathSegments;

            var idx = 0;
            foreach (var segment in segments)
            {
                if(idx == 0)
                {
                    var radians = segment.GetOrientationInRadians();
                    var bottomLeft = (segment.P1 - new Vector2(0, nodeMesh.meshWidth / 2)).RotateAround(segment.P1, radians);
                    var bottomRight = (segment.P2 - new Vector2(0, nodeMesh.meshWidth / 2)).RotateAround(segment.P2, radians);
                    var topLeft = (segment.P1 + new Vector2(0, nodeMesh.meshWidth / 2)).RotateAround(segment.P1, radians);
                    var topRight = (segment.P2 + new Vector2(0, nodeMesh.meshWidth / 2)).RotateAround(segment.P2, radians);
                    var data = new[] { bottomLeft, bottomRight, topLeft, topRight }.Select(v => v.ToVector3());
                    vertices.AddRange(data);
                }
                else
                {
                    var radians = segment.GetOrientationInRadians();
                    var bottomRight = (segment.P2 - new Vector2(0, nodeMesh.meshWidth / 2)).RotateAround(segment.P2, radians);
                    var topRight = (segment.P2 + new Vector2(0, nodeMesh.meshWidth / 2)).RotateAround(segment.P2, radians);
                    var data = new[] { bottomRight, topRight }.Select(v => v.ToVector3());
                    vertices.AddRange(data);
                }
                indices.AddRange(GetIndices(idx));
                uvs.AddRange(GetUvs(idx, nodeMesh.useBezier ? nodeMesh.bezierDivisions : 0));
                idx++;
            }

            nodeMesh.mesh.uv = null;
            nodeMesh.mesh.triangles = null;
            nodeMesh.mesh.vertices = null;

            nodeMesh.mesh.vertices = vertices.ToArray();
            nodeMesh.mesh.triangles = indices.ToArray();
            nodeMesh.mesh.uv = uvs.ToArray();

            nodeMesh.GetComponent<MeshFilter>().mesh = nodeMesh.mesh;
        }

        private static int[] GetIndices(int idx)
        {
            if (idx == 0)
            {
                return new[] 
                {
                    0, 3, 1,
                    0, 2, 3
                };
            }
            if (idx == 1)
            {
                return new[] 
                {
                    1, 5, 4,
                    1, 3, 5
                };
            }
            return new[] 
            { 
                2 * idx, 2 * idx + 3, 2 * idx + 2,
                2 * idx, 2 * idx + 1, 2 * idx + 3,
            };
        }

        private static Vector2[] GetUvs(int idx, int bezierDivisions)
        {
            float divisionFactor = bezierDivisions + 1;
            if (idx == 0)
            {
                return new[] 
                {
                    new Vector2(0, 0),
                    new Vector2(1.0f / divisionFactor, 0),
                    new Vector2(0, 1.0f),
                    new Vector2(1.0f / divisionFactor, 1.0f)
                };
            }
            return new[] 
            { 
                new Vector2((idx + 1.0f) / divisionFactor, 0),
                new Vector2((idx + 1.0f) / divisionFactor, 1.0f)
            };
        }

        private static void Print<T>(IEnumerable<T> data)
        {
            string result = string.Empty;
            foreach (var d in data)
            {
                result = result + d.ToString() + ", ";
            }
            Debug.Log(result);
        }
    }
}
