using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using MathHelper.DataStructures;
using Terrain.Builder.Helper;
using Terrain.Builder.Helper.Interfaces;
using MathHelper.Extensions;

namespace Terrain.Builder
{
    public class OpenTerrainBuilder : TerrainBuilder
    {
        public void BuildMeshFor(OpenTerrainField field)
        {
            field.mesh.uv = null;
            field.mesh.triangles = null;
            field.mesh.vertices = null;

            var terrainSegments = GetTerrainSegmentsFor(field.NodePath.PathSegments, field);
            var helper = OpenTerrainBuilderHelper.GetNewBuilder(field.terrainFloorHeight, field.terrainSlopeHeight, field.terrainCeilingHeight, field.cornerWidth);
            if (field.addFilling)
            {
                helper = AddFilling(helper, field.NodePath.PathSegments, field.fillingLowPoint, field.fillingUFactor, field.fillingVFactor);
            } 
            helper = AddSlopeSegments(helper, terrainSegments);
            helper = AddCeilingSegments(helper, terrainSegments);
            helper = AddFloorSegments(helper, terrainSegments);

            field.mesh.vertices = helper.Vertices;
            field.mesh.triangles = helper.Indices;
            field.mesh.colors = helper.Colors;
            field.mesh.uv = helper.Uvs;

            field.GetComponent<MeshFilter>().mesh = field.mesh;
        }

        private ITerrainBuilderHelper AddFloorSegments(ITerrainBuilderHelper helper, IEnumerable<TerrainSegments> terrainSegments)
        {
            return
                AddSegments(
                    helper,
                    terrainSegments,
                    TerrainType.Floor,
                    helper.AddFloorSegmentStart,
                    (h, s) => h.AddFloorSegment(s),
                    (h, v, f) => h.AddFloorSegmentEnd(v, f));
        }

        private ITerrainBuilderHelper AddSlopeSegments(ITerrainBuilderHelper helper, IEnumerable<TerrainSegments> terrainSegments)
        {
            return
                AddSegments(
                    helper,
                    terrainSegments,
                    TerrainType.Slope,
                    helper.AddSlopeSegmentStart,
                    (h, s) => h.AddSlopeSegment(s),
                    (h, v, f)=> h.AddSlopeSegmentEnd(v, f));
        }

        private ITerrainBuilderHelper AddCeilingSegments(ITerrainBuilderHelper helper, IEnumerable<TerrainSegments> terrainSegments)
        {
            return
                AddSegments(
                    helper,
                    terrainSegments,
                    TerrainType.Ceiling,
                    helper.AddCeilingSegmentStart,
                    (h, s) => h.AddCeilingSegment(s),
                    (h, v, f) => h.AddCeilingSegmentEnd(v, f));
        }

        private ITerrainBuilderHelper AddSegments<TBuilder>(
            ITerrainBuilderHelper helper, 
            IEnumerable<TerrainSegments> terrainSegments, 
            TerrainType terrainType,
            Func<LineSegment2D, TBuilder> addSegmentStart,
            Func<TBuilder, LineSegment2D, TBuilder> addSegment,
            Func<TBuilder, Vector2, float, ITerrainBuilderHelper> addSegmentEnd)
            where TBuilder : class
        {
            var typeSegments = terrainSegments.Where(s => s.TerrainType == terrainType);
            foreach (var typeSegment in typeSegments)
            {
                TBuilder tBuilder = null;
                typeSegment.Segments.ForEach(s =>
                {
                    if (tBuilder == null)
                    {
                        tBuilder = addSegmentStart(s);
                    }
                    else
                    {
                        tBuilder = addSegment(tBuilder, s);
                    }
                });
                var lastSlopeSegment = typeSegment.Segments.Last();
                helper = addSegmentEnd(tBuilder, lastSlopeSegment.P2, lastSlopeSegment.GetOrientationInRadians());
            }
            return helper;
        }

        private ITerrainBuilderHelper AddFilling(ITerrainBuilderHelper helper, IEnumerable<LineSegment2D> segments, float fillingLowPoint, float fillingUFactor, float fillingVFactor)
        {
            return helper.AddFilling(segments, fillingLowPoint, fillingUFactor, fillingVFactor);
        }

        private void Print<T>(IEnumerable<T> data)
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
