﻿using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using System.Collections;

namespace GenericComponents.Behaviours
{
    public class NodePathBehaviour : MonoBehaviour, IEnumerable<Vector2>
    {
        [SerializeField]
        private NodePath _nodePath;

        public NodePath NodePath { get { return _nodePath; } }

        public IEnumerable<LineSegment2D> ControlPathSegments
        {
            get
            {
                return InScenePathNodes.ToPairs().Select(p => new LineSegment2D(p.Item1, p.Item2));
            }
        }

        public IEnumerable<BezierLineSegment2D> BezierPathSegments
        {
            get
            {
                var position = this.transform.position.ToVector2();
                var controlPointPairs = NodePath.BiezerControlPoints.ToSequencePairs().ToArray();
                return ControlPathSegments.Select((s, i) =>
                {
                    var controlPoints = controlPointPairs[i];
                    return new BezierLineSegment2D(s, controlPoints.Item1 + position, controlPoints.Item2 + position);
                });
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

        private IEnumerable<Vector2> InScenePathNodes
        {
            get
            {
                var position = this.transform.position.ToVector2();
                return NodePath.Select(n => n + position);
            }
        }
    }
}