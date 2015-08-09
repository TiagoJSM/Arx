using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.DataStructures
{
    public class Edge
    {
        public int p1;
        public int p2;
        public Edge(int point1, int point2)
        {
            p1 = point1;
            p2 = point2;
        }
        public Edge() : this(0, 0) { }
        public bool Equals(Edge other)
        {
            return ((this.p1 == other.p2) && (this.p2 == other.p1)) || ((this.p1 == other.p1) && (this.p2 == other.p2));
        }
    }
}
