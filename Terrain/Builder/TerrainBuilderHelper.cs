using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain.Builder.Helper;
using Terrain.Builder.Helper.SegmentBuilders;
using UnityEngine;
using Extensions;

namespace Terrain.Builder
{
    public class TerrainBuilderHelper
    {
        private float _floorHeight;
        private float _slopeHeight;
        private float _ceilingHeight;
        private float _cornerWidth;

        protected BuilderDataContext DataContext { get; private set; }
        protected FloorSegmentBuilder FloorBuilder { get; private set; }
        protected SlopeSegmentBuilder SlopeBuilder { get; private set; }
        protected CeilingSegmentBuilder CeilingBuilder { get; private set; }

        public Vector3[] Vertices
        {
            get { return DataContext.Vertices.Select(v => v.ToVector3()).ToArray(); }
        }

        public int[] Indices
        {
            get { return DataContext.Indices.ToArray(); }
        }

        public Vector2[] Uvs
        {
            get { return DataContext.Uvs.ToArray(); }
        }

        public Color[] Colors
        {
            get { return DataContext.Colors.ToArray(); }
        }

        protected TerrainBuilderHelper(
            float floorHeight,
            float slopeHeight,
            float ceilingHeight,
            float cornerWidth)
        {
            _floorHeight = floorHeight;
            _slopeHeight = slopeHeight;
            _ceilingHeight = ceilingHeight;
            _cornerWidth = cornerWidth;
            DataContext = new BuilderDataContext();
            FloorBuilder = new FloorSegmentBuilder(DataContext, _floorHeight, _cornerWidth);
            SlopeBuilder = new SlopeSegmentBuilder(DataContext, _slopeHeight, _cornerWidth);
            CeilingBuilder = new CeilingSegmentBuilder(DataContext, _ceilingHeight, _cornerWidth);
        }
    }
}
