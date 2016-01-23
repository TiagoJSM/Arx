using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D 
{
	public static class BoneUtils
	{	
		public static string GetUniqueBoneName(Bone2D root)
		{
			string boneName = "bone";
			
			Bone2D[] bones = null;
			
			if(root)
			{
				bones = root.GetComponentsInChildren<Bone2D>(true);
				boneName = boneName + " " + (bones.Length + 1).ToString();
			}
			
			return boneName;
		}

		public static void DrawBoneCap(Bone2D bone)
		{
			DrawBoneCap(bone, bone.color);
		}

		public static void DrawBoneCap(Bone2D bone, Color color)
		{
			Handles.matrix = bone.transform.localToWorldMatrix;
			DrawBoneCap(Vector3.zero,GetBoneRadius(bone),color);
		}

		public static void DrawBoneCap(Bone2D bone, Color outer, Color inner)
		{
			Handles.matrix = bone.transform.localToWorldMatrix;
			DrawBoneCap(Vector3.zero,GetBoneRadius(bone),outer,inner);
		}
		
		public static void DrawBoneCap(Vector3 position, float radius, Color outer)
		{
			Color inner = outer * 0.25f;
			inner.a = 1f;
			DrawBoneCap(position,radius,outer,inner);
		}
		
		public static void DrawBoneCap(Vector3 position, float radius, Color outer, Color inner)
		{
			Handles.color = outer;
			HandlesExtra.DrawCircle(position, radius);
			Handles.color = inner;
			HandlesExtra.DrawCircle(position, radius*0.65f);
		}

		public static void DrawBoneBody(Bone2D bone)
		{
			DrawBoneBody(bone,bone.color);
		}

		public static void DrawBoneBody(Bone2D bone, Color color)
		{
			Handles.matrix = bone.transform.localToWorldMatrix;
			DrawBoneBody(Vector3.zero, bone.localEndPosition, GetBoneRadius(bone),color);
		}
		
		public static void DrawBoneBody(Vector3 position, Vector3 childPosition, float radius, Color color)
		{
			Vector3 distance = position - childPosition;
			
			if(distance.magnitude > radius)
			{
				HandlesExtra.DrawLine(position,childPosition,Vector3.back,2f*radius,0f,color);
			}
		}

		public static void DrawBoneOutline(Bone2D bone, float outlineSize, Color color)
		{
			Handles.matrix = bone.transform.localToWorldMatrix;
			DrawBoneOutline(Vector3.zero,bone.localEndPosition,GetBoneRadius(bone),outlineSize,color);
		}

		public static void DrawBoneOutline(Vector3 position, Vector3 endPoint, float radius, float outlineSize, Color color)
		{
			Handles.color = color;
			HandlesExtra.DrawLine(position,endPoint,Vector3.back, 2f * (radius + outlineSize), 2f * outlineSize);
			HandlesExtra.DrawCircle(position, radius + outlineSize);
			HandlesExtra.DrawCircle(position, outlineSize);
		}

		public static float GetBoneRadius(Bone2D bone)
		{
			return Mathf.Min(bone.localLength / 20f, 0.125f * HandleUtility.GetHandleSize(bone.transform.position));
		}

		public static string GetBonePath(Bone2D bone)
		{
			return GetBonePath(bone.root.transform,bone);
		}

		public static string GetBonePath(Transform root, Bone2D bone)
		{
			string path = "";

			Transform current = bone.transform;
			
			if(root)
			{
				while(current && current != root)
				{
					path = current.name + path;
					
					current = current.transform.parent;
					
					if(current != root)
					{
						path = "/" + path;
					}
					
				}
				
				if(!current)
				{
					path = "";
				}
			}
			
			return path;
		}
	}
}
