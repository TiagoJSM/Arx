using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Anima2D
{
	[InitializeOnLoad]
	public class EditorEventHandler
	{
		static EditorEventHandler()
		{
			SceneView.onSceneGUIDelegate += OnSceneGUI;
			EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;
		}

		static SpriteMesh spriteMesh = null;
		static GameObject instance = null;
		static bool init = false;
		static Vector3 instancePosition = Vector3.zero;
		static Transform parentTransform = null;

		static SpriteMesh GetSpriteMesh()
		{
			SpriteMesh l_spriteMesh = null;

			if(DragAndDrop.objectReferences.Length > 0)
			{
				Object obj = DragAndDrop.objectReferences[0];

				l_spriteMesh = obj as SpriteMesh;

				if(!l_spriteMesh && DragAndDrop.paths.Length > 0)
				{
					l_spriteMesh = AssetDatabase.LoadAssetAtPath(DragAndDrop.paths[0],typeof(SpriteMesh)) as SpriteMesh;
				}
			}

			return l_spriteMesh;
		}

		static void Cleanup()
		{
			init = false;
			spriteMesh = null;
			instance = null;
		}

		static Vector3 GetMouseWorldPosition()
		{
			Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
			Plane rootPlane = new Plane(Vector3.forward,Vector3.zero);
			
			float distance = 0f;
			Vector3 mouseWorldPos = Vector3.zero;
			
			if(rootPlane.Raycast(mouseRay, out distance))
			{
				mouseWorldPos = mouseRay.GetPoint(distance);
			}

			return mouseWorldPos;
		}

		static GameObject CreateInstance(Transform parent)
		{
			GameObject l_instance = null;

			if(spriteMesh)
			{
				l_instance = new GameObject(spriteMesh.name);
				
				Mesh mesh = AssetDatabase.LoadAssetAtPath(DragAndDrop.paths[0],typeof(Mesh)) as Mesh;
				Material material = AssetDatabase.LoadAssetAtPath(DragAndDrop.paths[0],typeof(Material)) as Material;
				SpriteMeshInstance spriteMeshRenderer = l_instance.AddComponent<SpriteMeshInstance>();
				spriteMeshRenderer.spriteMesh = spriteMesh;
				MeshFilter meshFilter = l_instance.AddComponent<MeshFilter>();
				meshFilter.sharedMesh = mesh;
				MeshRenderer renderer = l_instance.AddComponent<MeshRenderer>();
				renderer.sharedMaterial = material;

				if(parent)
				{
					l_instance.transform.parent = parent;
					l_instance.transform.localPosition = Vector3.zero;
				}
			}

			return l_instance;
		}

		private static void HierarchyWindowItemCallback(int pID, Rect pRect)
		{
			instancePosition = Vector3.zero;
			GameObject parent = null;

			if(pRect.Contains(Event.current.mousePosition))
			{
				parent = EditorUtility.InstanceIDToObject(pID) as GameObject;

				if(parent)
				{
					parentTransform = parent.transform;
				}
			}

			HandleDragAndDrop(false,parentTransform);
		}

		static void OnSceneGUI(SceneView sceneview)
		{
			instancePosition = GetMouseWorldPosition();
			HandleDragAndDrop(true,null);
		}

		static void HandleDragAndDrop(bool createOnEnter, Transform parent)
		{
			switch(Event.current.type)
			{
			case EventType.DragUpdated:

				if(!init)
				{
					spriteMesh = GetSpriteMesh();

					if(createOnEnter)
					{
						 instance = CreateInstance(parent);
					}

					if(instance)
					{
						Event.current.Use();
					}

					init = true;
				}

				if(instance)
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
					instance.transform.position = instancePosition;
				}

				break;
			
			case EventType.DragExited:

				if(instance)
				{
					GameObject.DestroyImmediate(instance);
					Event.current.Use();
				}
				Cleanup();
				break;

			case EventType.DragPerform:

				if(!createOnEnter)
				{
					instance = CreateInstance(parent);
				}

				if(instance)
				{
					Event.current.Use();
				}
				Cleanup();
				break;
			}
		}
	}
}
