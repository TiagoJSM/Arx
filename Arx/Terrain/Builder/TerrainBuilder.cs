using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using MathHelper.DataStructures;

namespace Terrain.Builder
{
    public static class TerrainBuilder
    {
        private static int[] _firstTwoTriangleIndices =
            new[]
            {
                0, 3, 1,
                0, 2, 3
            };
        private static int[] _secondTwoTriangleIndices =
            new[]
            {
                1, 5, 4,
                1, 3, 5
            };

        public static void BuildMeshFor(TerrainField field)
        {
            //mesh.name = "random";
            field.mesh.uv = null;
            field.mesh.triangles = null;
            field.mesh.vertices = null;

            var terrainSegments = GetTerrainSegmentsFor(field);

            field.mesh.vertices = GetVerticesFor(terrainSegments);
            field.mesh.triangles = GetTrianglesFor(terrainSegments);
            field.mesh.colors = GetColorsFor(terrainSegments);
            field.mesh.uv = GetUvFor(terrainSegments);
            Print(field.mesh.uv);
            field.GetComponent<MeshFilter>().mesh = field.mesh;
        }

        private static IEnumerable<TerrainSegments> GetTerrainSegmentsFor(TerrainField field)
        {
            var terrainSegments = new List<TerrainSegments>();
            
            var segments = new TerrainSegments();
            var terrainType = TerrainType.Floor;

            foreach (var seg in field.OriginPathSegments)
            {
                var segmentTerrainType = GetTerrainTypeFromSegment(seg);
                if (segmentTerrainType != terrainType)
                {
                    if (segments.Segments.Count > 0)
                    {
                        terrainSegments.Add(segments);
                        segments = new TerrainSegments();
                    }
                    terrainType = segmentTerrainType;
                }
                
                segments.TerrainType = segmentTerrainType;
                segments.Segments.Add(seg);
            }

            if (segments.Segments.Count > 0)
            {
                terrainSegments.Add(segments);
            }

            return terrainSegments;
        }

        private static Color[] GetColorsFor(IEnumerable<TerrainSegments> segments)
        {
            var result = new List<Color>();
            result.AddRange(GetColorsFor(segments, TerrainType.Slope, new Color(0f, 0f, 0f, 0f)));
            result.AddRange(GetColorsFor(segments, TerrainType.Floor, new Color(0f, 0f, 0f, 0.5f)));
            return result.ToArray();
        }

        private static Color[] GetColorsFor(IEnumerable<TerrainSegments> segments, TerrainType type, Color color)
        {
            segments = segments.Where(ts => ts.TerrainType == type);
            var result = new List<Color>();

            foreach (var segment in segments)
            {
                var consecutiveSegment = false;
                var colors = segment.Segments.SelectMany(seg =>
                {
                    if (!consecutiveSegment)
                    {
                        consecutiveSegment = true;
                        return new Color[]
                        {
                            color, 
                            color,
                            color,
                            color
                        };
                    }
                    return new Color[]
                    {
                        color,
                        color
                    };
                });
                result.AddRange(colors);
            }

            return result.ToArray();
        }

        private static Vector3[] GetVerticesFor(IEnumerable<TerrainSegments> segments)
        {
            var result = new List<Vector3>();
            result.AddRange(GetVerticesFor(segments, TerrainType.Slope));
            result.AddRange(GetVerticesFor(segments, TerrainType.Floor));
            return result.ToArray();
        }

        private static Vector3[] GetVerticesFor(IEnumerable<TerrainSegments> segments, TerrainType type)
        {
            segments = segments.Where(ts => ts.TerrainType == type);
            var result = new List<Vector3>();

            foreach (var segment in segments)
            {
                var consecutiveSegment = false;
                var vectors = segment.Segments.SelectMany(seg =>
                {
                    if (!consecutiveSegment)
                    {
                        consecutiveSegment = true;
                        return new Vector3[]
                        {
                            seg.P1.ToVector3(), 
                            seg.P2.ToVector3(),
                            seg.P1.ToVector3() + new Vector3(0, 0.5f),
                            seg.P2.ToVector3() + new Vector3(0, 0.5f)
                        };
                    }
                    return new Vector3[]
                    {
                        seg.P2.ToVector3(),
                        seg.P2.ToVector3() + new Vector3(0, 0.5f)
                    };
                });
                result.AddRange(vectors);
            }

            return result.ToArray();
        }

