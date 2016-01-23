using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Anima2D
{
	public class ContextMenu
	{
		[MenuItem("Assets/Create/Anima2D/SpriteMesh", true)]
		static bool ValidateCreateSpriteMesh(MenuCommand menuCommand)
		{
			bool validated = false;

			validated = ((Selection.activeObject as Sprite) != null);

			List<Texture2D> selectedTextures = Selection.objects.ToList().Where( o => o is Texture2D).ToList().ConvertAll( o => o as Texture2D);

			validated = validated || selectedTextures.Count > 0;

			return validated;
		}

		[MenuItem("Assets/Create/Anima2D/SpriteMesh", false)]
		static void CreateSpriteMesh(MenuCommand menuCommand)
		{
			List<Texture2D> selectedTextures = Selection.objects.ToList().Where( o => o is Texture2D).ToList().ConvertAll( o => o as Texture2D);

			foreach(Texture2D texture in selectedTextures)
			{
				SpriteMeshUtils.CreateSpriteMesh(texture);
			}

			if(selectedTextures.Count == 0)
			{
				SpriteMeshUtils.CreateSpriteMesh(Selection.activeObject as Sprite);
			}
		}
		
		[MenuItem("GameObject/2D Object/SpriteMesh", false, 10)]
		static void ContextCreateSpriteMesh(MenuCommand menuCommand)
		{
			GameObject spriteRendererGO = Selection.activeGameObject;
			SpriteRenderer spriteRenderer = null;
			SpriteMesh spriteMesh = null;
			
			if(spriteRendererGO)
			{
				spriteRenderer = spriteRendererGO.GetComponent<SpriteRenderer>();
			}
			
			if(spriteRenderer)
			{
				spriteMesh = SpriteMeshUtils.CreateSpriteMesh(spriteRenderer.sprite);
			}
			
			if(spriteMesh)
			{
				Undo.DestroyObjectImmediate(spriteRenderer);
				
				string spriteMeshPath = AssetDatabase.GetAssetPath(spriteMesh);
				
				Mesh mesh = AssetDatabase.LoadAssetAtPath(spriteMeshPath,typeof(Mesh)) as Mesh;
				Material material = AssetDatabase.LoadAssetAtPath(spriteMeshPath,typeof(Material)) as Material;
				
				SpriteMeshInstance spriteMeshRenderer = spriteRendererGO.AddComponent<SpriteMeshInstance>();
				spriteMeshRenderer.spriteMesh = spriteMesh;
				
				MeshFilter meshFilter = spriteRendererGO.AddComponent<MeshFilter>();
				meshFilter.sharedMesh = mesh;
				
				MeshRenderer renderer = spriteRendererGO.AddComponent<MeshRenderer>();
				renderer.sharedMaterial = material;
				
				Undo.RegisterCreatedObjectUndo(meshFilter, "Create SpriteMesh");
				Undo.RegisterCreatedObjectUndo(renderer, "Create SpriteMesh");
				
				Selection.activeGameObject = spriteRendererGO;
			}else{
				Debug.Log("Select a SpriteRenderer with a Sprite to convert to SpriteMesh");
			}
		}

		[MenuItem("GameObject/2D Object/Bone &#b", false, 10)]
		static void CreateBone(MenuCommand menuCommand)
		{
			GameObject bone = new GameObject("New bone");
			Bone2D boneComponent = bone.AddComponent<Bone2D>();

			Undo.RegisterCreatedObjectUndo(bone, "Create bone");

			bone.transform.position = GetDefaultInstantiatePosition();

			GameObject selectedGO = Selection.activeGameObject;
			if(selectedGO)
			{
				bone.transform.parent = selectedGO.transform;
				bone.transform.localPosition = Vector3.zero;
				bone.transform.localRotation = Quaternion.identity;
				bone.transform.localScale = Vector3.one;
				
				Bone2D selectedBone = selectedGO.GetComponent<Bone2D>();

				if(selectedBone)
				{
					bone.transform.position = selectedBone.endPosition;

					if(!selectedBone.child)
					{
						selectedBone.child = boneComponent;
					}
				}
			}

			Selection.activeGameObject = bone;
		}

		[MenuItem("GameObject/2D Object/IK CCD &#c", false, 10)]
		static void CreateIkCCD(MenuCommand menuCommand)
		{
			GameObject ikCCD = new GameObject("New Ik CCD");
			Undo.RegisterCreatedObjectUndo(ikCCD,"Crate Ik CCD");

			IkCCD2D ikCCDComponent = ikCCD.AddComponent<IkCCD2D>();
			ikCCD.transform.position = GetDefaultInstantiatePosition();
			
			GameObject selectedGO = Selection.activeGameObject;
			if(selectedGO)
			{
				ikCCD.transform.parent = selectedGO.transform;
				ikCCD.transform.localPosition = Vector3.zero;

				Bone2D selectedBone = selectedGO.GetComponent<Bone2D>();
				
				if(selectedBone)
				{
					ikCCD.transform.parent = selectedBone.root.transform.parent;
					ikCCD.transform.position = selectedBone.endPosition;
					ikCCDComponent.numBones = selectedBone.chainLength;
					ikCCDComponent.target = selectedBone;
				}
			}
			
			ikCCD.transform.rotation = Quaternion.identity;
			ikCCD.transform.localScale = Vector3.one;

			Selection.activeGameObject = ikCCD;
		}

		[MenuItem("GameObject/2D Object/IK Limb &#l", false, 10)]
		static void CreateIkLimb(MenuCommand menuCommand)
		{
			GameObject ikLimb = new GameObject("New Ik Limb");
			Undo.RegisterCreatedObjectUndo(ikLimb,"Crate Ik Limb");
			
			IkLimb2D ikLimbComponent = ikLimb.AddComponent<IkLimb2D>();
			ikLimb.transform.position = GetDefaultInstantiatePosition();
			
			GameObject selectedGO = Selection.activeGameObject;
			if(selectedGO)
			{
				ikLimb.transform.parent = selectedGO.transform;
				ikLimb.transform.localPosition = Vector3.zero;
				
				Bone2D selectedBone = selectedGO.GetComponent<Bone2D>();
				
				if(selectedBone)
				{
					ikLimb.transform.parent = selectedBone.root.transform.parent;
					ikLimb.transform.position = selectedBone.endPosition;
					ikLimbComponent.numBones = selectedBone.chainLength;
					ikLimbComponent.target = selectedBone;
				}
			}
			
			ikLimb.transform.rotation = Quaternion.identity;
			ikLimb.transform.localScale = Vector3.one;
			
			Selection.activeGameObject = ikLimb;
		}

		[MenuItem("Assets/Create/Anima2D/Pose")]
		public static void CreatePose(MenuCommand menuCommand)
		{
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);

			if(System.IO.File.Exists(path))
			{
				path = System.IO.Path.GetDirectoryName(path);
			}
			
			path += "/";
			
			if(System.IO.Directory.Exists(path))
			{
				path += "New pose.asset";

				ScriptableObjectUtility.CreateAsset<Pose>(path);
			}
		}

		static Vector3 GetDefaultInstantiatePosition()
		{
			Vector3 result = Vector3.zero;
			if (SceneView.lastActiveSceneView)
			{
				if (SceneView.lastActiveSceneView.in2DMode)
				{
					result = SceneView.lastActiveSceneView.camera.transform.position;
					result.z = 0f;
				}
				else
				{
					PropertyInfo prop = typeof(SceneView).GetProperty("cameraTargetPosition",BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

					result = (Vector3) prop.GetValue(SceneView.lastActiveSceneView,null);
				}
			}
			return result;
		}
	}
}
