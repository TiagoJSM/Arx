using CommonEditors;
using CommonEditors.GuiComponents.GuiComponents.CustomEditors;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using InventorySystem.InventoryObjects;
using InventorySystemEditors.CustomEditors;
using InventorySystemEditors.GuiComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace InventorySystemEditors
{
    public class InventoryItemEditor : ExtendedEditorWindow
    {
        private InventoryItemIoActionsMenuGuiComponent _topButtonMenus;
        private List<BaseGuiComponent> _components;
        private Vector2 _scrollPosition;
        private InventoryItem _item;

        [MenuItem("Window/Inventory Item Editor")]
        static void Init()
        {
            EditorWindow.GetWindow<InventoryItemEditor>();
        }

        public InventoryItemEditor()
        {
            _topButtonMenus = new InventoryItemIoActionsMenuGuiComponent();
            _topButtonMenus.OnNew += OnNewHandler;
            _topButtonMenus.OnOpenFile += OnOpenItemHandler;
            //OnMouseUp += OnMouseUpHandler;
            NewItemScreen();
        }

        protected override void DoOnGui()
        {
            _topButtonMenus.OnGui();

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            //_scrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), _scrollPosition, new Rect(0, 0, 1000, 1000));
            GUILayout.BeginVertical();
            DrawGuiComponents(_components);

            var editor = Editor.CreateEditor(_item, typeof(InventoryItemCustomEditor));
            EditorGUILayout.InspectorTitlebar(new UnityEngine.Object[]{editor.target});

            editor.OnInspectorGUI();
            GUILayout.EndVertical();
            //GUI.EndScrollView();
            GUILayout.EndScrollView();
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
            var item = ScriptableObject.CreateInstance(_topButtonMenus.SelectedInventoryType) as InventoryItem;
            //item.Id = Guid.NewGuid().ToString();
            LoadedItemScreen(item);
        }

        private void LoadedItemScreen(InventoryItem item)
        {
            _item = item;
            _topButtonMenus.Object = _item;
            _components = new List<BaseGuiComponent>() { };
            /*foreach (var condition in _quest.conditions)
            {
                var conditionComponent = new ConditionGuiComponent(condition);
                _components.Add(conditionComponent);
            }*/
        }
    }
}
