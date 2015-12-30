using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Anima2D 
{
	public class HandlesExtra
	{
		static Material s_HandleWireMaterial;
		static Material s_HandleWireMaterial2D;
		
		static Material handleWireMaterial
		{
			get
			{
				if (!s_HandleWireMaterial)
				{
					s_HandleWireMaterial = (Material)EditorGUIUtility.LoadRequired("SceneView/HandleLines.mat");
					s_HandleWireMaterial2D = (Material)EditorGUIUtility.LoadRequired("SceneView/2DHandleLines.mat");
				}
				return (!Camera.current) ? s_HandleWireMaterial2D : s_HandleWireMaterial;
			}
		}

		static Vector3[] s_circleArray;

		static void SetDiscSectionPoints(Vector3[] dest, int count, Vector3 normal, Vector3 from, float angle)
		{
			from.Normalize();
			Quaternion rotation = Quaternion.AngleAxis(angle / (float)(count - 1), normal);
			Vector3 vector = from;
			for (int i = 0; i < count; i++)
			{
				dest[i] = vector;
				vector = rotation * vector;
			}
		}

		public static void DrawCircle(Vector3 center, float radius)
		{
			DrawCircle(center,radius,0f);
		}

		public static void DrawCircle(Vector3 center, float radius, float innerRadius)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			
			innerRadius = Mathf.Clamp01(innerRadius);
			
			if(s_circleArray == null)
			{
				s_circleArray = new Vector3[12];
				SetDiscSectionPoints(s_circleArray, 12, Vector3.forward, Vector3.right, 360f);
			}

			Shader.SetGlobalColor("_HandleColor", Handles.color * new Color(1f, 1f, 1f, 0.5f));
			Shader.SetGlobalFloat("_HandleSize", 1f);
			handleWireMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(Handles.matrix);
			GL.Begin(4);
			for (int i = 1; i < s_circleArray.Length; i++)
			{
				GL.Color(Handles.color);
				GL.Vertex(center + s_circleArray[i - 1]* radius * innerRadius);
				GL.Vertex(center + s_circleArray[i - 1]*radius);
				GL.Vertex(center + s_circleArray[i]*radius);
				GL.Vertex(center + s_circleArray[i - 1]* radius * innerRadius);
				GL.Vertex(center + s_circleArray[i]*radius);
				GL.Vertex(center + s_circleArray[i]* radius * innerRadius);
			}
			GL.End();
			GL.PopMatrix();
		}

		public static void DrawTriangle(Vector3 center, Vector3 normal, float radius, float innerRadius)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			
			innerRadius = Mathf.Clamp01(innerRadius);
			
			Vector3[] array = new Vector3[60];
			SetDiscSectionPoints(array, 4, normal, Vector3.up, 360f);
			Shader.SetGlobalColor("_HandleColor", Handles.color * new Color(1f, 1f, 1f, 0.5f));
			Shader.SetGlobalFloat("_HandleSize", 1f);
			handleWireMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(Handles.matrix);
			GL.Begin(4);
			for (int i = 1; i < array.Length; i++)
			{
				GL.Color(Handles.color);
				GL.Vertex(center + array[i - 1]*innerRadius);
				GL.Vertex(center + array[i - 1]*radius);
				GL.Vertex(center + array[i]*radius);
				GL.Vertex(center + array[i - 1]*innerRadius);
				GL.Vertex(center + array[i]*radius);
				GL.Vertex(center + array[i]*innerRadius);
			}
			GL.End();
			GL.PopMatrix();
		}

		public static void DrawSquare(Vector3 center, Vector3 normal, float radius, float innerRadius)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			
			innerRadius = Mathf.Clamp01(innerRadius);
			
			Vector3[] array = new Vector3[60];
			SetDiscSectionPoints(array, 5, normal, Vector3.left + Vector3.up, 360f);
			Shader.SetGlobalColor("_HandleColor", Handles.color * new Color(1f, 1f, 1f, 0.5f));
			Shader.SetGlobalFloat("_HandleSize", 1f);
			handleWireMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(Handles.matrix);
			GL.Begin(4);
			for (int i = 1; i < array.Length; i++)
			{
				GL.Color(Handles.color);
				GL.Vertex(center + array[i - 1]*innerRadius);
				GL.Vertex(center + array[i - 1]*radius);
				GL.Vertex(center + array[i]*radius);
				GL.Vertex(center + array[i - 1]*innerRadius);
				GL.Vertex(center + array[i]*radius);
				GL.Vertex(center + array[i]*innerRadius);
			}
			GL.End();
			GL.PopMatrix();
		}

		public static void DrawLine (Vector3 p1, Vector3 p2, Vector3 normal, float width)
		{
			DrawLine(p1,p2,normal,width,width,Handles.color);
		}

		public static void DrawLine (Vector3 p1, Vector3 p2, Vector3 normal, float widthP1, float widthP2)
		{
			DrawLine(p1,p2,normal,widthP1,widthP2,Handles.color);
		}

		public static void DrawLine (Vector3 p1, Vector3 p2, Vector3 normal, float widthP1, float widthP2, Color color)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			Vector3 right = Vector3.Cross(normal,p2-p1).normalized;
			handleWireMaterial.SetPass(0);
			GL.PushMatrix ();
			GL.MultMatrix (Handles.matrix);
			GL.Begin (4);
			GL.Color (color);
			GL.Vertex (p1 + right * widthP1 * 0.5f);
			GL.Vertex (p1 - right * widthP1 * 0.5f);
			GL.Vertex (p2 - right * widthP2 * 0.5f);
			GL.Vertex (p1 + right * widthP1 * 0.5f);
			GL.Vertex (p2 - right * widthP2 * 0.5f);
			GL.Vertex (p2 + right * widthP2 * 0.5f);
			GL.End ();
			GL.PopMatrix ();
		}
	}
}
