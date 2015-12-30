using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D 
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Ik2D),true)]
	public class Ik2DEditor : Editor
	{
		override public void OnInspectorGUI()
		{
			serializedObject.Update();

			SerializedProperty targetProp = serializedObject.FindProperty("m_Target");
			SerializedProperty weightProp = serializedObject.FindProperty("m_Weight");

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(targetProp);

			if(EditorGUI.EndChangeCheck())
			{
				IkUtils.InitializeIk2D(serializedObject);

				DoUpdateIK();
			}

			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.Slider(weightProp,0f,1f);
			
			if(EditorGUI.EndChangeCheck())
			{
				DoUpdateIK();
			}

			serializedObject.ApplyModifiedProperties();
		}

		protected void DoUpdateIK()
		{
			Ik2D ik2D = target as Ik2D;

			IkUtils.UpdateIK(ik2D,"Update IK");
		}
	}	
}
