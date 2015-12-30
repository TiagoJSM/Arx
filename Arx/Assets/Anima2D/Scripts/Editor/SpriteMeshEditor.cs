using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D 
{
	[CustomEditor(typeof(SpriteMesh))]
	public class SpriteMeshEditor : Editor
	{
		override public void OnInspectorGUI()
		{
			DrawDefaultInspector();

			SpriteMesh spriteMesh = target as SpriteMesh;

			EditorGUILayout.BeginHorizontal();
			
			GUILayout.FlexibleSpace();
			
			if (GUILayout.Button("Edit Sprite Mesh",GUILayout.Width(150f)))
			{
				SpriteMeshEditorWindow.Initialize(spriteMesh);
			}
			
			GUILayout.FlexibleSpace();
			
			EditorGUILayout.EndHorizontal();
		}
	}
}
