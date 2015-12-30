using UnityEngine;
using System;
using System.Collections;

namespace Anima2D 
{
	[Serializable]
	public class Hole
	{
		public Vector2 vertex = Vector2.zero;

		public Hole(Vector2 vertex)
		{
			this.vertex = vertex;
		}
	}
}
