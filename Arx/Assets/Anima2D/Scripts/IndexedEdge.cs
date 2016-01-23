using UnityEngine;
using System;
using System.Collections;

namespace Anima2D 
{
	[Serializable]
	public struct IndexedEdge
	{
		public int index1;
		public int index2;

		public IndexedEdge(int index1, int index2)
		{
			this.index1 = index1;
			this.index2 = index2;
		}
	}
}
