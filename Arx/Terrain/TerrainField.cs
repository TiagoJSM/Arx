using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using System.Collections;
using MathHelper.DataStructures;
using Extensions;

namespace Terrain
{
    public class TerrainField : MonoBehaviour, IEnumerable<Vector2>
    {
        [SerializeField]
        private List<Vector2> _pathNodes;

        public Mesh mesh;
        [Header("Terrain shape")]
        public float maxSegmentLenght = 2;
        public float fillingLowPoint = 0;
        public float floorTerrainMaximumSlope = 1.0f;
        public bool addFilling = true;
        public float terrainHeight = 0.5f;
        public float cornerWidth = 0.5f;

        [Header("Terrain collider")]
        public float colliderOffset = 0;
        public bool generateCollider = true;

        [Header("Terrain texturing")]
        public float fillingUFactor = 1.0f;
        public float fillingVFactor = 1.0f;
        public Shader shader;

        public int VerticeCount { get { return _pathNodes.Count; } }
        public IEnumerable<LineSegment2D> PathSegments
        {
            get
            {
                return InScenePathNodes.ToPairs().Select(p => new LineSegment2D(p.Item1, p.Item2));
            }
        }
        public IEnumerable<LineSegment2D> OriginPathSegments
        {
            get
            {
                return _pathNodes.ToPairs().Select(p => new LineSegment2D(p.Item1, p.Item2));
            }
        }
        public IEnumerable<Vector2> PathNodes
        {
            get
            {
                return _pathNodes;
            }
        }

        public Vector2 this[int index]
        {
            get
            {
                var position = this.transform.position.ToVector2();
                return _pathNodes[index] + position;
            }
            set
            {
                var position = this.transform.position.ToVector2();
                _pathNodes[index] = value - position;
            }
        }

        public void AddPathNode(Vector2 vector2)
        {
            var position = this.transform.position.ToVector2();
            _pathNodes.Add(vector2 - position);
        }

        private IEnumerable<Vector2> InScenePathNodes
        {
            get
            {
                var position = this.transform.position.ToVector2();
                return _pathNodes.Select(n => n + position);
            }
        }

        public TerrainField()
        {
            _pathNodes = new List<Vector2>();
        }

        public void DivideSegment(int segmentIdx)
        {
            var segmentStartPoint = _pathNodes[segmentIdx];
            var segmentEndPoint = _pathNodes[segmentIdx + 1];

            var halfLenght = (segmentEndPoint - segmentStartPoint) / 2;
            var divisionPoint = segmentStartPoint + halfLenght;
            _pathNodes.Insert(segmentIdx + 1, divisionPoint);
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return InScenePathNodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
