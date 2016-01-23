using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Bone2D))]
	public class Bone2DEditor : Editor
	{
		void OnEnable()
		{
			Tools.hidden = Tools.current == Tool.Move;
		}

		void OnDisable()
		{
			Tools.hidden = false;
		}

		override public void OnInspectorGUI()
		{
			DrawDefaultInspector();

			Bone2D bone = target as Bone2D;

			EditorGUI.BeginChangeCheck();

			Bone2D child = EditorGUILayout.ObjectField("Child", bone.child, typeof(Bone2D), true) as Bone2D;

			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(bone,"Child");
				bone.child = child;
				EditorUtility.SetDirty(bone);
			}

			EditorGUI.BeginChangeCheck();

			float length = EditorGUILayout.FloatField("Length", bone.localLength);

			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(bone,"Length");
				bone.localLength = length;
				EditorUtility.SetDirty(bone);
			}
		}

		void OnSceneGUI()
		{
			Bone2D bone = target as Bone2D;

			if(Tools.current == Tool.Move)
			{
				Tools.hidden = true;

				float size = HandleUtility.GetHandleSize(bone.transform.position) / 5f;

				Quaternion rotation = bone.transform.rotation;

				EditorGUI.BeginChangeCheck();

				Vector3 newPosition = Handles.FreeMoveHandle(bone.transform.position,
				                                             rotation,
				                                             size,
				                                             Vector3.zero,
				                                             Handles.RectangleCap);

				if(EditorGUI.EndChangeCheck())
				{
					GUI.changed = true;

					Bone2D linkedParentBone = bone.linkedParentBone;

					if(linkedParentBone)
					{
						Vector3 newLocalPosition = linkedParentBone.transform.InverseTransformPoint(newPosition);

						if(newLocalPosition.sqrMagnitude > 0f)
						{
							float angle = Mathf.Atan2(newLocalPosition.y,newLocalPosition.x) * Mathf.Rad2Deg;

							Undo.RecordObject(linkedParentBone.transform,"Move");
							Undo.RecordObject(linkedParentBone,"Move");

							linkedParentBone.transform.localRotation *= Quaternion.AngleAxis(angle, Vector3.forward);

							EditorUtility.SetDirty(linkedParentBone.transform);
						}
					}

					Undo.RecordObject(bone.transform,"Move");
					bone.transform.position = newPosition;
					bone.transform.rotation = rotation;
					EditorUtility.SetDirty(bone.transform);

					IkUtils.UpdateIK(bone,"Move");
				}
			}else{
				Tools.hidden = false;
			}
		}
	}
}
