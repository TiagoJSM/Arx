using UnityEngine;
using System.Collections.Generic;
using Utils.DataStructures;

namespace Utils
{
    public class Triangulator
    {
        public static int[] TriangulatePolygon(Vector2[] XZofVertices)
        {
            int VertexCount = XZofVertices.Length;
            float xmin = XZofVertices[0].x;
            float ymin = XZofVertices[0].y;
            float xmax = xmin;
            float ymax = ymin;
            for (int ii1 = 1; ii1 < VertexCount; ii1++)
            {
                if (XZofVertices[ii1].x < xmin)
                {
                    xmin = XZofVertices[ii1].x;
                }
                else if (XZofVertices[ii1].x > xmax)
                {
                    xmax = XZofVertices[ii1].x;
                }
                if (XZofVertices[ii1].y < ymin)
                {
                    ymin = XZofVertices[ii1].y;
                }
                else if (XZofVertices[ii1].y > ymax)
                {
                    ymax = XZofVertices[ii1].y;
                }
            }
            float dx = xmax - xmin;
            float dy = ymax - ymin;
            float dmax = (dx > dy) ? dx : dy;
            float xmid = (xmax + xmin) * 0.5f;
            float ymid = (ymax + ymin) * 0.5f;
            Vector2[] ExpandedXZ = new Vector2[3 + VertexCount];
            for (int ii1 = 0; ii1 < VertexCount; ii1++)
            {
                ExpandedXZ[ii1] = XZofVertices[ii1];
            }
            ExpandedXZ[VertexCount] = new Vector2((xmid - 2 * dmax), (ymid - dmax));
            ExpandedXZ[VertexCount + 1] = new Vector2(xmid, (ymid + 2 * dmax));
            ExpandedXZ[VertexCount + 2] = new Vector2((xmid + 2 * dmax), (ymid - dmax));
            List<Triangle> TriangleList = new List<Triangle>();
            TriangleList.Add(new Triangle(VertexCount, VertexCount + 1, VertexCount + 2));
            for (int ii1 = 0; ii1 < VertexCount; ii1++)
            {
                List<Edge> Edges = new List<Edge>();
                for (int ii2 = 0; ii2 < TriangleList.Count; ii2++)
                {
                    if (TriangulatePolygonSubFunc_InCircle(ExpandedXZ[ii1], ExpandedXZ[TriangleList[ii2].p1], ExpandedXZ[TriangleList[ii2].p2], ExpandedXZ[TriangleList[ii2].p3]))
                    {
                        Edges.Add(new Edge(TriangleList[ii2].p1, TriangleList[ii2].p2));
                        Edges.Add(new Edge(TriangleList[ii2].p2, TriangleList[ii2].p3));
                        Edges.Add(new Edge(TriangleList[ii2].p3, TriangleList[ii2].p1));
                        TriangleList.RemoveAt(ii2);
                        ii2--;
                    }
                }
                if (ii1 >= VertexCount)
                {
                    continue;
                }
                for (int ii2 = Edges.Count - 2; ii2 >= 0; ii2--)
                {
                    for (int ii3 = Edges.Count - 1; ii3 >= ii2 + 1; ii3--)
                    {
                        if (Edges[ii2].Equals(Edges[ii3]))
                        {
                            Edges.RemoveAt(ii3);
                            Edges.RemoveAt(ii2);
                            ii3--;
                            continue;
                        }
                    }
                }
                for (int ii2 = 0; ii2 < Edges.Count; ii2++)
                {
                    TriangleList.Add(new Triangle(Edges[ii2].p1, Edges[ii2].p2, ii1));
                }
                Edges.Clear();
                Edges = null;
            }
            for (int ii1 = TriangleList.Count - 1; ii1 >= 0; ii1--)
            {
                if (TriangleList[ii1].p1 >= VertexCount || TriangleList[ii1].p2 >= VertexCount || TriangleList[ii1].p3 >= VertexCount)
                {
                    TriangleList.RemoveAt(ii1);
                }
            }
            TriangleList.TrimExcess();
            int[] Triangles = new int[3 * TriangleList.Count];
            for (int ii1 = 0; ii1 < TriangleList.Count; ii1++)
            {
                Triangles[3 * ii1] = TriangleList[ii1].p1;
                Triangles[3 * ii1 + 1] = TriangleList[ii1].p2;
                Triangles[3 * ii1 + 2] = TriangleList[ii1].p3;
            }
            return Triangles;
        }

        private static bool TriangulatePolygonSubFunc_InCircle(
                Vector2 p, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            if (Mathf.Abs(p1.y - p2.y) < float.Epsilon &&
                    Mathf.Abs(p2.y - p3.y) < float.Epsilon)
            { return false; }
            float m1, m2, mx1, mx2, my1, my2, xc, yc;
            if (Mathf.Abs(p2.y - p1.y) < float.Epsilon)
            {
                m2 = -(p3.x - p2.x) / (p3.y - p2.y);
                mx2 = (p2.x + p3.x) * 0.5f;
                my2 = (p2.y + p3.y) * 0.5f;
                xc = (p2.x + p1.x) * 0.5f;
                yc = m2 * (xc - mx2) + my2;
            }
            else if (Mathf.Abs(p3.y - p2.y) < float.Epsilon)
            {
                m1 = -(p2.x - p1.x) / (p2.y - p1.y);
                mx1 = (p1.x + p2.x) * 0.5f;
                my1 = (p1.y + p2.y) * 0.5f;
                xc = (p3.x + p2.x) * 0.5f;
                yc = m1 * (xc - mx1) + my1;
            }
            else
            {
                m1 = -(p2.x - p1.x) / (p2.y - p1.y);
                m2 = -(p3.x - p2.x) / (p3.y - p2.y);
                mx1 = (p1.x + p2.x) * 0.5f;
                mx2 = (p2.x + p3.x) * 0.5f;
                my1 = (p1.y + p2.y) * 0.5f;
                my2 = (p2.y + p3.y) * 0.5f;
                xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2);
                yc = m1 * (xc - mx1) + my1;
            }
            float dx = p2.x - xc;
            float dy = p2.y - yc;
            float rsqr = dx * dx + dy * dy;
            dx = p.x - xc;
            dy = p.y - yc;
            double drsqr = dx * dx + dy * dy;
            return (drsqr <= rsqr);
        }
    }
}
