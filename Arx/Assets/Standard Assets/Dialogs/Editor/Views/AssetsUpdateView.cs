using Assets.Standard_Assets.Dialogs.Editor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Dialogs.Editor.Views
{
    public static class AssetsUpdateView
    {
        private static Vector2 _scrollPosition;
        private static int _selectedItem = -1;
        private static List<LocalizedTexts> _assets;

        public static void OnGui(IDialogEditorContext context)
        {
            RenderDialogAssetsList(context);
            RenderButtons(context);
        }

        private static void RenderDialogAssetsList(IDialogEditorContext context)
        {
            _assets = FindAssetsByType<LocalizedTexts>();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            _selectedItem = GUILayout.SelectionGrid(
                _selectedItem,
                _assets.Select(a => new GUIContent(AssetDatabase.GetAssetPath(a))).ToArray(),
                1,
                GUILayout.MaxWidth(500),
                GUILayout.ExpandWidth(true));

            EditorGUILayout.EndScrollView();
        }

        private static void RenderButtons(IDialogEditorContext context)
        {
            EditorGUILayout.BeginHorizontal();
            UpdateSelectedButton(context);
            UpdateAll(context);
            EditorGUILayout.EndHorizontal();
        }

        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            var assets = new List<T>();
            var guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        private static void UpdateSelectedButton(IDialogEditorContext context)
        {
            GUI.enabled = _selectedItem > -1;

            if (GUILayout.Button("Update Selected Button"))
            {
                var asset = _assets[_selectedItem];
                UpdateAsset(asset);
                AssetDatabase.SaveAssets();
            }
            GUI.enabled = true;
        }

        private static void UpdateAll(IDialogEditorContext context)
        {
            if (GUILayout.Button("Update All"))
            {
                foreach(var asset in _assets)
                {
                    UpdateAsset(asset);
                }
                AssetDatabase.SaveAssets();
            }
        }

        private static void UpdateAsset(LocalizedTexts asset)
        {
            var data = GoogleApi.GetValuesInSheet(asset.sheetUrl);
            var sheet = new ExcelSheet()
            {
                Data = data
            };
            asset.PopulateWith(sheet);
            EditorUtility.SetDirty(asset);
        }
    }
}
