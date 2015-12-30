using System;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using UnityEngine.Sprites;
using UnityEditor.Sprites;

namespace Anima2D 
{
	class SpriteMeshEditorWindow : TextureEditorWindow
	{
		WeightEditor weightEditor = new WeightEditor();

		Material mMeshGuiMaterial;
		Material meshGuiMaterial {
			get {
				if(!mMeshGuiMaterial)
				{
					mMeshGuiMaterial = new Material(Shader.Find("Hidden/Internal-GUITextureClip"));
					mMeshGuiMaterial.hideFlags = HideFlags.DontSave;
				}

				return mMeshGuiMaterial;
			}
		}

		Texture2D mDotTexture;
		Texture2D dotTexture {
			get {
				if(!mDotTexture)
				{
					mDotTexture = EditorGUIUtility.Load("Anima2D/dot.png") as Texture2D;
					mDotTexture.hideFlags = HideFlags.DontSave;
				}
				
				return mDotTexture;
			}
		}

		public static SpriteMeshEditorWindow Initialize()
		{
			return EditorWindow.GetWindow<SpriteMeshEditorWindow>("SpriteMesh Editor");
		}

		[MenuItem("Window/SpriteMesh Editor",false,2015)]
		static void ContextInitialize()
		{
			SpriteMeshEditorWindow window = Initialize();
			window.UpdateFromSelection();
		}

		public static void Initialize(SpriteMesh spriteMesh)
		{
			SpriteMeshEditorWindow window = Initialize();
			window.spriteMesh = spriteMesh;
		}

		void OnEnable()
		{
			Undo.undoRedoPerformed -= UndoRedoPerformed;
			Undo.undoRedoPerformed += UndoRedoPerformed;

			showBones = true;
		}

		void OnDisable()
		{
			Undo.undoRedoPerformed -= UndoRedoPerformed;

			if(mMeshGuiMaterial)
			{
				DestroyImmediate(mMeshGuiMaterial);
			}
		}

		SpriteMeshCache mSpriteMeshCache;
		SpriteMeshInstance spriteMeshInstance { get; set; }
		SpriteMesh spriteMesh { get; set; }
		
		Vector3 spritePivotPoint {
			get {
				Vector3 l_pivotPoint = -1f * spriteMesh.sprite.bounds.min;
				l_pivotPoint.z = 0f;
				return l_pivotPoint * spriteMesh.sprite.pixelsPerUnit;
			}
		}
		
		Vector3[] spriteVertices
		{
			get {
				Vector2[] vertices2 = spriteMesh.sprite.vertices;
				Vector3[] vertices3 = new Vector3[vertices2.Length];

				for(int i = 0; i < vertices2.Length; ++i)
				{
					vertices3[i].x = vertices2[i].x;
					vertices3[i].y = vertices2[i].y;
					vertices3[i].z = 0f;
				}
				
				return vertices3;
			}
		}
		
		bool isBound
		{
			get {
				return mSpriteMeshCache.bindPoses.Count > 0f;
			}
		}

		//bool snapPixel { get; set; }

		bool showBones { get; set; }
		bool showTriangles { get; set; }

		bool showWeightEditor
		{
			get {
				return isBound;
			}
		}

		Vector2 mMousePosition;
		Vector2 mMouseDownPosition;
		Vector2 mMouseDownPositionTextureSpace;
		Vector2 mMousePositionTextureSpace;

		Vertex mHoveredVertex = null;
		Hole mHoveredHole = null;
		
		Edge mHoveredEdge = null;
		Edge mClosestEdge = null;

		BindInfo mHoveredBone = null;

		Matrix4x4 mSpriteMatrix = new Matrix4x4();
		Matrix4x4 mSpriteMeshMatrix = new Matrix4x4();

		bool addVertices = false;
		bool addHoles = false;

		bool mSetFromRect = false;

		Rect mSelectionRect;
		List<Vertex> mSelectionToolVertices = new List<Vertex>();

		float mPointSize = 6f;

		void UndoRedoPerformed()
		{
			Repaint();
		}

		override protected void HandleZoom()
		{
			if(showWeightEditor && weightEditor.IsHovered())
			{
				return;
			}

			base.HandleZoom();
		}

		void HandleAddVertex()
		{
			if(addVertices)
			{
				switch (Event.current.type)
				{
				case EventType.MouseDown:
					if(Event.current.button == 0 &&
					   mHoveredVertex == null &&
					   !Event.current.alt)
					{
						Undo.IncrementCurrentGroup();
						Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"add point");

						Edge edge = null;

						if(mHoveredEdge != null)
						{
							edge = mHoveredEdge;
						}

						if(Event.current.shift)
						{
							edge = mClosestEdge;
						}

						mSpriteMeshCache.AddVertex(mMousePositionTextureSpace,edge);

						Event.current.Use();
						Repaint();
					}
					break;
				case EventType.MouseMove:
					Event.current.Use();
					break;
				case EventType.MouseUp:
					if(Event.current.button == 1)
					{
						addVertices = false;
						Event.current.Use();
						EditorGUIUtility.SetWantsMouseJumping(0);
						Repaint();
					}
					break;
				case EventType.Repaint:

					if(mHoveredVertex == null)
					{
						if((Event.current.shift && mClosestEdge != null) ||
						   mHoveredEdge != null)
						{
							DrawSplitEdge(mClosestEdge,mMousePositionTextureSpace);
						}else{
							DrawVertex(mMousePositionTextureSpace);
						}
					}

					break;
				}
			}
		}

