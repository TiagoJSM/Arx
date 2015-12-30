using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D 
{
	[Serializable]
	public class Vertex : ICloneable
	{
		public Vector2 vertex = Vector2.zero;
		public BoneWeight2 boneWeight = new BoneWeight2();

		public Vertex() {}

		public Vertex(Vertex other)
		{
			vertex = other.vertex;
			boneWeight.boneIndex0 = other.boneWeight.boneIndex0;
			boneWeight.boneIndex1 = other.boneWeight.boneIndex1;
			boneWeight.boneIndex2 = other.boneWeight.boneIndex2;
			boneWeight.boneIndex3 = other.boneWeight.boneIndex3;
			boneWeight.weight0 = other.boneWeight.weight0;
			boneWeight.weight1 = other.boneWeight.weight1;
			boneWeight.weight2 = other.boneWeight.weight2;
			boneWeight.weight3 = other.boneWeight.weight3;
		}

		public Vertex(Vector2 vertex)
		{
			this.vertex = vertex;
		}

		public object Clone()
		{
			Vertex clone = MemberwiseClone() as Vertex;
			clone.boneWeight = boneWeight.Clone() as BoneWeight2;
			return clone;
		}

		public override string ToString ()
		{
			return vertex.ToString();
		}
	}
}
