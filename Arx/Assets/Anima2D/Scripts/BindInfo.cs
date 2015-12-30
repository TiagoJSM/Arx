using UnityEngine;
using System;

namespace Anima2D 
{
	[Serializable]
	public class BindInfo : ICloneable
	{
		public Matrix4x4 bindPose;
		public float boneLength;

		public Vector3 position { get { return bindPose.inverse * new Vector4 (0f, 0f, 0f, 1f); } }
		public Vector3 endPoint { get { return bindPose.inverse * new Vector4 (boneLength, 0f, 0f, 1f); } }

		public string path;
		public string name;

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}
