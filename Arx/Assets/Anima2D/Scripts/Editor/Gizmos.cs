using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anima2D 
{
	[InitializeOnLoad]
	public class Gizmos
	{
		static List<Bone2D> s_Bones = new List<Bone2D>();

		static List<Bone2D> visibleBones { 
			get {
				return s_Bones.Where( b => b && IsVisible(b)).ToList();
			}
		}

		static List<Bone2D> unlockedVisibleBones { 
			get {
				return s_Bones.Where( b => b && IsVisible(b) && !IsLocked(b)).ToList();
			}
		}

		static Gizmos()
		{
			SceneView.onSceneGUIDelegate += OnSceneGUI;
			EditorApplication.hierarchyWindowChanged += HierarchyWindowChanged;
		}

		static bool IsVisible(Bone2D bone)
		{
			return (Tools.visibleLayers & 1 << bone.gameObject.layer) != 0;
		}

		static bool IsLocked(Bone2D bone)
		{
			return (Tools.lockedLayers & 1 << bone.gameObject.layer) != 0;
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		static void HierarchyWindowChanged()
		{
			s_Bones = new List<Bone2D>(GameObject.FindObjectsOfType<Bone2D>());

			UpdateChildBoneProperty();

			SceneView.RepaintAll();
		}

		static void DrawBoneGizmos()
		{
			Color blue = new Color (0f, 0.75f, 0.75f, 1f);

			List<Bone2D> l_bones = visibleBones;

			if(s_HoveredBone)
			{
				float outlineSize = HandleUtility.GetHandleSize(s_HoveredBone.transform.position) * 0.015f * s_HoveredBone.color.a;

				BoneUtils.DrawBoneOutline(s_HoveredBone,outlineSize,Color.yellow);
			}

			for (int j = 0; j < l_bones.Count; j++)
			{
				Bone2D bone = l_bones[j];

				if(bone && bone.color.a == 0f)
				{
					continue;	
				}

				if (bone &&
				    bone.gameObject.activeInHierarchy &&
				    bone.parentBone &&
				    bone.parentBone.child != bone)
				{
					Handles.matrix = Matrix4x4.identity;
					
					BoneUtils.DrawBoneBody(bone.transform.position,
					                       bone.parentBone.transform.position,
					                       BoneUtils.GetBoneRadius(bone),
					                       bone.color * new Color(1f,1f,1f,0.5f));
				}
			}
			
			for (int j = 0; j < l_bones.Count; j++)
			{
				Bone2D bone = l_bones[j];

				if(bone && bone.color.a == 0f)
				{
					continue;	
				}

				if(bone && bone.gameObject.activeInHierarchy)
				{	
					BoneUtils.DrawBoneBody(bone); 
				}
			}
			
			for (int j = 0; j < l_bones.Count; j++)
			{
				Bone2D bone = l_bones[j];

				if(bone && bone.color.a == 0f)
				{
					continue;	
				}

				if (bone && bone.gameObject.activeInHierarchy)
				{
					Color innerColor = bone.color * 0.25f;

					if(bone.attachedIK &&
						bone.attachedIK.isActiveAndEnabled)
					{
						innerColor = blue;
					}

					innerColor.a = bone.color.a;

					BoneUtils.DrawBoneCap(bone,bone.color,innerColor);
				}
			}
		}

		static Bone2D GetClosestBone(Vector2 mousePosition, float minDistance)
		{
			Bone2D closestBone = null;
			float minSqrDistance = float.MaxValue;

			List<Bone2D> l_bones = unlockedVisibleBones;

			for (int i = 0; i < l_bones.Count; i++)
			{
				Bone2D bone = l_bones[i];

				if(bone)
				{
					float radius = BoneUtils.GetBoneRadius(bone);
					Vector3 direction = (bone.endPosition - bone.transform.position).normalized;
					Vector2 screenPosition = HandleUtility.WorldToGUIPoint(bone.transform.position);
					Vector3 endPoint = bone.endPosition - direction * radius;
					
					if(bone.child)
					{
						endPoint -= direction * BoneUtils.GetBoneRadius(bone.child);
					}
					
					Vector2 screenEndPosition = HandleUtility.WorldToGUIPoint(endPoint);

					float sqrDistance = MathUtils.SegmentSqrtDistance(mousePosition,screenPosition,screenEndPosition);

					if(sqrDistance < minSqrDistance && sqrDistance < minDistance * minDistance)
					{
						minSqrDistance = sqrDistance;
						closestBone = bone;
					}
				}
			}
			
			return closestBone;
		}
		
		static Bone2D s_HoveredBone = null;
		static Bone2D s_SelectedBone = null;
		
		static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
		{
			dirA = Vector3.ProjectOnPlane(dirA, axis);
			dirB = Vector3.ProjectOnPlane(dirB, axis);
			float num = Vector3.Angle(dirA, dirB);
			return num * (float)((Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) >= 0f) ? 1 : -1);
		}
		
		static void UpdateChildBoneProperty()
		{
			for (int i = 0; i < s_Bones.Count; i++)
			{
				Bone2D bone = s_Bones [i];
				
				if(bone)
				{
					SerializedObject boneSO = new SerializedObject(bone);
					SerializedProperty childProp = boneSO.FindProperty("mChild");
					
					boneSO.Update();
					if(childProp.objectReferenceValue && (childProp.objectReferenceValue == bone || (childProp.objectReferenceValue as Bone2D).transform.parent != bone.transform))
					{
						childProp.objectReferenceValue = null;
					}
					boneSO.ApplyModifiedProperties();
				}
			}
		}
		
		public static void OnSceneGUI(SceneView sceneview)
		{
			int dragBoneControlId = GUIUtility.GetControlID(FocusType.Passive);

			switch(Event.current.GetTypeForControl(dragBoneControlId))
			{
			case EventType.MouseMove:

				Bone2D bone = GetClosestBone(Event.current.mousePosition, 10f);

				if(bone != s_HoveredBone)
				{
					s_HoveredBone = bone;
					SceneView.RepaintAll();
				}

				break;
				
			case EventType.MouseDown:

				if(!ToolsExtra.viewToolActive &&
				   HandleUtility.nearestControl == dragBoneControlId && 
				   Event.current.button == 0)
				{
					s_SelectedBone = s_HoveredBone;
					Undo.IncrementCurrentGroup();

					GUIUtility.hotControl = dragBoneControlId;
					Event.current.Use();
				}
				
				break;
				
			case EventType.MouseUp:
				
				if(s_SelectedBone &&
				   GUIUtility.hotControl == dragBoneControlId &&
				   Event.current.button == 0)
				{
					Selection.activeGameObject = s_SelectedBone.gameObject;

					GUIUtility.hotControl = 0;
					s_SelectedBone = null;
					
					Event.current.Use();
				}
				
				break;
				
			case EventType.MouseDrag:
				
				if(s_SelectedBone &&
				   GUIUtility.hotControl == dragBoneControlId &&
				   Event.current.button == 0)
				{
					Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
					Plane plane = new Plane(s_SelectedBone.transform.forward,s_SelectedBone.transform.position);
					float distance;
					if(plane.Raycast(ray,out distance))
					{
						Vector3 point = ray.GetPoint(distance);
						float angle = AngleAroundAxis(s_SelectedBone.transform.right,point - s_SelectedBone.transform.position,s_SelectedBone.transform.forward);
						
						Vector3 eulerAngles = s_SelectedBone.transform.localEulerAngles;
						eulerAngles.z += angle;
						
						Undo.RecordObject(s_SelectedBone.transform, "Rotate");
						s_SelectedBone.transform.localRotation = Quaternion.Euler(eulerAngles);
						
						IkUtils.UpdateIK(s_SelectedBone,"Rotate");
						
					}
					
					GUI.changed = true;
					Event.current.Use();
				}
				
				break;
				
			case EventType.Repaint:
				
				DrawBoneGizmos();
				
				break;
				
			case EventType.Layout:
				
				if(!ToolsExtra.viewToolActive && s_HoveredBone)
				{
					float radius = BoneUtils.GetBoneRadius(s_HoveredBone);
					Vector3 direction = (s_HoveredBone.endPosition - s_HoveredBone.transform.position).normalized;
					Vector2 screenPosition = HandleUtility.WorldToGUIPoint(s_HoveredBone.transform.position);

					Vector3 endPoint = s_HoveredBone.endPosition - direction * radius;

					if(s_HoveredBone.child)
					{
						endPoint -= direction * BoneUtils.GetBoneRadius(s_HoveredBone.child);
					}

					Vector2 screenEndPosition = HandleUtility.WorldToGUIPoint(endPoint);

					float distance = MathUtils.SegmentDistance(Event.current.mousePosition,screenPosition,screenEndPosition);

					HandleUtility.AddControl(dragBoneControlId, distance * 0.49f);
				}
				
				break;
			}
		}
	}
}
