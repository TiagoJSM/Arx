using Extensions;
using GenericComponents;
using MathHelper.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.GenericComponents.Behaviours
{
    public class NodePathBehaviour : MonoBehaviour, IEnumerable<Vector2>
    {
        [SerializeField]
        private NodePath _nodePath;

        public NodePath NodePath { get { return _nodePath; } }

        public IEnumerable<BezierLineSegment2D> BezierLinePathSegments
        {
            get
            {
                var position = this.transform.position.ToVector2();
                var controlPointPairs = NodePath.BiezerControlPoints.ToSequencePairs().ToArray();
                return InScenePathSegments.Select((s, i) =>
                {
                    var controlPoints = controlPointPairs[i];
                    return new BezierLineSegment2D(s, controlPoints.Item1 + position, controlPoints.Item2 + position);
                });
            }
        }

        public IEnumerable<LineSegment2D> BezierPathSegments
        {
            get
            {
                var position = this.transform.position.ToVector2();
                return NodePath.PathSegments.Select((s, i) =>
                {
                    s.P1 = s.P1 + position;
                    s.P2 = s.P2 + position;
                    return s;
                });
            }
        }

        public IEnumerable<Vector2> PathNodes
        {
            get
            {
                var position = this.transform.position.ToVector2();
                return _nodePath.PathNodes.Select(n => n + position);
            }
        }

        public Vector2 this[int index]
        {
            get
            {
                var position = this.transform.position.ToVector2();
                return NodePath[index] + position;
            }
            set
            {
                var position = this.transform.position.ToVector2();
                NodePath[index] = value - position;
            }
        }

        public NodePathBehaviour()
        {
            _nodePath = new NodePath();
        }

        public void AddPathNode(Vector2 vector2)
        {
            var position = this.transform.position.ToVector2();
            NodePath.AddPathNode(vector2 - position);
        }

        public Vector2 GetBezierControlPointAt(int idx)
        {
            var position = this.transform.position.ToVector2();
            return NodePath.GetBezierControlPointAt(idx) + position;
        }

        public void SetBezierControlPointAt(int idx, Vector2 translated)
        {
            var position = this.transform.position.ToVector2();
            NodePath.SetBezierControlPointAt(idx, translated - position);
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return InScenePathNodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerable<Vector2> InScenePathNodes
        {
            get
            {
                var position = this.transform.position.ToVector2();
                return NodePath.Select(n => n + position);
            }
        }

        public IEnumerable<LineSegment2D> InScenePathSegments
        {
            get
            {
                var position = this.transform.position.ToVector2();
                return NodePath.OriginControlPathSegments.Select(s => s + position);
            }
        }

    }
}
