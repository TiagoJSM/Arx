using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Anima2D 
{
	[InitializeOnLoad]
	public class EditorUpdater
	{
		static bool s_DraggingATool = false;
		static List<Ik2D> s_Ik2Ds = new List<Ik2D>();

		static EditorUpdater()
		{
			SceneView.onSceneGUIDelegate += OnSceneGUI;
			EditorApplication.update += Update;
			EditorApplication.hierarchyWindowChanged += HierarchyWindowChanged;

			Undo.undoRedoPerformed += UndoRedoPerformed;
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		static void HierarchyWindowChanged()
		{
			s_Ik2Ds = new List<Ik2D>(GameObject.FindObjectsOfType<Ik2D>());
		}

		static void UndoRedoPerformed()
		{
			ForceDeserialize();

			EditorApplication.delayCall += () => { SceneView.RepaintAll(); };
		}

		static void ForceDeserialize()
		{
			for (int i = 0; i < s_Ik2Ds.Count; i++)
			{
				Ik2D ik2D = s_Ik2Ds [i];

				if(ik2D)
				{
					ForceDeserialize(ik2D);
				}
			}

			List<SpriteMeshInstance> spriteMeshInstances = new List<SpriteMeshInstance>(GameObject.FindObjectsOfType<SpriteMeshInstance>());

			for (int i = 0; i < spriteMeshInstances.Count; i++)
			{
				SpriteMeshInstance spriteMeshInstance = spriteMeshInstances [i];

				if(spriteMeshInstance)
				{
					ForceDeserialize(spriteMeshInstance);
				}
			}

			UpdateAttachedIKs();
		}

		static void ForceDeserialize(MonoBehaviour monoBehaviour)
		{
			SerializedObject serializedObject = new SerializedObject(monoBehaviour);

			UnityEngine.Object obj = null;
			
			SerializedProperty iterator = serializedObject.GetIterator();

			//Discard m_Script property, throws warnings in PlayMode
			iterator.NextVisible(true);

			//Look for any ObjectRef property
			while(iterator.NextVisible(true) && iterator.propertyType != SerializedPropertyType.ObjectReference) {}

			//Force refresh
			if(iterator.propertyType == SerializedPropertyType.ObjectReference)
			{
				obj = iterator.objectReferenceValue;
				
				serializedObject.Update();
				iterator.objectReferenceValue = null;
				serializedObject.ApplyModifiedProperties();
				
				serializedObject.Update();
				iterator.objectReferenceValue = obj;
				serializedObject.ApplyModifiedProperties();
			}
		}
		
		static void OnSceneGUI(SceneView sceneview)
		{
			if(!s_DraggingATool &&
			   GUIUtility.hotControl != 0 &&
			   !ToolsExtra.viewToolActive)
			{
				s_DraggingATool = Event.current.type == EventType.MouseDrag;
			}
		}

		static void UpdateAttachedIKs()
		{
			for (int i = 0; i < s_Ik2Ds.Count; i++)
			{
				Ik2D ik2D = s_Ik2Ds[i];
				
				if(ik2D)
				{
					for (int j = 0; j < ik2D.solver.solverPoses.Count; j++)
					{
						IkSolver2D.SolverPose pose = ik2D.solver.solverPoses[j];
						
						if(pose.bone)
						{
							pose.bone.attachedIK = ik2D;
						}
					}
				}
			}
		}

		static void Update()
		{
			UpdateAttachedIKs();

			if(s_DraggingATool)
			{
				string undoName = "Move";

				if(Tools.current == Tool.Rotate) undoName = "Rotate";
				if(Tools.current == Tool.Scale) undoName = "Scale";

				for (int i = 0; i < Selection.transforms.Length; i++)
				{
					Transform transform = Selection.transforms [i];
					Ik2D ik2D = transform.GetComponent<Ik2D> ();
					if (ik2D)
					{
						IkUtils.UpdateIK(ik2D, undoName);
					}

					Bone2D bone = transform.GetComponent<Bone2D>();
					if(bone)
					{
						IkUtils.UpdateIK(bone, undoName);
					}
				}

				s_DraggingATool = false;
			}
		}
	}
}
