using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using MathHelper.Extensions;
using MathHelper;
using Assets.Standard_Assets.Decorators.NodeMeshDecoration;
using Assets.Standard_Assets.GenericComponents.Builders;

namespace Assets.Standard_Assets.Decorators.Builders
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

            nodeMesh.mesh.vertices = context.Vertices.ToArray();
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
