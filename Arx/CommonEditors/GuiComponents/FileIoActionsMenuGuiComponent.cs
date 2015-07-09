using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CommonEditors.GuiComponents.GuiComponents.GuiComponents
{
    public delegate void OnNew();
    public delegate void OnOpenQuest(string path);

    public class FileIoActionsMenuGuiComponent : BaseGuiComponent
    {
        private string _saveAsTitle;
        private string _saveAsMessage;
        private string _openTitle;
        private string _extension;
        
        public string Path { get; set; }
        public UnityEngine.Object Object { get; set; }

        public OnNew OnNew;
        public OnOpenQuest OnOpenQuest;

        public FileIoActionsMenuGuiComponent(
            string saveAsTitle,
            string saveAsMessage,
            string openTitle,
            string extension,
            UnityEngine.Object obj)
        {
            _saveAsTitle = saveAsTitle;
            _saveAsMessage = saveAsMessage;
            _openTitle = openTitle;
            _extension = extension;
            Object = obj;
        }

        public override void OnGui()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("New"))
            {
                Path = null;
                if (OnNew != null)
                {
                    OnNew();
                }
            }
            GUI.enabled = Path != null;
            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(Object);
                AssetDatabase.SaveAssets();
            }
            GUI.enabled = true;
            if (GUILayout.Button("Save as..."))
            {
                var path = EditorUtility.SaveFilePanelInProject(
                                _saveAsTitle,
                                null,
                                _extension,
                                _saveAsMessage);
                if (!string.IsNullOrEmpty(path))
                {
                    var dummy = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(Object.GetInstanceID()), Object.GetType());
                    
                    if (dummy == null)
                    {
                        AssetDatabase.CreateAsset(Object, path);
                    }
                    else
                    {
                        AssetDatabase.CopyAsset(Path, path);
                    }
                    Path = path;
                }
            }
            if (GUILayout.Button("Open..."))
            {
                var path = EditorUtility.OpenFilePanel(
                    _openTitle,
                    string.Empty,
                    _extension);
                
                if (!string.IsNullOrEmpty(path))
                {
                    path = path.Replace(Application.dataPath, "Assets");
                    Path = path;
                    if (OnOpenQuest != null)
                    {
                        OnOpenQuest(path);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
