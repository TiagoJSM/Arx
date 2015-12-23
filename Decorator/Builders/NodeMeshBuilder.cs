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
using GenericComponents.Builders;

namespace Decorator.Builders
{
    public static class NodeMeshBuilder
    {
        public static void BuildMeshFor(NodeMesh nodeMesh)
        {
            var context = new BuilderDataContext();
            var builder = new NodeMeshSegmentBuilder(context, nodeMesh.meshWidth);

            var segments = nodeMesh.NodePath.PathSegments;

            var idx = 0;
            foreach (var segment in segments)
            {
                if(idx == 0)
                {
                    builder.AddFirstSegment(segment);
                }
                else
                {
                    builder.AddNextSegment(segment);
                }
                idx++;
            }

            nodeMesh.mesh.uv = null;
            nodeMesh.mesh.triangles = null;
            nodeMesh.mesh.vertices = null;

            nodeMesh.mesh.vertices = context.Vertices.ToVector3s().ToArray();
            nodeMesh.mesh.triangles = context.Indices.ToArray();
            nodeMesh.mesh.uv = context.Uvs.ToArray();

            nodeMesh.GetComponent<MeshFilter>().mesh = nodeMesh.mesh;
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
