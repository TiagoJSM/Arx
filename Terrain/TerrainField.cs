using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using System.Collections;
using MathHelper.DataStructures;
using Extensions;
using GenericComponents;

namespace Terrain
{
    public class TerrainField : NodePath
    {
        public Mesh mesh;
        [Header("Terrain shape")]
        public float maxSegmentLenght = 2;
        public float fillingLowPoint = 0;
        public float floorTerrainMaximumSlope = 1.0f;
        public bool addFilling = true;
        public float cornerWidth = 0.5f;
        public float terrainFloorHeight = 0.5f;
        public float terrainSlopeHeight = 0.5f;
        public float terrainCeilingHeight = 0.5f;

        [Header("Terrain collider")]
        public float colliderOffset = 0;
        public bool generateCollider = true;

        [Header("Terrain texturing")]
        public float fillingUFactor = 1.0f;
        public float fillingVFactor = 1.0f;
        public Shader shader;
    }
}
