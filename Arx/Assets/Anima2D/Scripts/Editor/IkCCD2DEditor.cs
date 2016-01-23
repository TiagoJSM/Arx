using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D 
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(IkCCD2D))]
	public class IkCCD2DEditor : Ik2DEditor
	{
		override public void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			SerializedProperty targetBoneProp = serializedObject.FindProperty("m_Target");
			SerializedProperty numBonesProp = serializedObject.FindProperty("m_NumBones");
			SerializedProperty iterationsProp = serializedObject.FindProperty("iterations");
			SerializedProperty dampingProp = serializedObject.FindProperty("damping");

			Bone2D targetBone = targetBoneProp.objectReferenceValue as Bone2D;

			EditorGUI.BeginDisabledGroup(!targetBone);

			EditorGUI.BeginChangeCheck();

			int chainLength = 0;

			if(targetBone)
			{
				chainLength = targetBone.chainLength;
			}

			EditorGUILayout.IntSlider(numBonesProp,0,chainLength);
			
			if(EditorGUI.EndChangeCheck())
			{
				IkUtils.InitializeIk2D(serializedObject);

				DoUpdateIK();
			}

			EditorGUI.EndDisabledGroup();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(iterationsProp);
			EditorGUILayout.PropertyField(dampingProp);

			if(EditorGUI.EndChangeCheck())
			{
				DoUpdateIK();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}	
}
