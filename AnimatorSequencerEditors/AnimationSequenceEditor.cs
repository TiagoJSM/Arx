using AnimatorSequencerEditors.AnimationSequence;
using CommonEditors.Nodes;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using CommonEditors.Nodes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AnimatorSequencerEditors
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

        protected override void AdditionalSideWindowContent()
        {
            base.AdditionalSideWindowContent();

            if (GUILayout.Button(new GUIContent("Save as animation sequence", "Saves the Canvas as an animation sequence to be used in a scene")))
            {
                string path = EditorUtility.SaveFilePanelInProject("Save Animation Sequence", "Animation Sequence", "asset", "", NodeEditor.editorPath + "Resources/Saves/");
                if (!string.IsNullOrEmpty(path))
                {
                    SaveAnimationSequence(path);
                }
            }

            if (GUILayout.Button(new GUIContent("Load as animation sequence", "Saves the Canvas as an animation sequence to be used in a scene")))
            {
                string path = EditorUtility.OpenFilePanel("Load Node Canvas", NodeEditor.editorPath + "Resources/Saves/", "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    ScriptableObject[] objects = ResourceManager.LoadResources<ScriptableObject>(path);
                }
            }
        }

        private void SaveAnimationSequence(string path)
        {
            var data = AnimationSequenceBehaviourGenerator.Compile(mainNodeCanvas);
            AnimatorSequenceSaveManager.Save(path, data);
            Repaint();
        }
    }
}
