using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TriangleNet.Geometry;

namespace Anima2D
{
	public class SpriteMeshCache : ScriptableObject, ISerializationCallbackReceiver
	{
		public SpriteMesh spriteMesh;
		public SpriteMeshInstance spriteMeshInstance;

		public List<Vertex> texVertices = new List<Vertex>();

		[SerializeField]
		List<IndexedEdge> mIndexedEdges = new List<IndexedEdge>();

		public List<IndexedEdge> indexedEdges {
			get { return mIndexedEdges; }
			set {
				mIndexedEdges.Clear();
				mIndexedEdges.AddRange(value);

				UpdateEdges();
			}
		}

		[NonSerialized]
		public List<Edge> edges = new List<Edge>();

		public List<Hole> holes = new List<Hole>();

		public List<int> indices = new List<int>();

		public List<BindInfo> bindPoses = new List<BindInfo>();

		[SerializeField]
		List<int> mSelectedVerticesIndices = new List<int>();

		[NonSerialized]
		public List<Vertex> selectedVertices = new List<Vertex>();
		public Vertex selectedVertex { get { return selectedVertices.Count == 1? selectedVertices[0] : null; } }
		public bool multiselection { get { return selectedVertices.Count > 1; } }

		public bool isDirty { get; set; }

		[SerializeField]
		int mSelectedEdgeIndex = -1;

		Edge mSelectedEdge = null;

		[SerializeField]
		int mSelectedHoleIndex = -1;

		Hole mSelectedHole = null;
		public Hole selectedHole {
			get {
				if(mSelectedHole != null && holes.Contains(mSelectedHole))
				{
					return mSelectedHole;
				}else{
					mSelectedHole = null;
				}
				
				return null;
			}
			set {
				mSelectedHole = value;
			}
		}

		[SerializeField]
		int mSelectedBoneIndex = -1;

		[NonSerialized]
		public BindInfo selectedBone = null;

		public Edge selectedEdge {
			get {
				if(mSelectedEdge != null && edges.Contains(mSelectedEdge))
				{
					return mSelectedEdge;
				}else{
					mSelectedEdge = null;
				}

				return null;
			}
			set {
				mSelectedEdge = value;
			}
		}

		public void Clear()
		{
			spriteMesh = null;
			spriteMeshInstance = null;
			selectedBone = null;
			selectedEdge = null;
			selectedVertices.Clear();
			texVertices.Clear();
			indexedEdges.Clear();
			indices.Clear();

			isDirty = false;

		}

		public void OnBeforeSerialize()
		{
			UpdateIndexedEdges();

			mSelectedVerticesIndices.Clear();

			for (int i = 0; i < selectedVertices.Count; i++)
			{
				mSelectedVerticesIndices.Add(texVertices.IndexOf(selectedVertices[i]));
			}

			mSelectedEdgeIndex = -1;

			if(!ReferenceEquals(selectedEdge,null))
			{
				mSelectedEdgeIndex = edges.IndexOf(selectedEdge);
			}

			mSelectedBoneIndex = -1;

			if(!ReferenceEquals(selectedBone,null))
			{
				mSelectedBoneIndex = bindPoses.IndexOf(selectedBone);
			}

			mSelectedHoleIndex = -1;
			
			if(!ReferenceEquals(selectedHole,null))
			{
				mSelectedHoleIndex = holes.IndexOf(selectedHole);
			}
		}
		
		public void OnAfterDeserialize()
		{
			UpdateEdges();

			selectedVertices.Clear();

			for (int i = 0; i < mSelectedVerticesIndices.Count; i++)
			{
				selectedVertices.Add(texVertices[mSelectedVerticesIndices[i]]);
			}

			selectedEdge = null;
			
			if(mSelectedEdgeIndex >= 0)
			{
				selectedEdge = edges[mSelectedEdgeIndex];
			}

			selectedBone = null;
			
			if(mSelectedBoneIndex >= 0)
			{
				selectedBone = bindPoses[mSelectedBoneIndex];
			}

			selectedHole = null;
			
			if(mSelectedHoleIndex >= 0)
			{
				selectedHole = holes[mSelectedHoleIndex];
			}
		}

