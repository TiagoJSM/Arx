using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D 
{
	public class SpriteMesh : ScriptableObject
	{
		public Sprite sprite;

		public List<Vertex> texVertices = new List<Vertex>();
		public List<IndexedEdge> edges = new List<IndexedEdge>();
		public List<Hole> holes = new List<Hole>();
		public List<int> indices = new List<int>();
		public List<BindInfo> bindPoses = new List<BindInfo>();
	}
}
