using Assets.Standard_Assets.InventorySystem.Editor.CustomEditors;
using Assets.Standard_Assets.InventorySystem.Editor.GuiComponent;
using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using CommonEditors;
using CommonEditors.GuiComponents.GuiComponents.CustomEditors;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Assets.Standard_Assets.InventorySystem.Editor
{
    public class InventoryItemEditor : ExtendedEditorWindow
    {
        private static int NO_SELECTED_ITEM = -1;

        private List<BaseGuiComponent> _components;
        private Vector2 _scrollEditorPosition;
        private Vector2 _scrollItemListPosition;
        private InventoryItem _item;
        private string _itemSearch;
        private Type[] _inventoryItemTypes;
        private string[] _inventoryItemNames;
        private int _index;

        [MenuItem("Window/Inventory Item Editor")]
        static void Init()
        {
            var window = EditorWindow.GetWindow<InventoryItemEditor>();
            window.titleContent = new GUIContent("Inventory Item Editor");
        }

        protected override void DoOnGui()
        {
            EditorGUILayout.BeginVertical();

            HeaderMenu();

            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginHorizontal();

            ItemListPanel();
            GUILayout.Box(
                EditorGUIUtility.whiteTexture, 
                new GUILayoutOption[] { GUILayout.ExpandHeight(true), GUILayout.Width(1) });
            ItemForm();

            EditorGUILayout.EndHorizontal();
            
        }

        private void Awake()
        {
            _inventoryItemTypes = IntrospectionUtils.GetAllCompatibleTypes<InventoryItem>().ToArray();
            _inventoryItemNames = _inventoryItemTypes.Select(t => t.Name).ToArray();
        }

        private void HeaderMenu()
        {
            _index = EditorGUILayout.Popup(_index, _inventoryItemNames);
            if (GUILayout.Button("New Item"))
            {
                NewItemScreen();
            }
        }

        private void OnNewHandler()
        {
            NewItemScreen();
        }

        private void OnOpenItemHandler(string path)
        {
            var item = AssetDatabase.LoadAssetAtPath(path, typeof(InventoryItem)) as InventoryItem;
            if (item == null)
            {
                return;
            }
            LoadedItemScreen(item);
        }

        private void NewItemScreen()
        {
            var path = EditorUtility.SaveFilePanelInProject(
                                "Save item",
                                null,
                                "asset",
                                "Please enter a file name to save the item to");
            if (!string.IsNullOrEmpty(path))
            {
                var item = ScriptableObject.CreateInstance(_inventoryItemTypes[_index]) as InventoryItem;
                item.Id = GetNewId();
                LoadedItemScreen(item);
                AssetDatabase.CreateAsset(item, path);
                _item = item;
            }
        }

        private string GetNewId()
        {
            var items = FindAssetsByType<InventoryItem>();
            var id = default(string);
            var isUnique = false;
            do
            {
                id = Guid.NewGuid().ToString();
                isUnique = items.All(item => item.Id != id);
            } while (!isUnique);
            
            return id;
        }

        private void ItemListPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(300));
            EditorGUILayout.LabelField("Items");
            _itemSearch = EditorGUILayout.TextField(_itemSearch);
            EditorGUILayout.Separator();
            _scrollItemListPosition = GUILayout.BeginScrollView(_scrollItemListPosition);
            var items = GetItems(_itemSearch);
            var selected = _item == null ? NO_SELECTED_ITEM : items.IndexOf(_item);

            var newSelected = GUILayout.SelectionGrid(
                selected,
                items.Select(f => new GUIContent(f.Name)).ToArray(),
                1,
                GUILayout.ExpandWidth(true));

            if (newSelected != selected && newSelected >= 0)
            {
                _item = items[newSelected];
                LoadedItemScreen(_item);
            }
            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void ItemForm()
        {
            _scrollEditorPosition = GUILayout.BeginScrollView(_scrollEditorPosition);

            GUILayout.BeginVertical();

            if (_item != null)
            {
                DrawGuiComponents(_components);

                EditorGUILayout.LabelField("ID:", _item.Id);
                var editor = UnityEditor.Editor.CreateEditor(_item, typeof(InventoryItemCustomEditor));
                EditorGUILayout.InspectorTitlebar(new UnityEngine.Object[] { editor.target });

                editor.OnInspectorGUI();
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void LoadedItemScreen(InventoryItem item)
        {
            _item = item;
            _components = new List<BaseGuiComponent>() { };
        }

        private List<InventoryItem> GetItems(string name)
        {
            var items = FindAssetsByType<InventoryItem>();
            if (string.IsNullOrEmpty(name))
            {
                return items;
            }
            return items.Where(q => q.Name.ToLower().Contains(name.ToLower())).ToList();
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
    }
}