		public void UpdateIndexedEdges()
		{
			indexedEdges.Clear();
			
			for (int i = 0; i < edges.Count; i++)
			{
				Edge edge = edges[i];
				IndexedEdge indexedEdge = new IndexedEdge(texVertices.IndexOf(edge.vertex1),
				                                          texVertices.IndexOf(edge.vertex2));
				indexedEdges.Add(indexedEdge);
			}
		}

		void UpdateEdges()
		{
			edges.Clear();
			for (int i = 0; i < mIndexedEdges.Count; i++)
			{
				IndexedEdge indexedEdge = mIndexedEdges[i];
				Edge edge = new Edge(texVertices[indexedEdge.index1],
				                       texVertices[indexedEdge.index2]);
				
				edges.Add(edge);
			}
		}

		public Vertex AddVertex(Vector2 position)
		{
			return AddVertex(position,null);
		}
		
		public Vertex AddVertex(Vector2 position, Edge edge)
		{
			Vertex vertex = new Vertex(position);
			
			texVertices.Add(vertex);
			
			if(edge != null)
			{
				edges.Add(new Edge(edge.vertex1,vertex));
				edges.Add(new Edge(edge.vertex2,vertex));
				edges.Remove(edge);
			}
			
			Triangulate();
			
			return vertex;
		}
		
		public void RemoveVertex(Vertex vertex, bool triangulate = true)
		{
			List<Edge> l_edges = new List<Edge>();
			foreach(Edge edge in edges)
			{
				if(edge.vertex1 == vertex || edge.vertex2 == vertex)
				{
					l_edges.Add(edge);
				}
			}
			
			if(l_edges.Count == 2)
			{
				Vertex vertex1 = l_edges[0].vertex1 != vertex ? l_edges[0].vertex1 : l_edges[0].vertex2;
				Vertex vertex2 = l_edges[1].vertex1 != vertex ? l_edges[1].vertex1 : l_edges[1].vertex2;
				
				edges.Remove(l_edges[0]);
				edges.Remove(l_edges[1]);
				AddEdge(vertex1,vertex2);
			}else{
				foreach(Edge edge in l_edges)
				{
					edges.Remove(edge);
				}
			}
			
			texVertices.Remove(vertex);
			
			if(triangulate)
			{
				Triangulate();
			}
		}
		
		public void AddEdge(Vertex vertex1, Vertex vertex2)
		{
			Edge newEdge = new Edge(vertex1,vertex2);
			
			if(!edges.Contains(newEdge))
			{
				edges.Add(newEdge);
				Triangulate();
			}
		}

		public void RemoveEdge(Edge edge)
		{
			if(edges.Contains(edge))
			{
				edges.Remove(edge);
				
				Triangulate();
			}
		}
		
		public void AddHole(Vector2 position)
		{
			holes.Add(new Hole(position));
			Triangulate();
		}
		
		public void RemoveHole(Hole hole, bool triangulate = true)
		{
			holes.Remove(hole);

			if(triangulate)
			{
				Triangulate();
			}
		}

		public void Triangulate()
		{
			indices.Clear();

			if(texVertices.Count >= 3)
			{
				InputGeometry inputGeometry = new InputGeometry(texVertices.Count);
				
				for(int i = 0; i < texVertices.Count; ++i)
				{
					Vector2 vertex = texVertices[i].vertex;
					inputGeometry.AddPoint(vertex.x,vertex.y);
				}

				for(int i = 0; i < edges.Count; ++i)
				{
					Edge edge = edges[i];
					inputGeometry.AddSegment(texVertices.IndexOf(edge.vertex1),
					                         texVertices.IndexOf(edge.vertex2));
				}

				for(int i = 0; i < holes.Count; ++i)
				{
					Vector2 hole = holes[i].vertex;
					inputGeometry.AddHole(hole.x,hole.y);
				}

				TriangleNet.Mesh trangleMesh = new TriangleNet.Mesh();
				
				trangleMesh.Triangulate(inputGeometry);


				foreach (TriangleNet.Data.Triangle triangle in trangleMesh.Triangles)
				{
					if(triangle.P0 >= 0 && triangle.P0 < texVertices.Count &&
					   triangle.P0 >= 0 && triangle.P1 < texVertices.Count &&
					   triangle.P0 >= 0 && triangle.P2 < texVertices.Count)
					{
						indices.Add(triangle.P0);
						indices.Add(triangle.P2);
						indices.Add(triangle.P1);
					}
				}
			}

			isDirty = true;
		}

