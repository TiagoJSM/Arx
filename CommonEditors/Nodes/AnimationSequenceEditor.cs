using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using CommonEditors.Nodes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CommonEditors.Nodes
{
    public class AnimationSequenceEditor : NodeEditorWindow
    {
        [MenuItem("Window/Animation Sequence Editor")]
        public static void CreateEditor()
        {
            var editor = GetWindow<AnimationSequenceEditor>();
            editor.minSize = new Vector2(800, 600);
            NodeEditor.ClientRepaints += editor.Repaint;
            NodeEditor.initiated = NodeEditor.InitiationError = false;

            editor.iconTexture = ResourceManager.LoadTexture(EditorGUIUtility.isProSkin ? "Textures/Icon_Dark.png" : "Textures/Icon_Light.png");
            editor.titleContent = new GUIContent("Node Editor", editor.iconTexture);
        }

        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool AutoOpenCanvas(int instanceID, int line)
        {
            if (Selection.activeObject != null && Selection.activeObject.GetType() == typeof(NodeCanvas))
            {
                string NodeCanvasPath = AssetDatabase.GetAssetPath(instanceID);
                CreateEditor();
                GetWindow<AnimationSequenceEditor>().LoadNodeCanvas(NodeCanvasPath);
                return true;
            }
            return false;
        }
    }
}
