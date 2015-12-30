using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Anima2D 
{
	public class SpriteMeshUtils
	{
		public static SpriteMesh CreateSpriteMesh(Sprite sprite)
		{
			SpriteMesh spriteMesh = null;

			if(sprite)
			{
				string spritePath = AssetDatabase.GetAssetPath(sprite);
				string directory = Path.GetDirectoryName(spritePath);
				string prefabPath = AssetDatabase.GenerateUniqueAssetPath(directory + Path.DirectorySeparatorChar + sprite.name + ".prefab");

				GameObject go = new GameObject(sprite.name);
				GameObject prefab = PrefabUtility.CreatePrefab(prefabPath,go,ReplacePrefabOptions.ConnectToPrefab);
				GameObject.DestroyImmediate(go);

				spriteMesh = ScriptableObject.CreateInstance<SpriteMesh>();
				spriteMesh.name = sprite.name;
				
				InitFromSprite(spriteMesh,sprite);

				AssetDatabase.AddObjectToAsset(spriteMesh,prefab);

				UpdateAssets(spriteMesh);

				AssetDatabase.SaveAssets();
				AssetDatabase.ImportAsset(prefabPath);

				Selection.activeObject = prefab;
			}

			return spriteMesh;
		}

		public static void CreateSpriteMesh(Texture2D texture)
		{
			if(texture)
			{
				Object[] objects = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture));

				for (int i = 0; i < objects.Length; i++)
				{
					Object o = objects [i];
					Sprite sprite = o as Sprite;
					if (sprite) {
						EditorUtility.DisplayProgressBar ("Processing " + texture.name, sprite.name, (i+1) / (float)objects.Length);
						CreateSpriteMesh (sprite);
					}
				}

				EditorUtility.ClearProgressBar();
			}
		}

		public static void UpdateAssets(SpriteMesh spriteMesh)
		{
			if(spriteMesh)
			{
				string spriteMeshPath = AssetDatabase.GetAssetPath(spriteMesh);

				GameObject go = AssetDatabase.LoadAssetAtPath(spriteMeshPath,typeof(GameObject)) as GameObject;
				Mesh mesh = AssetDatabase.LoadAssetAtPath(spriteMeshPath,typeof(Mesh)) as Mesh;
				Material material = AssetDatabase.LoadAssetAtPath(spriteMeshPath,typeof(Material)) as Material;

				spriteMesh.name = go.name;

				if(!mesh)
				{
					mesh = new Mesh();
					AssetDatabase.AddObjectToAsset(mesh,spriteMeshPath);
				}

				mesh.name = go.name;

				if(!material)
				{
					material = new Material(Shader.Find("Unlit/Transparent"));
					material.mainTexture = spriteMesh.sprite.texture;
					AssetDatabase.AddObjectToAsset(material,spriteMeshPath);
				}

				material.name = go.name;

				List<Vector3> vertices = new List<Vector3>(spriteMesh.texVertices.Count);
				List<Vector2> uvs = new List<Vector2>(spriteMesh.texVertices.Count);
				List<Vector3> normals = new List<Vector3>(spriteMesh.texVertices.Count);
				List<BoneWeight> boneWeights = new List<BoneWeight>(spriteMesh.texVertices.Count);

				for (int i = 0; i < spriteMesh.texVertices.Count; i++)
				{
					Vertex vertex = spriteMesh.texVertices[i];
					vertices.Add(TexCoordToVertex(spriteMesh,vertex.vertex));
					uvs.Add(new Vector2(vertex.vertex.x / spriteMesh.sprite.texture.width,
					                    vertex.vertex.y / spriteMesh.sprite.texture.height));
					normals.Add(Vector3.back);

					List< KeyValuePair<int,float> > pairs = new List<KeyValuePair<int, float>>();
					pairs.Add(new KeyValuePair<int, float>(vertex.boneWeight.boneIndex0,vertex.boneWeight.weight0));
					pairs.Add(new KeyValuePair<int, float>(vertex.boneWeight.boneIndex1,vertex.boneWeight.weight1));
					pairs.Add(new KeyValuePair<int, float>(vertex.boneWeight.boneIndex2,vertex.boneWeight.weight2));
					pairs.Add(new KeyValuePair<int, float>(vertex.boneWeight.boneIndex3,vertex.boneWeight.weight3));

					pairs = pairs.OrderByDescending(s=>s.Value).ToList();

					BoneWeight boneWeight = new BoneWeight();
					boneWeight.boneIndex0 = pairs[0].Key;
					boneWeight.boneIndex1 = pairs[1].Key;
					boneWeight.boneIndex2 = pairs[2].Key;
					boneWeight.boneIndex3 = pairs[3].Key;
					boneWeight.weight0 = pairs[0].Value;
					boneWeight.weight1 = pairs[1].Value;
					boneWeight.weight2 = pairs[2].Value;
					boneWeight.weight3 = pairs[3].Value;
					boneWeights.Add(boneWeight);
				}

				mesh.Clear();
				mesh.vertices = vertices.ToArray();
				mesh.uv = uvs.ToArray();
				mesh.triangles = spriteMesh.indices.ToArray();
				mesh.normals = normals.ToArray();
				mesh.boneWeights = boneWeights.ToArray();
				mesh.bindposes = spriteMesh.bindPoses.ConvertAll( bindPose => bindPose.bindPose ).ToArray();
				mesh.RecalculateBounds();
			}
		}

		static void InitFromSprite(SpriteMesh spriteMesh, Sprite sprite)
		{
			SerializedObject spriteMeshSO = new SerializedObject(spriteMesh);
			
			spriteMeshSO.Update();
			
			SerializedProperty spriteProp = spriteMeshSO.FindProperty("sprite");

			spriteProp.objectReferenceValue = sprite;

			spriteMeshSO.ApplyModifiedProperties();

			GetInfoFromSprite(sprite,spriteMesh.texVertices,spriteMesh.edges,spriteMesh.indices);

			EditorUtility.SetDirty(spriteMesh);
		}

		public static void GetInfoFromSprite(Sprite sprite, List<Vertex> texVertices, List<IndexedEdge> indexedEdges, List<int> indices)
		{
			texVertices.Clear();
			indices.Clear();
			indexedEdges.Clear();

			if(sprite)
			{
				Vector2[] uvs = SpriteUtility.GetSpriteUVs(sprite,false);
				
				for(int i = 0; i < uvs.Length; ++i)
				{
					Vector2 texCoord = new Vector2(uvs[i].x * sprite.texture.width,
					                               uvs[i].y * sprite.texture.height);
					
					texVertices.Add(new Vertex(texCoord));
				}

				ushort[] l_indices = sprite.triangles;
				
				for(int i = 0; i < l_indices.Length; ++i)
				{	
					indices.Add((int)l_indices[i]);
				}


				HashSet<Edge> edgesSet = new HashSet<Edge>();
				
				for(int i = 0; i < indices.Count; i += 3)
				{
					int index1 = indices[i];
					int index2 = indices[i+1];
					int index3 = indices[i+2];
					
					Edge edge1 = new Edge(texVertices[index1],texVertices[index2]);
					Edge edge2 = new Edge(texVertices[index2],texVertices[index3]);
					Edge edge3 = new Edge(texVertices[index1],texVertices[index3]);
					
					if(edgesSet.Contains(edge1))
					{
						edgesSet.Remove(edge1);
					}else{
						edgesSet.Add(edge1);
					}
					
					if(edgesSet.Contains(edge2))
					{
						edgesSet.Remove(edge2);
					}else{
						edgesSet.Add(edge2);
					}
					
					if(edgesSet.Contains(edge3))
					{
						edgesSet.Remove(edge3);
					}else{
						edgesSet.Add(edge3);
					}
				}

				foreach(Edge edge in edgesSet)
				{
					indexedEdges.Add(new IndexedEdge(texVertices.IndexOf(edge.vertex1),
					                                 texVertices.IndexOf(edge.vertex2)));
				}
			}
		}

		public static Vector3 TexCoordToVertex(SpriteMesh spriteMesh, Vector2 texVertex)
		{
			Vector3 vertex = Vector3.zero;

			if(spriteMesh != null)
			{
				Vector3 pivotPoint = (Vector3)GetPivotPoint(spriteMesh.sprite);
				vertex = ((Vector3)texVertex - pivotPoint) / spriteMesh.sprite.pixelsPerUnit;
			}

			return vertex;
		}

		public static Vector2 VertexToTexCoord(SpriteMesh spriteMesh, Vector3 vertex)
		{
			Vector2 texCoord = Vector3.zero;
			
			if(spriteMesh != null)
			{
				Vector3 pivotPoint = (Vector3)GetPivotPoint(spriteMesh.sprite);
				texCoord = vertex * spriteMesh.sprite.pixelsPerUnit + pivotPoint;
			}
			
			return texCoord;
		}

		public static Vector2 GetPivotPoint(SpriteMesh spriteMesh)
		{
			return GetPivotPoint(spriteMesh.sprite);
		}

		public static Vector2 GetPivotPoint(Sprite sprite)
		{
			Vector3 pivotPoint = -1f * sprite.bounds.min * sprite.pixelsPerUnit + (Vector3)sprite.rect.position;
			return new Vector2(pivotPoint.x,pivotPoint.y);
		}
	}

}