		bool ContainsVector(Vector3 vectorToFind, List<Vector3> list, float epsilon, out int index)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 v = list [i];
				if ((v - vectorToFind).sqrMagnitude < epsilon)
				{
					index = i;
					return true;
				}
			}

			index = -1;
			return false;
		}

		public void BindBones()
		{
			if(spriteMeshInstance)
			{
				bindPoses.Clear();

				foreach(Bone2D bone in spriteMeshInstance.bones)
				{
					if(bone)
					{
						BindInfo bindInfo = new BindInfo();
						
						bindInfo.bindPose = bone.transform.worldToLocalMatrix * spriteMeshInstance.transform.localToWorldMatrix;
						bindInfo.boneLength = bone.localLength;
						bindInfo.path = BoneUtils.GetBonePath(bone);
						bindInfo.name = bone.name;

						bindPoses.Add(bindInfo);
					}
				}

				isDirty = true;
			}
		}

		public void CalculateAutomaticWeights()
		{
			CalculateAutomaticWeights(texVertices);
		}

		public void CalculateAutomaticWeights(List<Vertex> targetVertices)
		{
			if(texVertices.Count <= 0)
			{
				Debug.Log("Cannot calculate automatic weights from a SpriteMesh with no vertices.");
				return;
			}

			if(bindPoses.Count <= 0)
			{
				Debug.Log("Cannot calculate automatic weights. Specify bones to the SpriteMeshInstance.");
				return;
			}

			if(spriteMesh)
			{
				List<Vector3> vertices = new List<Vector3>(texVertices.Count);
				
				for (int i = 0; i < texVertices.Count; i++)
				{
					Vertex vertex = texVertices[i];
					vertices.Add(vertex.vertex);
				}

				List<Vector3> controlPoints = new List<Vector3>(bindPoses.Count*2);
				List<IndexedEdge> controlPointEdges = new List<IndexedEdge>(bindPoses.Count);
				
				foreach(BindInfo bindInfo in bindPoses)
				{
					Vector3 tip = SpriteMeshUtils.VertexToTexCoord(spriteMesh,bindInfo.position);
					Vector3 tail = SpriteMeshUtils.VertexToTexCoord(spriteMesh,bindInfo.endPoint);
					
					int index1 = -1;
					
					if(!ContainsVector(tip,controlPoints,0.01f, out index1))
					{
						index1 = controlPoints.Count;
						controlPoints.Add(tip);
					}
					
					int index2 = -1;
					
					if(!ContainsVector(tail,controlPoints,0.01f, out index2))
					{
						index2 = controlPoints.Count;
						controlPoints.Add(tail);
					}
					
					IndexedEdge edge = new IndexedEdge(index1, index2);
					controlPointEdges.Add(edge);
					
				}
				
				float[,] weightArray;
				
				UpdateIndexedEdges();
				
				BbwPlugin.CalculateBbw(vertices.ToArray(),
				                       indices.ToArray(),
				                       indexedEdges.ToArray(),
				                       controlPoints.ToArray(),
				                       controlPointEdges.ToArray(),
				                       out weightArray);
				
				FillBoneWeights(targetVertices, weightArray);

				isDirty = true;
			}
		}

		void FillBoneWeights(List<Vertex> targetVertices, float[,] weights)
		{
			List<float> vertexWeights = new List<float>();
			
			for(int i = 0; i < targetVertices.Count; ++i)
			{
				int index = texVertices.IndexOf(targetVertices[i]);
				
				vertexWeights.Clear();
				
				Vertex vertex = targetVertices[i];
				
				for(int j = 0; j < bindPoses.Count; ++j)
				{
					vertexWeights.Add(weights[j,index]);
				}
				
				FillBoneWeight(vertex.boneWeight,vertexWeights);
			}
		}
		
		void FillBoneWeight(BoneWeight2 boneWeight, List<float> weights)
		{
			boneWeight.weight0 = weights.Max();
			boneWeight.boneIndex0 = weights.IndexOf(boneWeight.weight0);

			weights[boneWeight.boneIndex0] = 0f;

			boneWeight.weight1 = weights.Max();
			boneWeight.boneIndex1 = weights.IndexOf(boneWeight.weight1);
			
			weights[boneWeight.boneIndex1] = 0f;

			boneWeight.weight2 = weights.Max();
			boneWeight.boneIndex2 = weights.IndexOf(boneWeight.weight2);
			
			weights[boneWeight.boneIndex2] = 0f;

			boneWeight.weight3 = weights.Max();
			boneWeight.boneIndex3 = weights.IndexOf(boneWeight.weight3);

			float sum = boneWeight.weight0 + 
				boneWeight.weight1 +
				boneWeight.weight2 +
				boneWeight.weight3;

			if(sum > 0f)
			{
				boneWeight.weight0 /= sum;
				boneWeight.weight1 /= sum;
				boneWeight.weight2 /= sum;
				boneWeight.weight3 /= sum;
			}else{
				boneWeight.weight0 = 1f;
			}
		}

		public void SmoothVertices(List<Vertex> targetVertices)
		{
			float[,] weights = new float[texVertices.Count,bindPoses.Count];
			Array.Clear(weights,0,weights.Length);

			List<int> usedIndices = new List<int>();

			for (int i = 0; i < texVertices.Count; i++)
			{
				usedIndices.Clear();

				Vertex vertex = texVertices [i];
				BoneWeight2 weight = vertex.boneWeight;

				weights[i,weight.boneIndex0] = weight.weight0;
				usedIndices.Add(weight.boneIndex0);

				if(!usedIndices.Contains(weight.boneIndex1))
				{
					weights[i,weight.boneIndex1] = weight.weight1;
					usedIndices.Add(weight.boneIndex1);
				}
				if(!usedIndices.Contains(weight.boneIndex2))
				{
					weights[i,weight.boneIndex2] = weight.weight2;
					usedIndices.Add(weight.boneIndex2);
				}
				if(!usedIndices.Contains(weight.boneIndex3))
				{
					weights[i,weight.boneIndex3] = weight.weight3;
					usedIndices.Add(weight.boneIndex3);
				}
			}

			float[] denominator = new float[texVertices.Count];
			float[,] smoothedWeights = new float[texVertices.Count,bindPoses.Count]; 
			Array.Clear(smoothedWeights,0,smoothedWeights.Length);

			for (int i = 0; i < indices.Count / 3; ++i)
			{
				for (int j = 0; j < 3; ++j)
				{
					int j1 = (j + 1) % 3;
					int j2 = (j + 2) % 3;

					for(int k = 0; k < bindPoses.Count; ++k)
					{
						smoothedWeights[indices[i*3 + j],k] += weights[indices[i*3 + j1],k] + weights[indices[i*3 + j2],k]; 
					}

					denominator[indices[i*3 + j]] += 2;
				}
			}

			for (int i = 0; i < texVertices.Count; ++i)
			{
				for (int j = 0; j < bindPoses.Count; ++j)
				{
					smoothedWeights[i,j] /= denominator[i];
				}
			}

			float[,] smoothedWeightsTransposed = new float[bindPoses.Count,texVertices.Count]; 

			for (int i = 0; i < texVertices.Count; ++i)
			{
				for (int j = 0; j < bindPoses.Count; ++j)
				{
					smoothedWeightsTransposed[j,i] = smoothedWeights[i,j];
				}
			}

			FillBoneWeights(targetVertices, smoothedWeightsTransposed);
			
			isDirty = true;
		}
		
		public void ClearWeights()
		{
			bindPoses.Clear();

			isDirty = true;
		}

		public void SelectVertex(Vertex vertex, bool append)
		{
			if(!IsSelected(vertex))
			{
				if(!append)
				{
					selectedVertices.Clear();
				}
				selectedVertices.Add(vertex);
			}
		}
		
		public bool IsSelected(Vertex vertex)
		{
			return selectedVertices.Contains(vertex);
		}
		
		public void UnselectVertex(Vertex vertex)
		{
			if(IsSelected(vertex))
			{
				selectedVertices.Remove(vertex);
			}
		}
		
		public void UnselectVertices()
		{
			selectedVertices.Clear();
		}
	}
}
