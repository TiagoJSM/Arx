using UnityEngine;
using System.Collections;

namespace Anima2D 
{
	public static class MathUtils
	{
		public static float SignedAngle(Vector3 a, Vector3 b, Vector3 forward)
		{
			float angle = Vector3.Angle (a, b);
			float sign = Mathf.Sign (Vector3.Dot (forward, Vector3.Cross (a, b)));
			
			return angle * sign;
		}

		public static float Fmod(float value, float mod)
		{
			return Mathf.Abs(value % mod + mod) % mod;
		}

		public static float SegmentDistance(Vector3 point, Vector3 a, Vector3 b)
		{
			Vector2 v = b - a;
			Vector2 p = point - a;
			
			float dot = Vector2.Dot(v,p);
			
			if(dot <= 0f)
			{
				return p.magnitude;
			}
			
			if(dot >= v.sqrMagnitude)
			{
				return (point - b).magnitude;
			}
			
			return LineDistance(point,a,b);
		}

		public static float SegmentSqrtDistance(Vector3 point, Vector3 a, Vector3 b)
		{
			Vector2 v = b - a;
			Vector2 p = point - a;
			
			float dot = Vector2.Dot(v,p);
			
			if(dot <= 0f)
			{
				return p.sqrMagnitude;
			}
			
			if(dot >= v.sqrMagnitude)
			{
				return (point - b).sqrMagnitude;
			}
			
			return SqrtLineDistance(point,a,b);
		}
		
		public static float LineDistance(Vector3 point, Vector3 a, Vector3 b)
		{
			return Mathf.Sqrt(SqrtLineDistance(point,a,b));
		}
		
		public static float SqrtLineDistance(Vector3 point, Vector3 a, Vector3 b)
		{
			float num = Mathf.Abs((b.y-a.y)*point.x - (b.x-a.x)*point.y + b.x*a.y - b.y*a.x);
			return num*num / ((b.y - a.y)*(b.y - a.y) + (b.x-a.x)*(b.x-a.x));
		}

		public static void WorldFromMatrix4x4(this Transform transform, Matrix4x4 matrix)
		{
			transform.localScale = matrix.GetScale();
			transform.rotation = matrix.GetRotation();
			transform.position = matrix.GetPosition();
		}

		public static void LocalFromMatrix4x4(this Transform transform, Matrix4x4 matrix)
		{
			transform.localScale = matrix.GetScale();
			transform.localRotation = matrix.GetRotation();
			transform.localPosition = matrix.GetPosition();
		}
		
		public static Quaternion GetRotation(this Matrix4x4 matrix)
		{
			var qw = Mathf.Sqrt(1f + matrix.m00 + matrix.m11 + matrix.m22) / 2;
			var w = 4 * qw;
			var qx = (matrix.m21 - matrix.m12) / w;
			var qy = (matrix.m02 - matrix.m20) / w;
			var qz = (matrix.m10 - matrix.m01) / w;
			
			return new Quaternion(qx, qy, qz, qw);
		}
		
		public static Vector3 GetPosition(this Matrix4x4 matrix)
		{
			var x = matrix.m03;
			var y = matrix.m13;
			var z = matrix.m23;
			
			return new Vector3(x, y, z);
		}
		
		public static Vector3 GetScale(this Matrix4x4 m)
		{
			var x = Mathf.Sqrt(m.m00 * m.m00 + m.m01 * m.m01 + m.m02 * m.m02);
			var y = Mathf.Sqrt(m.m10 * m.m10 + m.m11 * m.m11 + m.m12 * m.m12);
			var z = Mathf.Sqrt(m.m20 * m.m20 + m.m21 * m.m21 + m.m22 * m.m22);
			
			return new Vector3(x, y, z);
		}
	}
}
