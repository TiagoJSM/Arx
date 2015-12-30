using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D 
{
	[Serializable]
	public class Edge : System.Object
	{
		public Vertex vertex1;
		public Vertex vertex2;

		public Edge() { }
		
		public Edge(Vertex vertex1, Vertex vertex2)
		{
			this.vertex1 = vertex1;
			this.vertex2 = vertex2;
		}

		public bool IsInEdge(Vector3 point, float distance)
		{
			if(IsInEdge(point) && MathUtils.SqrtLineDistance(point,vertex1.vertex,vertex2.vertex) <= distance * distance)
			{
				return true;
			}
			
			return false;
		}
		
		public bool IsInEdge(Vector2 point)
		{
			Vector2 v = vertex2.vertex - vertex1.vertex;
			Vector2 p = point - vertex1.vertex;
			
			float dot = Vector2.Dot(v,p);
			
			if(dot < 0f)
			{
				return false;
			}
			
			if(dot > v.sqrMagnitude)
			{
				return false;
			}
			
			return true;
		}

		public override bool Equals(System.Object obj) 
		{
			if (obj == null || GetType() != obj.GetType()) 
				return false;
			
			Edge p = (Edge)obj;

			bool value = (vertex1 == p.vertex1) && (vertex2 == p.vertex2);

			if(!value)
			{
				value = (vertex1 == p.vertex2) && (vertex2 == p.vertex1);
			}

			return value;
		}

		public override int GetHashCode() 
		{
			return vertex1.GetHashCode() ^ vertex2.GetHashCode();
		}
	}
}