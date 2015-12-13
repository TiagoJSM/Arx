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
    [Serializable]
    public class PathNode //: UnityEngine.Object
    {
        public Vector2 position;
        public Vector2 startBezierOffset;
        public Vector2 endBezierOffset;

        public Vector2 StartBezier { get { return position + startBezierOffset; } }
        public Vector2 EndBezier { get { return position + endBezierOffset; } }
    }

    [Serializable]
    public class NodePath : IEnumerable<Vector2>, IPath
    {
        [SerializeField]
        private List<PathNode> _pathNodes;

        public bool UseBezier { get; set; }
        public int BezierDivisions { get; set; }

        public int VerticeCount { get { return _pathNodes.Count; } }
        public int BezierControlPointsCount { get { return (_pathNodes.Count - 1) * 2; } }

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

        public IEnumerable<LineSegment2D> OriginControlPathSegments
        {
            get
            {
                return _pathNodes.ToPairs().Select(p => new LineSegment2D(p.Item1.position, p.Item2.position));
            }
        }

        public IEnumerable<BezierLineSegment2D> BezierOriginPathSegments
        {
            get
            {
                var controlPointPairs =
                    _pathNodes
                        .ToPairs()
                        .ToArray();

                return 
                    OriginControlPathSegments.Select((s, i) => 
                    {
                        var controlPoints = controlPointPairs[i];
                        return new BezierLineSegment2D(s, controlPoints.Item1.StartBezier, controlPoints.Item2.EndBezier);
                    });
            }
        }
        public IEnumerable<Vector2> ControlPathNodes
        {
            get
            {
                return _pathNodes.Select(n => n.position);
            }
        }

        public Vector2 this[int index]
        {
            get
            {
                return _pathNodes[index].position;
            }
            set
            {
                _pathNodes[index].position = value;
            }
        }

        public void AddPathNode(Vector2 vector2)
        {
            _pathNodes.Add(
                new PathNode()
                {
                    position = vector2
                });
        }

        public void RemovePathNodeAt(int index)
        {
            _pathNodes.RemoveAt(index);
        }

        public IEnumerable<Vector2> BiezerControlPoints
        {
            get
            {
                var points =
                    _pathNodes
                        .SelectMany(n => new[] { n.position + n.endBezierOffset, n.position + n.startBezierOffset })
                        .ToList();
                points.RemoveFirst();
                points.RemoveLast();
                return points;
            }
        }

        public NodePath()
        {
            _pathNodes = new List<PathNode>();
        }

        public void DivideSegment(int segmentIdx)
        {
            var segmentStartPoint = _pathNodes[segmentIdx];
            var segmentEndPoint = _pathNodes[segmentIdx + 1];

            var halfLenght = (segmentEndPoint.position - segmentStartPoint.position) / 2;
            var divisionPoint = segmentStartPoint.position + halfLenght;
            _pathNodes.Insert(
                segmentIdx + 1, 
                new PathNode()
                {
                    position = divisionPoint
                });
        }

        public Vector2 GetBezierControlPointAt(int idx)
        {
            var pointIndex = GetControlPointIndexForBezierPoint(idx);
            if (idx % 2 == 0)
            {
                return _pathNodes[pointIndex].startBezierOffset + _pathNodes[pointIndex].position;
            }
            else
            {
                return _pathNodes[pointIndex].endBezierOffset + _pathNodes[pointIndex].position;
            }
        }

        public void SetBezierControlPointAt(int idx, Vector2 point)
        {
            var pointIndex = GetControlPointIndexForBezierPoint(idx);
            var controlPoint = _pathNodes[GetControlPointIndexForBezierPoint(idx)];
            var bezierValue = point - controlPoint.position;
            if (idx % 2 == 0)
            {
                _pathNodes[pointIndex].startBezierOffset = bezierValue;
            }
            else
            {
                _pathNodes[pointIndex].endBezierOffset = bezierValue;
            }
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return _pathNodes.Select(n => n.position).GetEnumerator();
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
            if (!UseBezier || BezierDivisions <= 0)
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
            if (!UseBezier || BezierDivisions <= 0)
            {
                return ControlPathNodes;
            }
            return GetBezierPathNodes();
        }

        private IEnumerable<Vector2> GetBezierPathNodes()
        {
            float tRatio = 1.0f / ((float)(BezierDivisions + 1));
            var bezierOriginSegments = BezierOriginPathSegments;

            var r = 
                bezierOriginSegments
                    .SelectMany((s, i) =>
                    {
                        var values = new List<Vector2>();
                        values.Add(s.LineSegment.P1);

                        var currentRatio = tRatio;
                        for (var count = 0; count < BezierDivisions; count++, currentRatio += tRatio)
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
            return r;
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