		void HandleDeleteVertex()
		{
			switch(Event.current.type)
			{
			case EventType.KeyDown:
				if(GUIUtility.hotControl == 0 &&
				   mSpriteMeshCache.selectedVertices.Count > 0 &&
				   (Event.current.keyCode == KeyCode.Backspace ||
				   Event.current.keyCode == KeyCode.Delete))
				{
					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"delete vertex");

					foreach(Vertex vertex in mSpriteMeshCache.selectedVertices)
					{
						mSpriteMeshCache.RemoveVertex(vertex,false);
					}

					mSpriteMeshCache.UnselectVertices();

					mSpriteMeshCache.Triangulate();

					Event.current.Use();
				}
				break;
			}
		}

		void HandleDragVertex()
		{
			int dragVertexControlId = GUIUtility.GetControlID("DragVertex".GetHashCode(), FocusType.Keyboard);

			switch (Event.current.type)
			{
			case EventType.MouseDown:
				if(GUIUtility.hotControl == 0 &&
				   Event.current.button == 0 &&
				   mHoveredVertex != null &&
				   !Event.current.alt)
				{
					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"select vertex");

					if(!mSpriteMeshCache.IsSelected(mHoveredVertex))
					{
						mSpriteMeshCache.SelectVertex(mHoveredVertex,EditorGUI.actionKey);
					}else{
						if(EditorGUI.actionKey)
						{
							mSpriteMeshCache.UnselectVertex(mHoveredVertex);
						}
					}

					mSpriteMeshCache.selectedHole = null;
					mSpriteMeshCache.selectedEdge = null;

					GUIUtility.hotControl = dragVertexControlId;
					Event.current.Use();
					EditorGUIUtility.SetWantsMouseJumping(1);

					Repaint();
				}
				break;

			case EventType.MouseUp:
				
				if(GUIUtility.hotControl == dragVertexControlId &&
				   Event.current.button == 0)
				{
					GUIUtility.hotControl = 0;
					Event.current.Use();
					EditorGUIUtility.SetWantsMouseJumping(0);
					Repaint();
				}
				break;
			
			case EventType.MouseDrag:
				if(GUIUtility.hotControl == dragVertexControlId &&
				   Event.current.button == 0 &&
				   !Event.current.alt)
				{
					Vector2 delta = new Vector2(Event.current.delta.x,-Event.current.delta.y) / m_Zoom;

					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"move point");

					foreach(Vertex vertex in mSpriteMeshCache.selectedVertices)
					{
						vertex.vertex += delta;
					}
					
					mSpriteMeshCache.Triangulate();
					Repaint();
				}
				
				break;
			}
		}

		void HandleDragEdge()
		{
			int dragEdgeControlId = GUIUtility.GetControlID("DragEdge".GetHashCode(), FocusType.Keyboard);
			
			switch (Event.current.type)
			{
			case EventType.MouseDown:
				if(GUIUtility.hotControl == 0 &&
				   Event.current.button == 0 &&
				   mHoveredEdge != null &&
				   !Event.current.alt)
				{
					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"select edge");

					mSpriteMeshCache.selectedEdge = mHoveredEdge;
					
					mSpriteMeshCache.UnselectVertices();
					mSpriteMeshCache.selectedHole = null;

					GUIUtility.hotControl = dragEdgeControlId;
					Event.current.Use();
					EditorGUIUtility.SetWantsMouseJumping(1);
					
					Repaint();
				}
				
				break;
				
			case EventType.MouseUp:
				
				if(GUIUtility.hotControl == dragEdgeControlId &&
				   Event.current.button == 0)
				{
					GUIUtility.hotControl = 0;
					Event.current.Use();
					EditorGUIUtility.SetWantsMouseJumping(0);
					Repaint();
				}
				break;
			case EventType.MouseDrag:
				if(GUIUtility.hotControl == dragEdgeControlId &&
				   Event.current.button == 0 &&
				   !Event.current.alt)
				{
					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"move edge");
					
					Vector2 delta = new Vector2(Event.current.delta.x,-Event.current.delta.y) / m_Zoom;

					mSpriteMeshCache.selectedEdge.vertex1.vertex += delta;
					mSpriteMeshCache.selectedEdge.vertex2.vertex += delta;
					
					mSpriteMeshCache.Triangulate();
					
					Repaint();
				}
				
				break;
			}
		}

		void HandleDeleteEdge()
		{
			switch(Event.current.type)
			{
			case EventType.KeyDown:
				if(GUIUtility.hotControl == 0 &&
				   mSpriteMeshCache.selectedEdge != null &&
				   (Event.current.keyCode == KeyCode.Backspace ||
				    Event.current.keyCode == KeyCode.Delete))
				{
					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"delete edge");
					
					mSpriteMeshCache.RemoveEdge(mSpriteMeshCache.selectedEdge);
					mSpriteMeshCache.selectedEdge = null;
					mSpriteMeshCache.Triangulate();
					
					Event.current.Use();
				}
				break;
			}
		}

		void HandleAddEdge()
		{
			if(GUIUtility.hotControl == 0 &&
			   mSpriteMeshCache.selectedVertex != null &&
			   Event.current.shift)
			{
				switch(Event.current.type)
				{
				case EventType.MouseDown:
					if(Event.current.button == 0 &&
					   mSpriteMeshCache.selectedVertex != null &&
					   mSpriteMeshCache.selectedVertex != mHoveredVertex)
					{
						Undo.IncrementCurrentGroup();
						Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"add edge");

						Vertex targetVertex = mHoveredVertex;

						if(targetVertex == null)
						{
							targetVertex = mSpriteMeshCache.AddVertex(mMousePositionTextureSpace,mHoveredEdge);
						}

						if(targetVertex != null)
						{
							mSpriteMeshCache.AddEdge(mSpriteMeshCache.selectedVertex,targetVertex);
							mSpriteMeshCache.SelectVertex(targetVertex,false);
						}

						Event.current.Use();
						Repaint();
					}
					break;
				case EventType.MouseMove:
					Event.current.Use();
					break;
				case EventType.Repaint:
					Vector3 targetPosition = mMousePositionTextureSpace;
					
					if(mHoveredVertex != null)
					{
						targetPosition = mHoveredVertex.vertex;
					}else{

						if(mHoveredVertex == null)
						{
							DrawSplitEdge(mHoveredEdge, mMousePositionTextureSpace);
						}
					}

					DrawEdge(mSpriteMeshCache.selectedVertex.vertex, targetPosition, 2f / m_Zoom,  Color.yellow);
					DrawHoveredVertex();

					break;
				}
			}
		}

		void HandleAddHole()
		{
			if(addHoles)
			{
				switch (Event.current.type)
				{
				case EventType.MouseDown:
					if(Event.current.button == 0 &&
					   mHoveredVertex == null &&
					   mHoveredEdge == null &&
					   !Event.current.alt)
					{
						Undo.IncrementCurrentGroup();
						Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"add hole");
						
						mSpriteMeshCache.AddHole(mMousePositionTextureSpace);
						
						Event.current.Use();
						Repaint();
					}
					break;
				case EventType.MouseMove:
					Event.current.Use();
					break;
				case EventType.MouseUp:
					if(Event.current.button == 1)
					{
						addHoles = false;
						Event.current.Use();
						EditorGUIUtility.SetWantsMouseJumping(0);
						Repaint();
					}
					break;
				case EventType.Repaint:
					
					if(mHoveredVertex == null &&
					   mHoveredEdge == null)
					{
						DrawHole(mMousePositionTextureSpace);
					}
					
					break;
				}
			}
		}
		
		void HandleDeleteHole()
		{
			switch(Event.current.type)
			{
			case EventType.KeyDown:
				if(GUIUtility.hotControl == 0 &&
				   mSpriteMeshCache.selectedHole != null &&
				   (Event.current.keyCode == KeyCode.Backspace ||
				   Event.current.keyCode == KeyCode.Delete))
				{
					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"delete hole");

					mSpriteMeshCache.RemoveHole(mSpriteMeshCache.selectedHole);
					
					mSpriteMeshCache.UnselectVertices();
					
					Event.current.Use();
				}
				break;
			}
		}
		
		void HandleDragHole()
		{
			int dragHoleControlId = GUIUtility.GetControlID("DragHole".GetHashCode(), FocusType.Keyboard);
			
			switch (Event.current.type)
			{
			case EventType.MouseDown:
				if(GUIUtility.hotControl == 0 &&
				   Event.current.button == 0 &&
				   mHoveredHole != null &&
				   !Event.current.alt)
				{
					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"select hole");

					mSpriteMeshCache.selectedHole = mHoveredHole;

					mSpriteMeshCache.UnselectVertices();
					mSpriteMeshCache.selectedEdge = null;
					
					GUIUtility.hotControl = dragHoleControlId;
					Event.current.Use();
					EditorGUIUtility.SetWantsMouseJumping(1);
					
					Repaint();
				}
				break;
				
			case EventType.MouseUp:
				
				if(GUIUtility.hotControl == dragHoleControlId &&
				   Event.current.button == 0)
				{
					GUIUtility.hotControl = 0;
					Event.current.Use();
					EditorGUIUtility.SetWantsMouseJumping(0);
					Repaint();
				}
				break;
				
			case EventType.MouseDrag:
				if(GUIUtility.hotControl == dragHoleControlId &&
				   Event.current.button == 0 &&
				   !Event.current.alt)
				{
					Vector2 delta = new Vector2(Event.current.delta.x,-Event.current.delta.y) / m_Zoom;
					
					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"move hole");
					
					mSpriteMeshCache.selectedHole.vertex += delta;
					
					mSpriteMeshCache.Triangulate();
					Repaint();
				}
				
				break;
			}
		}

		void HandleSelectBone()
		{
			switch (Event.current.type)
			{
			case EventType.MouseDown:
				if(GUIUtility.hotControl == 0 &&
				   Event.current.button == 0 &&
				   showBones &&
				   mHoveredEdge == null &&
				   mHoveredVertex == null &&
				   !Event.current.alt)
				{
					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"select bone");
					
					mSpriteMeshCache.selectedBone = mHoveredBone;

					if(mHoveredBone != null)
					{
						Event.current.Use();
					}
					
					Repaint();
				}
				
				break;
			}
		}

		void HandleSelection()
		{
			int controlId = GUIUtility.GetControlID("Selection".GetHashCode(), FocusType.Keyboard);

			switch (Event.current.GetTypeForControl(controlId))
			{
			case EventType.MouseDown:
				if(GUIUtility.hotControl == 0 &&
				   Event.current.button == 0 &&
				   !Event.current.alt)
				{
					if(mHoveredVertex == null && mHoveredHole == null && mHoveredEdge == null && mHoveredBone == null)
					{
						Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"unselect vertices");

						GUIUtility.hotControl = controlId;
						mSelectionRect.position = GuiToTexture(mMouseDownPosition);
						mSelectionRect.size = Vector2.zero;

						mSelectionToolVertices.Clear();

						mSpriteMeshCache.selectedHole = null;
						mSpriteMeshCache.selectedEdge = null;

						if(!EditorGUI.actionKey)
						{
							mSpriteMeshCache.UnselectVertices();
						}

						Event.current.Use();
						EditorGUIUtility.SetWantsMouseJumping(1);
					}
				}
				break;
			case EventType.MouseUp:
				if(GUIUtility.hotControl == controlId && Event.current.button == 0)
				{
					Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"select vertices");

					for (int i = 0; i < mSelectionToolVertices.Count; i++)
					{
						Vertex vertex = mSelectionToolVertices[i];
						mSpriteMeshCache.SelectVertex(vertex, true);
					}

					mSelectionToolVertices.Clear();

					GUIUtility.hotControl = 0;
					Event.current.Use();
					EditorGUIUtility.SetWantsMouseJumping(0);
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlId &&
				    Event.current.button == 0)
				{
						float left = Mathf.Min(mMousePositionTextureSpace.x, mMouseDownPositionTextureSpace.x);
						float top = Mathf.Min(mMousePositionTextureSpace.y, mMouseDownPositionTextureSpace.y);
						float width = Mathf.Abs((mMousePositionTextureSpace - mMouseDownPositionTextureSpace).x);
						float height = Mathf.Abs((mMousePositionTextureSpace - mMouseDownPositionTextureSpace).y);

						mSelectionRect = new Rect(left,top,width,height);

						UpdateSelectionVertices();

						Event.current.Use();
				}
				break;
			case EventType.Repaint:
				if(GUIUtility.hotControl == controlId)
				{
					List<Vector3> verts = new List<Vector3>(4);
					verts.Add(new Vector3(mSelectionRect.position.x,mSelectionRect.position.y,0f));
					verts.Add(new Vector3(mSelectionRect.position.x,mSelectionRect.position.y + mSelectionRect.height,0f));
					verts.Add(new Vector3(mSelectionRect.position.x + mSelectionRect.width,mSelectionRect.position.y + mSelectionRect.height,0f));
					verts.Add(new Vector3(mSelectionRect.position.x + mSelectionRect.width,mSelectionRect.position.y,0f));
					Handles.color = Color.cyan;
					Handles.DrawSolidRectangleWithOutline(verts.ToArray(),new Color(1f,1f,1f,0.1f),new Color(1f,1f,1f,0.8f));
				}
				break;
			}
		}

		void UpdateSelectionVertices()
		{
			mSelectionToolVertices.Clear();

			for (int i = 0; i < mSpriteMeshCache.texVertices.Count; i++)
			{
				Vertex vertex = mSpriteMeshCache.texVertices[i];
				if(mSelectionRect.Contains(vertex.vertex))
				{
					mSelectionToolVertices.Add (vertex);
				}
			}
		}

		override protected void HandleEvents()
		{
			SetupSpriteMatrix();
			SetupSpriteMeshMatrix();
		}

		override protected void DoTextureGUIExtras()
		{
			UpdateMousePosition();
			UpdateHoveredVertex();
			UpdateHoveredHole();
			UpdateHoveredEdge();
			UpdateBones();

			if(Event.current.type == EventType.Repaint)
			{

				if(!isBound || !weightEditor.showPie)
				{
					DrawVertices();
				}
				
				DrawSelectedAndHoveredEdges();
				
				float pointSize = 2.5f / m_Zoom;
				
				if(isBound && weightEditor.showPie)
				{
					DrawPies(4 * pointSize);
				}
				
				DrawHoveredVertex();

				DrawHoles();
				DrawSelectedAndHoveredHoles();

				DrawSelectionList(mSpriteMeshCache.selectedVertices);
				DrawSelectionList(mSelectionToolVertices);
				
			}

			if(!(showWeightEditor && weightEditor.IsHovered()))
			{
				HandleAddVertex();
				HandleAddHole();
				HandleDeleteVertex();
				HandleDeleteHole();
				HandleDeleteEdge();

				if(!addVertices)
				{
					HandleAddEdge();
				    HandleSelectBone();
					HandleSelection();
				}
				
				HandleDragVertex();
				HandleDragHole();
				HandleDragEdge();
			}
		}

		override protected void DrawGizmos()
		{
			DrawSpriteMesh();

			if(showTriangles)
			{
				DrawTriangles();
			}

			if(showBones)
			{
				DrawBones();
			}

			DrawEdges();
		}

		void UpdateMousePosition()
		{
			mMousePosition = Event.current.mousePosition - new Vector2(2f,3f);
			mMousePositionTextureSpace = GuiToTexture(mMousePosition);

			if(Event.current.type == EventType.MouseDown)
			{
				if(Event.current.button == 0)
				{
					mMouseDownPosition = mMousePosition;
					mMouseDownPositionTextureSpace = mMousePositionTextureSpace;
				}
			}
		}

		void UpdateHoveredVertex()
		{
			Vertex vertex = GetClosestVertex(mMousePosition, 10f);
			if(mHoveredVertex != vertex)
			{
				mHoveredVertex = vertex;
				Repaint();
			}
		}

		void UpdateHoveredEdge()
		{
			Edge oldHovered = mHoveredEdge;

			mClosestEdge = GetClosestEdge(mMousePositionTextureSpace);
			mHoveredEdge = mClosestEdge;
			
			if(mClosestEdge != null)
			{
				float distance = 10f / m_Zoom;
				if(MathUtils.SegmentSqrtDistance(mMousePositionTextureSpace,
				                                 mClosestEdge.vertex1.vertex,
				                                 mClosestEdge.vertex2.vertex) > distance * distance)
				{
					mHoveredEdge = null;
				}
			}

			if(mHoveredEdge != oldHovered)
			{
				Repaint();
			}
		}

		void UpdateHoveredHole()
		{
			Hole hole = GetClosestHole(mMousePosition, 10f);
			if(mHoveredHole != hole)
			{
				mHoveredHole = hole;
				Repaint();
			}
		}

		void UpdateBones()
		{
			if(isBound && showBones)
			{
				BindInfo oldHovered = mHoveredBone;

				Vector2 mMousePositionMeshSpace = SpriteMeshUtils.TexCoordToVertex(spriteMesh,mMousePositionTextureSpace);
				mHoveredBone = GetClosestBone(mMousePositionMeshSpace);
				
				if(mHoveredBone != null)
				{
					float distance = 0.15f / m_Zoom;
					if(MathUtils.SegmentSqrtDistance(mMousePositionMeshSpace,
					                                 mHoveredBone.position,
					                                 mHoveredBone.endPoint) > distance * distance)
					{
						mHoveredBone = null;
					}
				}
				
				if(mHoveredBone != oldHovered)
				{
					Repaint();
				}
			}else{
				mHoveredBone = null;
			}
		}

		Vector2 GuiToTexture(Vector2 position)
		{
			Vector2 texturePosition = (position - (m_TextureRect.position - m_ScrollPosition)) / m_Zoom;
			texturePosition.y = m_Texture.height - texturePosition.y;
		
			return texturePosition;
		}

		Vector2 TextureToGUI(Vector2 position)
		{
			position.y = m_Texture.height - position.y;
			Vector2 guiPosition = position * m_Zoom + m_TextureRect.position - m_ScrollPosition;

			return guiPosition;
		}

		Vertex GetClosestVertex(Vector2 mousePosition, float minDistance)
		{
			minDistance /= m_Zoom;
			float sqrMinDistance = minDistance*minDistance;
			Vertex vertex = null;
			Vector2 position = GuiToTexture(mousePosition);
			float sqrMagnitude = float.MaxValue;

			for (int i = 0; i < mSpriteMeshCache.texVertices.Count; i++)
			{
				Vertex v = mSpriteMeshCache.texVertices[i];

				if(v != null)
				{
					float l_sqrMagnitude = (v.vertex - position).sqrMagnitude;
					if (l_sqrMagnitude < sqrMagnitude && l_sqrMagnitude < sqrMinDistance)
					{
						sqrMagnitude = l_sqrMagnitude;
						vertex = v;
					}
				}
			}

			return vertex;
		}

		Hole GetClosestHole(Vector2 mousePosition, float minDistance)
		{
			minDistance /= m_Zoom;
			float sqrMinDistance = minDistance*minDistance;
			Hole hole = null;
			Vector2 position = GuiToTexture(mousePosition);
			float sqrMagnitude = float.MaxValue;
			
			for (int i = 0; i < mSpriteMeshCache.holes.Count; i++)
			{
				Hole h = mSpriteMeshCache.holes[i];
				
				if(h != null)
				{
					float l_sqrMagnitude = (h.vertex - position).sqrMagnitude;
					if (l_sqrMagnitude < sqrMagnitude && l_sqrMagnitude < sqrMinDistance)
					{
						sqrMagnitude = l_sqrMagnitude;
						hole = h;
					}
				}
			}
			
			return hole;
		}

		Edge GetClosestEdge(Vector2 mousePosition)
		{
			float minSqrtDistance = float.MaxValue;
			Edge closestEdge = null;
			
			for(int i = 0; i < mSpriteMeshCache.edges.Count; ++i)
			{
				Edge edge = mSpriteMeshCache.edges[i];
				if(edge != null && edge.vertex1 != null && edge.vertex2 != null)
				{
					float sqrtDistance = MathUtils.SegmentSqrtDistance((Vector3)mousePosition,
					                                                   edge.vertex1.vertex,
					                                                   edge.vertex2.vertex);
					if(sqrtDistance < minSqrtDistance)
					{
						closestEdge = edge;
						minSqrtDistance = sqrtDistance;
					}
				}
			}

			return closestEdge;
		}

		BindInfo GetClosestBone(Vector2 mousePosition)
		{
			float minSqrtDistance = float.MaxValue;
			BindInfo closestBindInfo = null;
			
			for(int i = 0; i < mSpriteMeshCache.bindPoses.Count; ++i)
			{
				BindInfo bindInfo = mSpriteMeshCache.bindPoses[i];
				if(bindInfo != null)
				{
					float sqrtDistance = MathUtils.SegmentSqrtDistance((Vector3)mousePosition,
					                                                   bindInfo.position,
					                                                   bindInfo.endPoint);
					if(sqrtDistance < minSqrtDistance)
					{
						closestBindInfo = bindInfo;
						minSqrtDistance = sqrtDistance;
					}
				}
			}

			return closestBindInfo;
		}

		void DoApply()
		{
			if(spriteMesh && mSpriteMeshCache)
			{
				mSpriteMeshCache.UpdateIndexedEdges();

				spriteMesh.texVertices = mSpriteMeshCache.texVertices.ConvertAll( texVertex => texVertex.Clone() as Vertex );
				spriteMesh.edges = new List<IndexedEdge>(mSpriteMeshCache.indexedEdges);
				spriteMesh.holes = new List<Hole>(mSpriteMeshCache.holes.ToArray()); 
				spriteMesh.indices = new List<int>(mSpriteMeshCache.indices.ToArray());
				spriteMesh.bindPoses = mSpriteMeshCache.bindPoses.ConvertAll( bindPose => bindPose.Clone() as BindInfo );
				mSpriteMeshCache.isDirty = false;

				EditorUtility.SetDirty(spriteMesh);

				SpriteMeshUtils.UpdateAssets(spriteMesh);

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		void InvalidateCache()
		{
			if(mSpriteMeshCache)
			{
				DestroyImmediate(mSpriteMeshCache);
			}

			mSpriteMeshCache = ScriptableObject.CreateInstance<SpriteMeshCache>();
			mSpriteMeshCache.hideFlags = HideFlags.HideAndDontSave;
		}

		void DoRevert()
		{
			mClosestEdge = null;
			mHoveredEdge = null;
			mHoveredVertex = null;

			if(mSpriteMeshCache)
			{
				mSpriteMeshCache.Clear();
			
				if(spriteMesh)
				{
					mSpriteMeshCache.texVertices = spriteMesh.texVertices.ConvertAll( texVetex => texVetex.Clone() as Vertex);
					mSpriteMeshCache.indices = new List<int>(spriteMesh.indices);
					mSpriteMeshCache.indexedEdges = new List<IndexedEdge>(spriteMesh.edges);
					mSpriteMeshCache.holes = new List<Hole>(spriteMesh.holes);
					mSpriteMeshCache.bindPoses = spriteMesh.bindPoses.ConvertAll( bindPose => bindPose.Clone() as BindInfo);
					mSpriteMeshCache.spriteMesh = spriteMesh;
					mSpriteMeshCache.spriteMeshInstance = spriteMeshInstance;
				}
			}
		}

		void SetFromSprite()
		{
			if(mSpriteMeshCache)
			{
				List<IndexedEdge> indexedEdges = new List<IndexedEdge>();
				
				mSpriteMeshCache.Clear();

				SpriteMeshUtils.GetInfoFromSprite(spriteMesh.sprite,mSpriteMeshCache.texVertices,indexedEdges,mSpriteMeshCache.indices);
				mSpriteMeshCache.indexedEdges = indexedEdges;
			}

		}

		void SetFromRect()
		{
			mClosestEdge = null;
			mHoveredEdge = null;
			mHoveredVertex = null;
			
			if(mSpriteMeshCache)
			{
				mSpriteMeshCache.Clear();
				
				if(spriteMesh)
				{
					List<Vector2> texCoords = new List<Vector2>();

					Vector2 rectMin = spriteMesh.sprite.rect.min;
					Vector2 rectMax = spriteMesh.sprite.rect.max;

					texCoords.Add(new Vector2(rectMin.x,rectMin.y));
					texCoords.Add(new Vector2(rectMin.x,rectMax.y));
					texCoords.Add(new Vector2(rectMax.x,rectMax.y));
					texCoords.Add(new Vector2(rectMax.x,rectMin.y));

					for (int i = 0; i < texCoords.Count; i++)
					{
						mSpriteMeshCache.texVertices.Add(new Vertex(texCoords[i]));
					}

					List<IndexedEdge> edges = new List<IndexedEdge>(4);

					edges.Add(new IndexedEdge(0,1));
					edges.Add(new IndexedEdge(1,2));
					edges.Add(new IndexedEdge(2,3));
					edges.Add(new IndexedEdge(3,0));

					mSpriteMeshCache.indexedEdges = edges;

					mSpriteMeshCache.Triangulate();
				}
			}
		}

		override protected void DoToolbarGUI()
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUI.BeginDisabledGroup(spriteMesh == null);

			if (GUILayout.Button(s_Styles.spriteIcon, EditorStyles.toolbarButton, GUILayout.Width(25f) ))
			{
				Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"Reset");

				if(mSetFromRect)
				{
					SetFromRect();
				}else{
					SetFromSprite();
				}

				mSetFromRect = !mSetFromRect;
			}
			
			showBones = GUILayout.Toggle(showBones, s_Styles.showBonesIcon, EditorStyles.toolbarButton,GUILayout.Width(32));
			showTriangles = true;

			//snapPixel = GUILayout.Toggle(snapPixel, new GUIContent("Snap to pixel", "Snap to pixel"), EditorStyles.toolbarButton);

			EditorGUI.BeginChangeCheck();

			addVertices = GUILayout.Toggle(addVertices, new GUIContent("Add vertices", "Add vertices"), EditorStyles.toolbarButton);

			if(EditorGUI.EndChangeCheck())
			{
				addHoles = false;
			}

			EditorGUI.BeginChangeCheck();

			addHoles = GUILayout.Toggle(addHoles, new GUIContent("Add holes", "Add holes"), EditorStyles.toolbarButton);

			if(EditorGUI.EndChangeCheck())
			{
				addVertices = false;
			}

			GUILayout.FlexibleSpace();

			EditorGUI.BeginDisabledGroup(spriteMeshInstance == null);
			
			if (GUILayout.Toggle(isBound, new GUIContent("Bind bones", "Bind bones"), EditorStyles.toolbarButton))
			{
				if(!isBound)
				{
					BindBones();
				}
			}

			EditorGUI.EndDisabledGroup();

			if (GUILayout.Toggle(!isBound, new GUIContent("Clear weights", "Unind bones"), EditorStyles.toolbarButton))
			{
				if(isBound)
				{
					ClearWeights();
				}
			}

			if(GUILayout.Button(new GUIContent("Revert", "Revert changes"), EditorStyles.toolbarButton))
			{
				Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"revert");
				
				DoRevert();
			}
			
			if(GUILayout.Button(new GUIContent("Apply", "Apply changes"), EditorStyles.toolbarButton))
			{
				DoApply();
			}

			EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
		}

		void HotKeys()
		{
			if(GUIUtility.hotControl == 0 &&
			   Event.current.type == EventType.KeyDown)
			{
				if(Event.current.keyCode == KeyCode.V)
				{
					addVertices = true;
					addHoles = false;
					Event.current.Use();
					EditorGUIUtility.SetWantsMouseJumping(1);
					Repaint();
				}else if(Event.current.keyCode == KeyCode.H)
				{
					addVertices = false;
					addHoles = true;
					Event.current.Use();
					EditorGUIUtility.SetWantsMouseJumping(1);
					Repaint();
				}
			}
		}

		override protected void OnGUI()
		{
			Matrix4x4 matrix = Handles.matrix;

			textureColor = Color.gray;

			if (!mSpriteMeshCache || !spriteMesh)
			{
				EditorGUI.BeginDisabledGroup(true);
				GUILayout.Label("No SpriteMesh selected");
				EditorGUI.EndDisabledGroup();
			}else if(spriteMesh && !spriteMesh.sprite)
			{
				EditorGUI.BeginDisabledGroup(true);
				GUILayout.Label("SpriteMesh has no sprite");
				EditorGUI.EndDisabledGroup();
			}else{
				autoRepaintOnSceneChange = spriteMeshInstance && showBones && !isBound;
				wantsMouseMove = true;
				antiAlias = 2;

				HotKeys();

				base.OnGUI();

				GUI.color = Color.white;

				GUILayout.BeginArea(m_TextureViewRect);

				if(showWeightEditor)
				{
					weightEditor.spriteMeshCache = mSpriteMeshCache;
					weightEditor.OnGUI(this,m_TextureViewRect);
				}

				GUILayout.EndArea();
			}

			Handles.matrix = matrix;
		}

		void SetupSpriteMatrix()
		{
			Rect textureRect = m_TextureRect;
			textureRect.position -= m_ScrollPosition;

			mSpriteMatrix.SetTRS(
				new Vector3(textureRect.x + spriteMesh.sprite.rect.x * m_Zoom,
			            textureRect.y + (spriteMesh.sprite.texture.height - spriteMesh.sprite.rect.y) * m_Zoom,
			            0) + Vector3.Scale(spritePivotPoint, new Vector3(1f,-1f,1f)) * m_Zoom,
				Quaternion.Euler(0f, 0f, 0f),
				new Vector3(1f, -1f, 1f) * m_Zoom * spriteMesh.sprite.pixelsPerUnit);
		}

		void SetupSpriteMeshMatrix()
		{
			Rect textureRect = m_TextureRect;
			textureRect.position -= m_ScrollPosition;

			Vector3 invertY = new Vector3(1f, -1f, 1f);

			mSpriteMeshMatrix.SetTRS(
				new Vector3(textureRect.x,
			            textureRect.y + textureRect.height,
			            0) + Vector3.Scale((Vector3)SpriteMeshUtils.GetPivotPoint(spriteMesh),invertY) * m_Zoom,
				Quaternion.Euler(0f, 0f, 0f),
				invertY * m_Zoom * spriteMesh.sprite.pixelsPerUnit);
		}

		void DrawMesh(Vector3[] vertices, Vector2[] uvs, Color[] colors, int[] triangles, Material material)
		{
			Mesh mesh = new Mesh();
			
			mesh.vertices = vertices;
			mesh.uv = uvs;
			mesh.colors = colors;
			mesh.triangles = triangles;

			material.SetPass(0);
			
			Graphics.DrawMeshNow(mesh, Handles.matrix * GUI.matrix);
			
			DestroyImmediate(mesh);
		}

		protected void DrawSprite()
		{
			Vector3[] vertices = spriteVertices;
			Vector2[] uvs = spriteMesh.sprite.uv;
			Color[] colors = new Color[vertices.Length];
			ushort[] indicesShort = spriteMesh.sprite.triangles;

			for(int i = 0; i < colors.Length; ++i)
			{
				colors[i] = Color.gray;
			}

			int[] indicesInt = new int[indicesShort.Length];
			
			for(int i = 0; i < indicesShort.Length; ++i)
			{
				indicesInt[i] = (int) indicesShort[i];
			}

			Matrix4x4 old = Handles.matrix;

			Handles.matrix = mSpriteMatrix;
			meshGuiMaterial.mainTexture = spriteMesh.sprite.texture;
			DrawMesh(vertices,uvs,colors,indicesInt,meshGuiMaterial);

			Handles.matrix = old;
		}
		
		protected void DrawSpriteMesh()
		{
			if(mSpriteMeshCache)
			{
				Texture mainTexture = spriteMesh.sprite.texture;

				List<Color> colors = new List<Color>();

				if(isBound && weightEditor.overlayColors)
				{
					foreach(Vertex vertex in mSpriteMeshCache.texVertices)
					{
						Color vertexColor =
							ColorRing.GetColor(vertex.boneWeight.boneIndex0)*vertex.boneWeight.weight0 +
							ColorRing.GetColor(vertex.boneWeight.boneIndex1)*vertex.boneWeight.weight1 +
							ColorRing.GetColor(vertex.boneWeight.boneIndex2)*vertex.boneWeight.weight2 +
							ColorRing.GetColor(vertex.boneWeight.boneIndex3)*vertex.boneWeight.weight3;
						
						colors.Add(vertexColor);
					}

					mainTexture = null;
				}

				List<Vector3> vertices = new List<Vector3>(mSpriteMeshCache.texVertices.Count);
				List<Vector2> uvs = new List<Vector2>(mSpriteMeshCache.texVertices.Count);

				for (int i = 0; i < mSpriteMeshCache.texVertices.Count; i++)
				{
					Vector2 vertex = mSpriteMeshCache.texVertices[i].vertex;
					vertices.Add((Vector3)vertex);
					uvs.Add(new Vector2(vertex.x / spriteMesh.sprite.texture.width,
					                    vertex.y / spriteMesh.sprite.texture.height));
				}




				meshGuiMaterial.mainTexture = mainTexture;
				DrawMesh(vertices.ToArray(),
				         uvs.ToArray(),
				         colors.ToArray(),
				         mSpriteMeshCache.indices.ToArray(),
				         meshGuiMaterial);
			}
		}


		void DrawTriangles()
		{
			Handles.color = Color.white * new Color (1f, 1f, 1f, 0.25f);

			for (int i = 0; i < mSpriteMeshCache.indices.Count; i+=3)
			{
				int index = mSpriteMeshCache.indices[i];
				int index1 = mSpriteMeshCache.indices[i+1];
				int index2 = mSpriteMeshCache.indices[i+2];
				Vector3 v1 = mSpriteMeshCache.texVertices[index].vertex;
				Vector3 v2 = mSpriteMeshCache.texVertices[index1].vertex;
				Vector3 v3 = mSpriteMeshCache.texVertices[index2].vertex;
				
				Handles.DrawLine(v1,v2);
				Handles.DrawLine(v2,v3);
				Handles.DrawLine(v1,v3);
			}

		}

		void DrawPie(Vector3 position, BoneWeight2 boneWeight, float pieSize)
		{
			Handles.color = Color.black;
			HandlesExtra.DrawCircle(position,pieSize * 1.12f);

			float angleStart = 0f;
			float angle = Mathf.Lerp(0f,360f,boneWeight.weight0);
			Handles.color = ColorRing.GetColor(boneWeight.boneIndex0);
			Handles.DrawSolidArc(position, Vector3.forward,Vector3.up,angle, pieSize);
			
			angleStart += angle;
			angle = Mathf.Lerp(0f,360f,boneWeight.weight1);
			Handles.color = ColorRing.GetColor(boneWeight.boneIndex1);
			Handles.DrawSolidArc(position, Vector3.forward,Quaternion.AngleAxis(angleStart,Vector3.forward) * Vector3.up,angle, pieSize);
			
			angleStart += angle;
			angle = Mathf.Lerp(0f,360f,boneWeight.weight2);
			Handles.color = ColorRing.GetColor(boneWeight.boneIndex2);
			Handles.DrawSolidArc(position, Vector3.forward,Quaternion.AngleAxis(angleStart,Vector3.forward) * Vector3.up,angle, pieSize);
			
			angleStart += angle;
			angle = Mathf.Lerp(0f,360f,boneWeight.weight3);
			Handles.color = ColorRing.GetColor(boneWeight.boneIndex3);
			Handles.DrawSolidArc(position, Vector3.forward,Quaternion.AngleAxis(angleStart,Vector3.forward) * Vector3.up,angle, pieSize);
		}

		void DrawPies(float pieSize)
		{
			for (int i = 0; i < mSpriteMeshCache.texVertices.Count; i++)
			{
				BoneWeight2 boneWeigth = mSpriteMeshCache.texVertices[i].boneWeight;
				Vector3 v = mSpriteMeshCache.texVertices[i].vertex;
				
				DrawPie(v,boneWeigth,pieSize);
			}
		}

		void DrawEdge(Edge edge, float width, Color color)
		{
			DrawEdge(edge,width,color,0f);
		}

		void DrawEdge(Edge edge, float width, Color color, float vertexSize)
		{
			if(edge.vertex1 != null && edge.vertex2 != null)
			{
				DrawEdge(edge.vertex1.vertex,
				         edge.vertex2.vertex,
				         width,
				         color);

				if(vertexSize > 0f)
				{
					DrawVertex(edge.vertex1.vertex,color,mPointSize);
					DrawVertex(edge.vertex2.vertex,color,mPointSize);
				}
			}
		}

		void DrawEdge(Vector2 p1, Vector2 p2, float width, Color color)
		{
			Handles.color = color;
			
			HandlesExtra.DrawLine(p1, p2, Vector3.forward, width);
		}

		void DrawEdges()
		{
			if(mSpriteMeshCache)
			{
				for (int i = 0; i < mSpriteMeshCache.edges.Count; i++)
				{
					DrawEdge(mSpriteMeshCache.edges[i], 1.5f/ m_Zoom, Color.cyan * new Color (1f, 1f, 1f, 0.75f));
				}
			}
		}

		void DrawVertex(Vector2 position, Color color, Color colorArc, float size)
		{
			Vector2 vSize = Vector2.one * size;
			
			Rect rect = new Rect();
			rect.position = TextureToGUI(position) - vSize * 0.5f;
			rect.size = vSize;
			
			GUI.color = color;
			GUI.DrawTexture(rect,dotTexture);
		}

		void DrawVertex(Vector2 position, Color color, float size)
		{
			DrawVertex(position, color, color, size);
		}

		void DrawVertex(Vector2 position)
		{
			DrawVertex(position,Color.cyan,Color.cyan * Color.gray, mPointSize);
		}

		void DrawHole(Vector2 position)
		{
			DrawVertex(position,Color.red,Color.red * Color.gray, mPointSize);
		}

		void DrawSelectedVertex(Vector2 position)
		{
			DrawVertex(position,Color.yellow, mPointSize);
		}

		void DrawVertices()
		{
			for (int i = 0; i < mSpriteMeshCache.texVertices.Count; i++)
			{
				DrawVertex(mSpriteMeshCache.texVertices[i].vertex);
			}
		}

		void DrawHoles()
		{
			for (int i = 0; i < mSpriteMeshCache.holes.Count; i++)
			{
				DrawHole(mSpriteMeshCache.holes[i].vertex);
			}
		}

		void DrawSplitEdge(Edge edge, Vector2 vertexPosition)
		{
			if(edge != null)
			{
				Vector3 p1 = edge.vertex1.vertex;
				Vector3 p2 = edge.vertex2.vertex;
				DrawEdge(edge, 2f/m_Zoom, Color.white, 2f / m_Zoom);
				DrawEdge(p1,vertexPosition, 2f/m_Zoom, Color.yellow);
				DrawEdge(p2,vertexPosition, 2f/m_Zoom, Color.yellow);
				DrawSelectedVertex(p1);
				DrawSelectedVertex(p2);
			}
			
			DrawVertex(vertexPosition);
		}

		void DrawHoveredVertex()
		{
			if(Event.current.type == EventType.Repaint)
			{
				if(GUIUtility.hotControl == 0 &&
				   mHoveredVertex != null &&
				   mSpriteMeshCache.texVertices.Count > 0)
				{
					DrawVertexSelection(mHoveredVertex.vertex);
				}
			}
		}

		void DrawSelectedAndHoveredHoles()
		{
			if(Event.current.type == EventType.Repaint)
			{
				if(GUIUtility.hotControl == 0 &&
				   mHoveredVertex == null &&
				   mHoveredHole != null &&
				   mSpriteMeshCache.holes.Count > 0)
				{
					DrawVertexSelection(mHoveredHole.vertex);
				}
				if(mSpriteMeshCache.selectedHole != null)
				{
					DrawVertexSelection(mSpriteMeshCache.selectedHole.vertex);
				}
			}
		}


		void DrawSelectedAndHoveredEdges()
		{
			Color selectedVertexColor = Color.yellow;
			float pointSize = 2.5f / m_Zoom;

			if(Event.current.type == EventType.Repaint)
			{
				if(GUIUtility.hotControl == 0 &&
				   mHoveredVertex == null &&
				   mHoveredHole == null &&
				   mHoveredEdge != null &&
				   mSpriteMeshCache.texVertices.Count > 0)
				{
					DrawEdge(mHoveredEdge, 2f / m_Zoom, selectedVertexColor, pointSize);
				}
				if(mSpriteMeshCache.selectedEdge != null)
				{
					DrawEdge(mSpriteMeshCache.selectedEdge,2f / m_Zoom, selectedVertexColor,pointSize);
				}
			}
		}

		void DrawVertexSelection(Vector3 position)
		{
			float pointSize = 2.5f / m_Zoom;
			Handles.color = Color.yellow;
			if (isBound && weightEditor.showPie)
			{
				HandlesExtra.DrawCircle(position, 4.5f * pointSize, 0.8f);
			}else{
				DrawSelectedVertex(position);
			}
		}

		void DrawSelectionList(List<Vertex> selectionList)
		{
			for (int i = 0; i < selectionList.Count; i++)
			{
				Vertex vertex = selectionList[i];
				DrawVertexSelection(vertex.vertex);
			}
		}

		void DrawBones()
		{
			Matrix4x4 old = Handles.matrix;

			Handles.matrix = mSpriteMeshMatrix;

			float radius = 7.5f / spriteMesh.sprite.pixelsPerUnit / m_Zoom;

			if(!isBound)
			{
				if(spriteMeshInstance)
				{
					foreach(Bone2D bone in spriteMeshInstance.bones)
					{
						if(bone)
						{
							Vector3 position = spriteMeshInstance.transform.InverseTransformPoint(bone.transform.position);
							Vector3 endPoint = spriteMeshInstance.transform.InverseTransformPoint(bone.endPosition);
							BoneUtils.DrawBoneBody(position,endPoint,radius,bone.color);
						}
					}

					foreach(Bone2D bone in spriteMeshInstance.bones)
					{
						if(bone)
						{
							Vector3 position = spriteMeshInstance.transform.InverseTransformPoint(bone.transform.position);
							BoneUtils.DrawBoneCap(position,radius,bone.color);
						}
					}
				}
			}else{

				if(weightEditor.overlayColors)
				{
					for(int i = 0; i < mSpriteMeshCache.bindPoses.Count; i++)
					{
						BindInfo bindInfo = mSpriteMeshCache.bindPoses[i];

						if(bindInfo != mSpriteMeshCache.selectedBone)
						{
							Color c = ColorRing.GetColor(i) * 0.5f;
							c.a = 1f;

							BoneUtils.DrawBoneOutline(bindInfo.position,bindInfo.endPoint,radius,radius*0.25f,c);
						}
					}
				}


				if(mHoveredBone != null &&
				   mHoveredEdge == null &&
				   mHoveredVertex == null &&
				   GUIUtility.hotControl == 0)
				{
					BoneUtils.DrawBoneOutline(mHoveredBone.position,mHoveredBone.endPoint,radius,radius*0.25f,Color.white);
				}

				if(mSpriteMeshCache.selectedBone != null)
				{
					BoneUtils.DrawBoneOutline(mSpriteMeshCache.selectedBone.position,mSpriteMeshCache.selectedBone.endPoint,radius,radius*0.25f,Color.white);
				}

				for(int i = 0; i < mSpriteMeshCache.bindPoses.Count; i++)
				{
					BindInfo bindInfo = mSpriteMeshCache.bindPoses[i];

					BoneUtils.DrawBoneBody(bindInfo.position, bindInfo.endPoint, radius, ColorRing.GetColor(i));
				}

				for(int i = 0; i < mSpriteMeshCache.bindPoses.Count; i++)
				{
					Color innerColor = ColorRing.GetColor(i) * 0.25f;
					innerColor.a = 1f;

					BindInfo bindInfo = mSpriteMeshCache.bindPoses[i];

					BoneUtils.DrawBoneCap (bindInfo.position, radius,ColorRing.GetColor(i),innerColor);
				}

			}

			Handles.matrix = old;
		}

		void ClearWeights()
		{
			Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"Clear weights");
			mSpriteMeshCache.ClearWeights();
		}

		void BindBones()
		{
			if(spriteMeshInstance)
			{
				Undo.RegisterCompleteObjectUndo(mSpriteMeshCache,"Bind bones");
				mSpriteMeshCache.BindBones();
				mSpriteMeshCache.CalculateAutomaticWeights();
			}
		}
		
		public void UpdateFromSelection()
		{
			SpriteMesh l_spriteMesh = null;
			SpriteMeshInstance l_spriteMeshInstance = null;
			
			if(Selection.activeGameObject)
			{
				l_spriteMeshInstance = Selection.activeGameObject.GetComponent<SpriteMeshInstance>();
			}
			
			if(l_spriteMeshInstance)
			{
				l_spriteMesh = l_spriteMeshInstance.spriteMesh;

			}else{
				if (Selection.activeObject is SpriteMesh)
				{
					l_spriteMesh = Selection.activeObject as SpriteMesh;
				}else if(Selection.activeGameObject)
				{
					GameObject activeGameObject = Selection.activeGameObject;
					GameObject prefab = null;
					
					if(PrefabUtility.GetPrefabType(activeGameObject) == PrefabType.Prefab)
					{
						prefab = Selection.activeGameObject;
					}else if(PrefabUtility.GetPrefabType(activeGameObject) == PrefabType.PrefabInstance ||
					         PrefabUtility.GetPrefabType(activeGameObject) == PrefabType.DisconnectedPrefabInstance)
					{
						prefab = PrefabUtility.GetPrefabParent(activeGameObject) as GameObject;
					}
					
					if(prefab)
					{
						l_spriteMesh = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(prefab), typeof(SpriteMesh)) as SpriteMesh;
					}
				}
			}

			if(l_spriteMeshInstance || l_spriteMesh)
			{
				spriteMeshInstance = l_spriteMeshInstance;
				
				if(mSpriteMeshCache)
				{
					mSpriteMeshCache.spriteMeshInstance = spriteMeshInstance;
				}
			}

			if(l_spriteMesh && l_spriteMesh != spriteMesh)
			{
				HandleApplyRevertDialog();
				
				spriteMesh = l_spriteMesh;
				
				InvalidateCache();
				
				if(spriteMesh && spriteMesh.sprite)
				{
					m_Texture = SpriteUtility.GetSpriteTexture(spriteMesh.sprite,false);
					DoRevert();
					m_Zoom = -1f;
				}
			}
		}

		void HandleApplyRevertDialog()
		{
			if(mSpriteMeshCache && mSpriteMeshCache.isDirty && spriteMesh)
			{
				if (EditorUtility.DisplayDialog("Unapplied changes", "Unapplied changes for '" + spriteMesh.name + "'", "Apply", "Revert"))
				{
					DoApply();
				}
				else
				{
					DoRevert();
				}
			}
		}

		private void OnSelectionChange()
		{
			UpdateFromSelection();
			Repaint();
		}

	}
}
