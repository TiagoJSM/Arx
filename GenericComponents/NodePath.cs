using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using System.Collections;
using GenericComponents.Interfaces;
using MathHelper;

namespace GenericComponents
{
    public class NodePath : MonoBehaviour, IEnumerable<Vector2>, IPath
    {
        [SerializeField]
        private List<Vector2> _pathNodes;
        [SerializeField]
        private List<Vector2> _bezierControlPointsOffset;

        public bool useBezier;
        public int bezierDivisions;

        public int VerticeCount { get { return _pathNodes.Count; } }
        public int BezierControlPointsCount { get { return _bezierControlPointsOffset.Count; } }

        public IEnumerable<LineSegment2D> PathSegments
        {
            get { return GetSegments(); }
        }
        public IEnumerable<Vector2> PathNodes 
        { 
            get 
            {
                return GetNodes();
            } 
        }

        public IEnumerable<LineSegment2D> ControlPathSegments
        {
            get
            {
                return InScenePathNodes.ToPairs().Select(p => new LineSegment2D(p.Item1, p.Item2));
            }
        }
        public IEnumerable<LineSegment2D> OriginControlPathSegments
        {
            get
            {
                return _pathNodes.ToPairs().Select(p => new LineSegment2D(p.Item1, p.Item2));
            }
        }
        public IEnumerable<BezierLineSegment2D> BezierPathSegments
        {
            get
            {
                var controlPointPairs = InSceneBiezerControlPoints.ToSequencePairs().ToArray();
                return ControlPathSegments.Select((s, i) => 
                    {
                        var controlPoints = controlPointPairs[i];
                        return new BezierLineSegment2D(s, controlPoints.Item1, controlPoints.Item2);
                    });
            }
        }
        public IEnumerable<BezierLineSegment2D> BezierOriginPathSegments
        {
            get
            {
                var controlPointPairs =
                    _bezierControlPointsOffset
                        .Select((n, i) => n + _pathNodes[GetControlPointIndexForBezierPoint(i)])
                        .ToSequencePairs()
                        .ToArray();

                return 
                    OriginControlPathSegments.Select((s, i) => 
                    {
                        var controlPoints = controlPointPairs[i];
                        return new BezierLineSegment2D(s, controlPoints.Item1, controlPoints.Item2);
                    });
            }
        }
        public IEnumerable<Vector2> ControlPathNodes
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
            if (_pathNodes.Count > 1)
            {
                _bezierControlPointsOffset.Add(Vector2.zero);
                _bezierControlPointsOffset.Add(Vector2.zero);
            }
        }

        public void RemovePathNodeAt(int index)
        {
            _pathNodes.RemoveAt(index);
        }

        private IEnumerable<Vector2> InScenePathNodes
        {
            get
            {
                var position = this.transform.position.ToVector2();
                return _pathNodes.Select(n => n + position);
            }
        }
        private IEnumerable<Vector2> InSceneBiezerControlPoints
        {
            get
            {
                return _bezierControlPointsOffset.Select((n, i) => n + this[GetControlPointIndexForBezierPoint(i)]);
            }
        }

        public NodePath()
        {
            _pathNodes = new List<Vector2>();
            _bezierControlPointsOffset = new List<Vector2>();
        }

        public void DivideSegment(int segmentIdx)
        {
            var segmentStartPoint = _pathNodes[segmentIdx];
            var segmentEndPoint = _pathNodes[segmentIdx + 1];

            var halfLenght = (segmentEndPoint - segmentStartPoint) / 2;
            var divisionPoint = segmentStartPoint + halfLenght;
            _pathNodes.Insert(segmentIdx + 1, divisionPoint);
            var endOffsetIdx = GetBezierPointForControlPoint(segmentIdx + 1, false);
            var startOffsetIdx = GetBezierPointForControlPoint(segmentIdx + 1, true);
            _bezierControlPointsOffset.Insert(endOffsetIdx, Vector2.zero);
            _bezierControlPointsOffset.Insert(startOffsetIdx, Vector2.zero);
        }

        public Vector2 GetBezierControlPointAt(int idx)
        {
            return _bezierControlPointsOffset[idx] + this[GetControlPointIndexForBezierPoint(idx)];
        }

        public void SetBezierControlPointAt(int idx, Vector2 point)
        {
            var controlPoint = this[GetControlPointIndexForBezierPoint(idx)];
            _bezierControlPointsOffset[idx] = point - controlPoint;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return InScenePathNodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private int GetBezierPointForControlPoint(int idx, bool start)
        {
            var bezierIdx = idx * 2;
            if (start)
            {
                bezierIdx++;
            }
            return bezierIdx;
        }

        private int GetControlPointIndexForBezierPoint(int idx)
        {
            var result = idx / 2;
            if (idx % 2 != 0)
            {
                result++;
            }
            return result;
        }

        private IEnumerable<LineSegment2D> GetSegments()
        {
            if (!useBezier || bezierDivisions <= 0)
            {
                return OriginControlPathSegments;
            }

            return
                GetBezierPathNodes()
                    .ToPairs()
                    .Select(p => new LineSegment2D(p.Item1, p.Item2));
        }

        private IEnumerable<Vector2> GetNodes()
        {
            if (!useBezier || bezierDivisions <= 0)
            {
                return ControlPathNodes;
            }
            return GetBezierPathNodes();
        }

        private IEnumerable<Vector2> GetBezierPathNodes()
        {
            float tRatio = 1.0f / ((float)(bezierDivisions + 1));
            var bezierOriginSegments = BezierOriginPathSegments;

            return 
                bezierOriginSegments
                    .SelectMany((s, i) =>
                    {
                        var values = new List<Vector2>();
                        values.Add(s.LineSegment.P1);

                        var currentRatio = tRatio;
                        for (var count = 0; count < bezierDivisions; count++, currentRatio += tRatio)
                        {
                            var point = Bezier.CalculateBezierPoint(currentRatio, s.LineSegment.P1, s.P1ControlPoint, s.P2ControlPoint, s.LineSegment.P2);
                            values.Add(point);
                        }

                        if ((bezierOriginSegments.Count() - 1) == i)
                        {
                            values.Add(s.LineSegment.P2);
                        }
                        return values;
                    });
        }
    }
}