        private static int[] GetTrianglesFor(IEnumerable<TerrainSegments> segments)
        {
            var result = new List<int>();
            var slopeIndices = GetTrianglesFor(segments, TerrainType.Slope);
            var offset = slopeIndices.Any() ? slopeIndices.Last() + 1 : 0;
            var floorIndices = GetTrianglesFor(segments, TerrainType.Floor, offset);
            result.AddRange(slopeIndices);
            result.AddRange(floorIndices);
            return result.ToArray();
        }

        private static int[] GetTrianglesFor(IEnumerable<TerrainSegments> segments, TerrainType type, int segmentIndiceOffset = 0)
        {
            segments = segments.Where(ts => ts.TerrainType == type);
            var result = new List<int>();

            foreach (var segment in segments)
            {
                var indices = segment.Segments.SelectMany((seg, idx) =>
                {
                    if (idx == 0)
                    {
                        var segmentIndices = new int[_firstTwoTriangleIndices.Length];
                        _firstTwoTriangleIndices.CopyTo(segmentIndices, 0);
                        return segmentIndices;
                    }
                    if (idx == 1)
                    {
                        var segmentIndices = new int[_secondTwoTriangleIndices.Length];
                        _secondTwoTriangleIndices.CopyTo(segmentIndices, 0);
                        return segmentIndices;
                    }
                    return new int[]
                    {
                        idx * 2, idx * 2 + 3, idx * 2 + 2,
                        idx * 2, idx * 2 + 1, idx * 2 + 3
                    };
                });

                //add offset, this is needed since sometimes the segments can have breaks between due to slopes
                indices = indices.Select(i => i + segmentIndiceOffset);

                result.AddRange(indices);
                //by the logic of how the triangles are built the biggest indice is always the last
                segmentIndiceOffset = indices.Last() + 1;
            }
            return result.ToArray();
        }

        /*private static Vector2[] GetUvFor(Mesh mesh)
        {
            return mesh.vertices.Select(v => v.ToVector2() / 2).ToArray();
        }*/

        private static Vector2[] GetUvFor(IEnumerable<TerrainSegments> segments)
        {
            var result = new List<Vector2>();
            var slopeUvs = GetUvFor(segments, TerrainType.Slope);
            var floorUvs = GetUvFor(segments, TerrainType.Floor);
            result.AddRange(slopeUvs);
            result.AddRange(floorUvs);
            return result.ToArray();
        }

        private static Vector2[] GetUvFor(IEnumerable<TerrainSegments> segments, TerrainType type)
        {
            segments = segments.Where(ts => ts.TerrainType == type);
            var result = new List<Vector2>();

            foreach (var segment in segments)
            {
                var consecutiveSegment = false;
                var vectors = segment.Segments.SelectMany((seg, idx) =>
                {
                    if (!consecutiveSegment)
                    {
                        consecutiveSegment = true;
                        return new Vector2[]
                        {
                            new Vector2(), 
                            new Vector2(1, 0),
                            new Vector2(0, 1),
                            new Vector2(1, 1)
                        };
                    }
                    return new Vector2[]
                    {
                        new Vector2(idx + 1, 0),
                        new Vector2(idx + 1, 1),
                    };
                });
                result.AddRange(vectors);
            }

            return result.ToArray();
        }

        private static TerrainType GetTerrainTypeFromSegment(LineSegment2D segment)
        {
            if (segment.Slope == null || Math.Abs(segment.Slope.Value) >= 1.0f)
            {
                return TerrainType.Slope;
            }
            return TerrainType.Floor;
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
