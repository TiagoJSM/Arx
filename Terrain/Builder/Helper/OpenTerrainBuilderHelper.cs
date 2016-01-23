using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain.Builder.Helper.Interfaces;
using UnityEngine;
using Extensions;
using MathHelper.Extensions;
using Terrain.Utils;
using Utils;
using Terrain.Builder.Helper.SegmentBuilders;

namespace Terrain.Builder.Helper
{
    public class OpenTerrainBuilderHelper : 
        TerrainBuilderHelper, 
        ITerrainBuilderHelper, 
        IFloorSegmentBuilder, 
        ISlopeSegmentBuilder, 
        ICeilingSegmentBuilder
    {
        public static ITerrainBuilderHelper GetNewBuilder(
            float floorHeight, 
            float slopeHeight, 
            float ceilingHeight, 
            float cornerWidth, 
            float fillingLowPoint, 
            float fillingUFactor,
            float fillingVFactor)
        {
            return new OpenTerrainBuilderHelper(
                floorHeight, 
                slopeHeight, 
                ceilingHeight, 
                cornerWidth, 
                fillingLowPoint, 
                fillingUFactor, 
                fillingVFactor);
        }

        public OpenTerrainBuilderHelper(
            float floorHeight,
            float slopeHeight,
            float ceilingHeight, 
            float cornerWidth,
            float fillingLowPoint,
            float fillingUFactor,
            float fillingVFactor)
            : base(
                floorHeight,
                slopeHeight,
                ceilingHeight,
                cornerWidth,
                fillingLowPoint,
                fillingUFactor,
                fillingVFactor)
        {
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
