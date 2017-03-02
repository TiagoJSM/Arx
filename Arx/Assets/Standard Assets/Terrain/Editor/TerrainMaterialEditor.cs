﻿using Assets.Standard_Assets.Terrain.Editor.Utils;
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
        private const string ShaderName = "2DTerrain/Lit";
        private readonly string InvalidMaterialSelectedMessage;

        private const string TextureParameterName = "_Texture";

        private const string FloorCoordinates = "_Floor";
        private const string FloorLeftEndingCoordinates = "_FloorLeftEnding";
        private const string FloorRightEndingCoordinates = "_FloorRightEnding";

        private const string CeilingCoordinates = "_Ceiling";
        private const string CeilingLeftEndingCoordinates = "_CeilingLeftEnding";
        private const string CeilingRightEndingCoordinates = "_CeilingRightEnding";

        private const string SlopeCoordinates = "_Slope";
        private const string SlopeLeftEndingCoordinates = "_SlopeLeftEnding";
        private const string SlopeRightEndingCoordinates = "_SlopeRightEnding";

        private const int FloorIndex = 0;
        private const int CeilingIndex = 1;
        private const int SlopeIndex = 2;

        private Vector2 _textureEditorPosition;
        private Material _selectedMaterial;
        private int _toolbarSelected = 0;
        private TerrainTextureSelection _selection;

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
            var selectedMaterial = Selection.activeObject as Material;
            if (selectedMaterial == null)
            {
                _selectedMaterial = selectedMaterial;
                return false;
            }
            if(_selectedMaterial == null)
            {
                _selectedMaterial = selectedMaterial;
                var texture = _selectedMaterial.GetTexture(TextureParameterName);
                _selection = GetTerrainTextureSelectionForCurrentLayer(texture);
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
                AssignTextureCoordinatesForSelectedLayer(texture);
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
            var selectedToolbar = 
                GUILayout.Toolbar(
                    _toolbarSelected, 
                    new[] { "Floor", "Ceiling", "Slope" },
                    GUILayout.ExpandWidth(true));

            if(selectedToolbar != _toolbarSelected)
            {
                _toolbarSelected = selectedToolbar;
                var texture = _selectedMaterial.GetTexture(TextureParameterName);
                _selection = GetTerrainTextureSelectionForCurrentLayer(texture);
            }
        }

        private TerrainTextureSelection GetTerrainTextureSelectionForCurrentLayer(Texture texture)
        {
            switch (_toolbarSelected)
            {
                case FloorIndex:
                    return GetTerrainTextureSelection(texture, FloorLeftEndingCoordinates, FloorCoordinates, FloorRightEndingCoordinates);
                case CeilingIndex:
                    return GetTerrainTextureSelection(texture, CeilingLeftEndingCoordinates, CeilingCoordinates, CeilingRightEndingCoordinates);
                case SlopeIndex:
                    return GetTerrainTextureSelection(texture, SlopeLeftEndingCoordinates, SlopeCoordinates, SlopeRightEndingCoordinates);
            }
            return null;
        }

        private TerrainTextureSelection GetTerrainTextureSelection(
            Texture texture, string left, string center, string right)
        {
            var leftCoords = _selectedMaterial.GetVector(left);
            var centerCoords = _selectedMaterial.GetVector(center);
            var rightCoords = _selectedMaterial.GetVector(right);

            var texHeight = texture.height;
            var texWidth = texture.width;

            return new TerrainTextureSelection(
                new Vector2(leftCoords.x * (texWidth - 1), texHeight - leftCoords.w * (texHeight - 1)),
                new Vector2(rightCoords.z * (texWidth - 1), texHeight - rightCoords.y * (texHeight - 1)),
                centerCoords.x * (texWidth - 1),
                centerCoords.z * (texHeight - 1));
        }

        private void AssignTextureCoordinatesForSelectedLayer(Texture texture)
        {
            switch (_toolbarSelected)
            {
                case FloorIndex:
                    AssignTextureCoordinates(texture, FloorLeftEndingCoordinates, FloorCoordinates, FloorRightEndingCoordinates);
                    break;
                case CeilingIndex:
                    AssignTextureCoordinates(texture, CeilingLeftEndingCoordinates, CeilingCoordinates, CeilingRightEndingCoordinates);
                    break;
                case SlopeIndex:
                    AssignTextureCoordinates(texture, SlopeLeftEndingCoordinates, SlopeCoordinates, SlopeRightEndingCoordinates);
                    break;
            }
        }

        private void AssignTextureCoordinates(Texture texture, string left, string center, string right)
        {
            var leftCapLB = _selection.LeftCapLeftBottom;
            var leftCapRT = _selection.LeftCapRightTop;
            _selectedMaterial.SetVector(
                left,
                new Vector4(
                    leftCapLB.x / texture.width, 
                    (texture.height - 1 - leftCapLB.y) / texture.height, 
                    leftCapRT.x / texture.width, 
                    (texture.height - 1 - leftCapRT.y) / texture.height));

            var centerLB = _selection.CenterLeftBottom;
            var centerRT = _selection.CenterRightTop;
            _selectedMaterial.SetVector(
                center,
                new Vector4(
                    centerLB.x / texture.width, 
                    (texture.height - 1 - centerLB.y) / texture.height, 
                    centerRT.x / texture.width, 
                    (texture.height - 1 - centerRT.y) / texture.height));

            var rightCapLB = _selection.RightCapLeftBottom;
            var rightCapRT = _selection.RightCapRightTop;
            _selectedMaterial.SetVector(
                right,
                new Vector4(
                    rightCapLB.x / texture.width, 
                    (texture.height - 1 - rightCapLB.y) / texture.height, 
                    rightCapRT.x / texture.width, 
                    (texture.height - 1 - rightCapRT.y) / texture.height));
        }
    }
}