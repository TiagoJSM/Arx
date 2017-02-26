using Assets.Standard_Assets.Terrain.Editor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Terrain.Editor
{
    public class TerrainMaterialEditor : EditorWindow
    {
        private readonly string ShaderName = "2DTerrain/Lit";
        private readonly string InvalidMaterialSelectedMessage;

        private readonly string TextureParameterName = "_Texture";

        private Vector2 _textureEditorPosition;
        private Material _selectedMaterial;
        private int _toolbarSelected;
        private TerrainTextureSelection _selection = TerrainEditorUtils.GenerateTerrainTextureSelection(100, 100);

        public TerrainMaterialEditor()
        {
            InvalidMaterialSelectedMessage = 
                string.Format("Please select a material with shader {0}", ShaderName);
        }

        [MenuItem("Window/2D Terrain/Create Terrain Material")]
        static void Init()
        {
            var window = EditorWindow.GetWindow<TerrainMaterialEditor>("Terrain Mater Editor");
        }

        private void OnGUI()
        {
            if (!CheckShader())
            {
                GUILayout.Label(InvalidMaterialSelectedMessage);
                return;
            }
            
            EditorGUILayout.BeginVertical();

            LayerSelectionToolbar();
            Separator();
            TextureMaterialEditor();

            EditorGUILayout.EndVertical();
        }

        private bool CheckShader()
        {
            _selectedMaterial = Selection.activeObject as Material;
            if (_selectedMaterial == null)
            {
                return false;
            }
            var shader = _selectedMaterial.shader;
            if (shader != null && shader.name == ShaderName)
            {
                return true;
            }
            _selectedMaterial = null;
            return false;
        }

        private void TextureMaterialEditor()
        {
            var texture = _selectedMaterial.GetTexture(TextureParameterName);
            _textureEditorPosition = GUILayout.BeginScrollView(_textureEditorPosition);
            RenderTexture(texture);

            if(TerrainEditorUtils.TextureSelection(_selection, texture))
            {
                Repaint();
            }
            GUILayout.EndScrollView();
            
            Rect scale = GUILayoutUtility.GetLastRect();
        }

        private void RenderTexture(Texture texture)
        {
            GUILayout.BeginArea(new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), texture);
            GUILayout.EndArea();
        }

        private void Separator()
        {
            GUILayout.Box(
                EditorGUIUtility.whiteTexture,
                new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        }

        private void LayerSelectionToolbar()
        {
               _toolbarSelected = 
                GUILayout.Toolbar(
                    _toolbarSelected, 
                    new[] { "Floor", "Ceiling", "Slope" },
                    GUILayout.ExpandWidth(true));
        }
    }
}
