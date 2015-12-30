using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anima2D 
{
	public class WeightEditor
	{
		public SpriteMeshCache spriteMeshCache;

		public bool showPie { get; set; }
		public bool overlayColors { get; set; }

		float heightSingleSelection = 136f;
		float heightMultiSelection = 82f;
		public Rect windowRect = new Rect(0f, 0f, 250f, 136f);

		float mLastWeight = 0f;
		float mWeight = 0f;

		static Texture2D mStaticRectTexture;
		static GUIStyle mStaticRectStyle;

		public static void GUIDrawRect( Rect position, Color color )
		{
			if( mStaticRectTexture == null )
			{
				mStaticRectTexture = new Texture2D( 1, 1 );
				mStaticRectTexture.SetPixel( 0, 0, Color.white );
				mStaticRectTexture.Apply();
			}
			
			if( mStaticRectStyle == null )
			{
				mStaticRectStyle = new GUIStyle();
			}
			
			mStaticRectStyle.normal.background = mStaticRectTexture;

			Color backgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;

			GUI.Box( position, GUIContent.none, mStaticRectStyle );

			GUI.backgroundColor = backgroundColor;
		}

		public bool IsHovered()
		{
			return windowRect.Contains(Event.current.mousePosition);
		}

		public void OnGUI(EditorWindow editorWindow, Rect viewRect)
		{
			if(spriteMeshCache.selectedVertices.Count == 1)
			{
				windowRect.height = heightSingleSelection;
			}else{
				windowRect.height = heightMultiSelection;
			}

			windowRect.position = new Vector2(viewRect.width - windowRect.width - 5f,
			                                  viewRect.height - windowRect.height - 5f);

			editorWindow.BeginWindows();
			GUILayout.Window(1, windowRect, DoWindow, "Weight Editor");
			editorWindow.EndWindows();
		}

		string[] GetBonePaths()
		{
			List<string> paths = new List<string>(spriteMeshCache.bindPoses.Count);

			foreach(BindInfo bindInfo in spriteMeshCache.bindPoses)
			{
				paths.Add(bindInfo.path);
			}

			return paths.ToArray();
		}

		string[] GetBoneNames()
		{
			List<string> names = new List<string>(spriteMeshCache.bindPoses.Count);
			List<int> repetitions = new List<int>(spriteMeshCache.bindPoses.Count);
			
			foreach(BindInfo bindInfo in spriteMeshCache.bindPoses)
			{
				List<string> repetedNames = names.Where( s => s == bindInfo.name ).ToList();

				names.Add(bindInfo.name);
				repetitions.Add(repetedNames.Count);
			}

			for (int i = 0; i < names.Count; i++)
			{
				string name = names [i];
				int count = repetitions[i] + 1;
				if(count > 1)
				{
					name += " (" + count.ToString() + ")";
					names[i] = name;
				}
			}

			return names.ToArray();
		}

		void DoWindow(int windowId)
		{
			Vertex selectedVertex = spriteMeshCache.selectedVertex;

			string[] names = GetBoneNames();

			EditorGUIUtility.wideMode = true;
			float labelWidth = EditorGUIUtility.labelWidth;
			
			if(selectedVertex != null)
			{
				BoneWeight2 boneWeight = selectedVertex.boneWeight;

				int index0 = boneWeight.boneIndex0;
				int index1 = boneWeight.boneIndex1;
				int index2 = boneWeight.boneIndex2;
				int index3 = boneWeight.boneIndex3;

				float weight0 = boneWeight.weight0;
				float weight1 = boneWeight.weight1;
				float weight2 = boneWeight.weight2;
				float weight3 = boneWeight.weight3;

				EditorGUIUtility.labelWidth = 30f;

				EditorGUILayout.BeginHorizontal();

				EditorGUI.BeginChangeCheck();

				index0 = EditorGUILayout.Popup(index0,names,GUILayout.Width(100f));
				weight0 = EditorGUILayout.Slider(weight0,0f,1f);

				if(boneWeight != null && EditorGUI.EndChangeCheck())
				{
					Undo.RegisterCompleteObjectUndo(spriteMeshCache,"Modify Weights");
					boneWeight.SetWeight(0,index0,weight0);
				}

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				
				EditorGUI.BeginChangeCheck();

				index1 = EditorGUILayout.Popup(index1,names,GUILayout.Width(100f));
				weight1 = EditorGUILayout.Slider(weight1,0f,1f);

				if(boneWeight != null && EditorGUI.EndChangeCheck())
				{
					Undo.RegisterCompleteObjectUndo(spriteMeshCache,"Modify Weights");
					boneWeight.SetWeight(1,index1,weight1);
				}

				EditorGUILayout.EndHorizontal();


				EditorGUILayout.BeginHorizontal();

				EditorGUI.BeginChangeCheck();

				index2 = EditorGUILayout.Popup(index2,names,GUILayout.Width(100f));
				weight2 = EditorGUILayout.Slider(weight2,0f,1f);

				if(boneWeight != null && EditorGUI.EndChangeCheck())
				{
					Undo.RegisterCompleteObjectUndo(spriteMeshCache,"Modify Weights");
					boneWeight.SetWeight(2,index2,weight2);
				}

				EditorGUILayout.EndHorizontal();


				EditorGUILayout.BeginHorizontal();

				EditorGUI.BeginChangeCheck();

				index3 = EditorGUILayout.Popup(index3,names,GUILayout.Width(100f));
				weight3 = EditorGUILayout.Slider(weight3,0f,1f);

				if(boneWeight != null && EditorGUI.EndChangeCheck())
				{
					Undo.RegisterCompleteObjectUndo(spriteMeshCache,"Modify Weights");
					boneWeight.SetWeight(3,index3,weight3);
				}

				EditorGUILayout.EndHorizontal();

			}else{


				int index = spriteMeshCache.bindPoses.IndexOf(spriteMeshCache.selectedBone);

				EditorGUILayout.BeginHorizontal();

				EditorGUI.BeginChangeCheck();

				index = EditorGUILayout.Popup(index,names,GUILayout.Width(100f));

				if(index >= 0 && index < spriteMeshCache.bindPoses.Count)
				{
					spriteMeshCache.selectedBone = spriteMeshCache.bindPoses[index];
				}

				EditorGUI.BeginDisabledGroup(spriteMeshCache.selectedBone == null);

				if(Event.current.type == EventType.MouseUp ||
				   Event.current.type == EventType.MouseDown)
				{
					mLastWeight = 0f;
					mWeight = 0f;
				}

				mWeight = EditorGUILayout.Slider(mLastWeight,-1f,1f);

				if(EditorGUI.EndChangeCheck())
				{
					Undo.RegisterCompleteObjectUndo(spriteMeshCache,"Modify Weights");

					float delta = mWeight - mLastWeight;
					mLastWeight = mWeight;

					List<Vertex> vertices = spriteMeshCache.selectedVertices;

					if(vertices.Count == 0)
					{
						vertices = spriteMeshCache.texVertices;
					}

					foreach(Vertex vertex in vertices)
					{
						BoneWeight2 bw = vertex.boneWeight;
						bw.SetBoneIndexWeight(index,bw.GetBoneWeight(index) + delta);
					}
				}

				EditorGUILayout.EndHorizontal();

				EditorGUI.EndDisabledGroup();

			}

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Smooth"))
			{
				Undo.RegisterCompleteObjectUndo(spriteMeshCache,"Smooth Weights");

				List<Vertex> targetVertices = spriteMeshCache.texVertices;

				if(spriteMeshCache.selectedVertices.Count > 0)
				{
					targetVertices = spriteMeshCache.selectedVertices;
				}

				spriteMeshCache.SmoothVertices(targetVertices);
			}

			if(GUILayout.Button("Auto"))
			{
				Undo.RegisterCompleteObjectUndo(spriteMeshCache,"Calculate Weights");

				List<Vertex> targetVertices = spriteMeshCache.texVertices;
				
				if(spriteMeshCache.selectedVertices.Count > 0)
				{
					targetVertices = spriteMeshCache.selectedVertices;
				}

				spriteMeshCache.CalculateAutomaticWeights(targetVertices);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			EditorGUIUtility.labelWidth = 50f;
			
			overlayColors = EditorGUILayout.Toggle("Overlay", overlayColors);

			EditorGUIUtility.labelWidth = 30f;

			showPie = EditorGUILayout.Toggle("Pies", showPie);
			
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUIUtility.labelWidth = labelWidth;

			/*
			boneScrollListPosition = EditorGUILayout.BeginScrollView(boneScrollListPosition);

			boneList.DoLayoutList();

			EditorGUILayout.EndScrollView();
			*/

			EatMouseInput(new Rect(0,0,windowRect.width,windowRect.height));
		}

		void EatMouseInput(Rect rect)
		{
			int controlID = GUIUtility.GetControlID(new GUIContent("Weight tool"), FocusType.Native, windowRect);
			
			switch (Event.current.GetTypeForControl(controlID))
			{
			case EventType.MouseDown:
				if (rect.Contains(Event.current.mousePosition))
				{
					GUIUtility.hotControl = controlID;
					Event.current.Use();
				}
				break;
				
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID)
				{
					GUIUtility.hotControl = 0;
					Event.current.Use();
				}
				break;
				
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID)
					Event.current.Use();
				break;
				
			case EventType.ScrollWheel:
				if (rect.Contains(Event.current.mousePosition))
					Event.current.Use();

				break;
			}
		}
	}
}
