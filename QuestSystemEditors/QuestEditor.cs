using CommonEditors;
using CommonEditors.GuiComponents;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using QuestSystem.Conditions;
using QuestSystem.QuestStructures;
using QuestSystemEditors.Components;
using QuestSystemEditors.GuiComponent;
using QuestSystemEditors.Utils;
using QuestSystemEditors.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace QuestSystemEditors
{
    public class QuestEditor : ExtendedEditorWindow
    {
        private QuestIoActionsMenuGuiComponent _topButtonMenus;
        private List<BaseGuiComponent> _components;
        
        private Vector2 _scrollPosition;

        private Quest _quest;

        [MenuItem("Window/Quest Editor")]
        static void Init()
        {
            EditorWindow.GetWindow<QuestEditor>();
        }

        public QuestEditor()
        {
            _topButtonMenus = new QuestIoActionsMenuGuiComponent();
            _topButtonMenus.OnNew += OnNewHandler;
            _topButtonMenus.OnOpenFile += OnOpenQuestHandler;
            OnMouseUp += OnMouseUpHandler;
            NewQuestScreen();
        }

        protected override void DoOnGui()
        {
            _topButtonMenus.OnGui();
            
            _scrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), _scrollPosition, new Rect(0, 0, 1000, 1000));
            GUILayout.BeginVertical();
            DrawGuiComponents(_components);
            GUILayout.EndVertical();
            GUI.EndScrollView();
        }

        private void OnMouseUpHandler(MouseButton button, Vector2 position)
        {
            if (button != MouseButton.Right)
            {
                return;
            }
            var toolsMenu = ContextMenuUtils.GetcontextMenuForQuestEditor(AddConditonWindow);
            toolsMenu.ShowAsContext();
        }

        private void AddConditonWindow(object conditionType)
        {
            var type = conditionType as Type;
            if(type == null)
            {
                return;
            }
            var condition = ScriptableObject.CreateInstance(type) as Condition;
            _quest.conditions.Add(condition);
            var conditionComponent = new ConditionGuiComponent(condition);
            _components.Add(conditionComponent);
        }

        private void OnNewHandler()
        {
            NewQuestScreen();
        }

        private void OnOpenQuestHandler(string path)
        {
            var quest = AssetDatabase.LoadAssetAtPath(path, typeof(Quest)) as Quest;
            if (quest == null)
            {
                return;
            }
            LoadedQuestScreen(quest);
        }

        private void NewQuestScreen()
        {
            var quest = ScriptableObject.CreateInstance<Quest>();
            LoadedQuestScreen(quest);
        }

        private void LoadedQuestScreen(Quest quest)
        {
            _quest = quest;
            _topButtonMenus.Object = _quest;
            _components = new List<BaseGuiComponent>() { new QuestGuiComponent(_quest) };
            foreach (var condition in _quest.conditions)
            {
                var conditionComponent = new ConditionGuiComponent(condition);
                _components.Add(conditionComponent);
            }
        }
    }
}
