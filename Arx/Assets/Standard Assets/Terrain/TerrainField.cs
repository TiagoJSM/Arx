using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using System.Collections;
using MathHelper.DataStructures;
using GenericComponents;
using GenericComponents.Behaviours;
using Assets.Standard_Assets.GenericComponents.Behaviours;

namespace Assets.Standard_Assets.Terrain
{
    public class TerrainField : NodePathBehaviour
    {
        public Mesh mesh;
        [Header("Terrain floor shape")]
        public float maxFloorSegmentLenght = 2;
        public float floorCornerWidth = 0.5f;
        public float terrainFloorHeight = 0.5f;

        [Header("Terrain slope shape")]
        public float maxSlopeSegmentLenght = 2;
        public float slopeCornerWidth = 0.5f;
        public float terrainSlopeHeight = 0.5f;

        [Header("Terrain ceiling shape")]
        public float maxCeilingSegmentLenght = 2;
        public float terrainCeilingHeight = 0.5f;
        public float ceilingCornerWidth = 0.5f;

        [Header("Terrain filling")]
        public float fillingUFactor = 1.0f;
        public float fillingVFactor = 1.0f;

        [Header("Terrain global shape")]
        public float floorTerrainMaximumSlope = 1.0f;

        [Header("Terrain collider")]
        public float colliderOffset = 0;
        public bool generateCollider = true;
        //public Shader shader;
    }
}
