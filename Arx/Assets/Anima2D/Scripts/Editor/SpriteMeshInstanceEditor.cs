using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D
{
	[CustomEditor(typeof(SpriteMeshInstance))]
	public class SpriteMeshInstanceEditor : Editor
	{
		SpriteMeshInstance mSpriteMeshInstance;

		ReorderableList mBoneList = null;

		void OnEnable()
		{
			mSpriteMeshInstance = target as SpriteMeshInstance;

			SetupList();
		}

		void SetupList()
		{
			SerializedProperty bonesProperty = serializedObject.FindProperty("bones");

			if(bonesProperty != null)
			{
				mBoneList = new ReorderableList(serializedObject,bonesProperty,true,true,true,true);

				mBoneList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

					SerializedProperty boneProperty = mBoneList.serializedProperty.GetArrayElementAtIndex(index);

					rect.y += 1.5f;

					EditorGUI.PropertyField( new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), boneProperty, GUIContent.none);
				};

				mBoneList.drawHeaderCallback = (Rect rect) => {  
					EditorGUI.LabelField(rect, "Bones");
				};

				mBoneList.onSelectCallback = (ReorderableList list) => {};
			}
		}

		public override void OnInspectorGUI()
		{
			if(DrawDefaultInspector())
			{
				mSpriteMeshInstance.UpdateRenderer();

				if(mSpriteMeshInstance.cachedRenderer)
				{
					EditorUtility.SetDirty(mSpriteMeshInstance.cachedRenderer);
				}

				if(mSpriteMeshInstance.cachedSkinnedRenderer)
				{
					EditorUtility.SetDirty(mSpriteMeshInstance.cachedSkinnedRenderer);
				}
			}

			serializedObject.Update();

			Transform root = EditorGUILayout.ObjectField("Set bones",null,typeof(Transform),true) as Transform;

			Bone2D childBone = null;

			if(root)
			{
				childBone = root.GetComponent<Bone2D>();
			}

			if(root && !childBone)
			{
				childBone = root.GetComponentInChildren<Bone2D>();
				childBone = childBone.root;
			}

			if(childBone)
			{
				Undo.RegisterCompleteObjectUndo(mSpriteMeshInstance,"set bones");

				childBone.GetComponentsInChildren<Bone2D>(true,mSpriteMeshInstance.bones);

				EditorUtility.SetDirty(mSpriteMeshInstance);
			}

			if(mBoneList != null)
			{
				mBoneList.DoLayoutList();
			}
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			EditorGUI.BeginDisabledGroup(HasNullBones() ||
			                             mSpriteMeshInstance.bones.Count == 0 ||
			                             mSpriteMeshInstance.spriteMesh.bindPoses.Count != mSpriteMeshInstance.bones.Count);
			
			if(GUILayout.Button("Set skinned renderer",GUILayout.MaxWidth(150f)))
			{
				EditorApplication.delayCall += DoSetSkinnedRenderer;
			}
			
			EditorGUI.EndDisabledGroup();
			
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			
			if(GUILayout.Button("Set mesh renderer",GUILayout.MaxWidth(150f)))
			{
				EditorApplication.delayCall += DoSetMeshRenderer;
			}
			
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			if(HasNullBones())
			{
				EditorGUILayout.HelpBox("Warning:\nBone list contains null references.", MessageType.Warning);
			}
			
			if(mSpriteMeshInstance.spriteMesh.bindPoses.Count != mSpriteMeshInstance.bones.Count)
			{
				EditorGUILayout.HelpBox("Warning:\nNumber of SpriteMesh Bind Poses and number of Bones does not match.", MessageType.Warning);
			}
			
			serializedObject.ApplyModifiedProperties();
		}

		bool HasNullBones()
		{
			return mSpriteMeshInstance.bones.Contains(null);
		}

		void DoSetSkinnedRenderer()
		{
			SpriteMesh spriteMesh = mSpriteMeshInstance.spriteMesh;

			if(spriteMesh)
			{
				MeshFilter meshFilter = mSpriteMeshInstance.GetComponent<MeshFilter>();
				MeshRenderer meshRenderer = mSpriteMeshInstance.GetComponent<MeshRenderer>();
				Material material = null;
				if(meshFilter)
				{
					DestroyImmediate(meshFilter);
				}
				if(meshRenderer)
				{
					material = meshRenderer.sharedMaterial;
					DestroyImmediate(meshRenderer);
				}

				string path = AssetDatabase.GetAssetPath(spriteMesh);

				if(!material)
				{
					material = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;
				}

				Mesh mesh = AssetDatabase.LoadAssetAtPath(path, typeof(Mesh)) as Mesh;
				
				SkinnedMeshRenderer skinnedMeshRenderer = mSpriteMeshInstance.GetComponent<SkinnedMeshRenderer>();

				if(!skinnedMeshRenderer)
				{
					skinnedMeshRenderer = mSpriteMeshInstance.gameObject.AddComponent<SkinnedMeshRenderer>();
				}

				if(skinnedMeshRenderer)
				{
					skinnedMeshRenderer.sharedMesh = mesh;
					skinnedMeshRenderer.sharedMaterial = material;

					skinnedMeshRenderer.bones = mSpriteMeshInstance.bones.ConvertAll( bone => bone.transform ).ToArray();

					if(mSpriteMeshInstance.bones.Count > 0)
					{
						skinnedMeshRenderer.rootBone = mSpriteMeshInstance.bones[0].transform;
					}
				}
			}

			mSpriteMeshInstance.UpdateRenderer();
		}

		void DoSetMeshRenderer()
		{
			SpriteMesh spriteMesh = mSpriteMeshInstance.spriteMesh;
			
			if(spriteMesh)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = mSpriteMeshInstance.GetComponent<SkinnedMeshRenderer>();
				MeshFilter meshFilter = mSpriteMeshInstance.GetComponent<MeshFilter>();
				MeshRenderer meshRenderer = mSpriteMeshInstance.GetComponent<MeshRenderer>();

				Material material = null;

				if(skinnedMeshRenderer)
				{
					material = skinnedMeshRenderer.sharedMaterial;
					DestroyImmediate(skinnedMeshRenderer);
				}else if(meshRenderer)
				{
					material = meshRenderer.sharedMaterial;
				}

				string path = AssetDatabase.GetAssetPath(spriteMesh);
				Mesh mesh = AssetDatabase.LoadAssetAtPath(path, typeof(Mesh)) as Mesh;

				if(!meshFilter)
				{
					meshFilter = mSpriteMeshInstance.gameObject.AddComponent<MeshFilter>();
				}

				if(meshFilter)
				{
					meshFilter.sharedMesh = mesh;
				}
			
				if(!meshRenderer)
				{
					meshRenderer = mSpriteMeshInstance.gameObject.AddComponent<MeshRenderer>();
				}

				if(meshRenderer)
				{
					meshRenderer.sharedMaterial = material;
				}
			}

			mSpriteMeshInstance.UpdateRenderer();
		}
	}
}
