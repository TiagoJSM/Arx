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
        private string _questSearch;

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
        }

        protected override void DoOnGui()
        {
            _topButtonMenus.OnGui();
            
            
            EditorGUILayout.BeginHorizontal();

            QuestListPanel();
            EditorGUILayout.Separator();
            QuestForm();

            EditorGUILayout.EndHorizontal();
            
        }

        private void Awake()
        {
            NewQuestScreen();
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
            var condition = Activator.CreateInstance(type) as ICondition;//ScriptableObject.CreateInstance(type) as Condition;
            _quest.conditions.Add(condition);
            /*var conditionComponent = new ConditionGuiComponent(condition);
            _components.Add(conditionComponent);*/
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
            quest.questId = Guid.NewGuid().ToString();
            LoadedQuestScreen(quest);
        }

        private void LoadedQuestScreen(Quest quest)
        {
            _quest = quest;
            _topButtonMenus.Object = _quest;
            _topButtonMenus.OnNew += OnNewHandler;
            _components = new List<BaseGuiComponent>() { new QuestGuiComponent(_quest) };
            /*foreach (var condition in _quest.conditions)
            {
                var conditionComponent = new ConditionGuiComponent(condition);
                _components.Add(conditionComponent);
            }*/
        }

        private void QuestListPanel()
        {
            EditorGUILayout.BeginVertical();
            _questSearch = EditorGUILayout.TextField(_questSearch);
            EditorGUILayout.Separator();
            var quests = GetQuests(_questSearch);
            foreach(var quest in quests)
            {

                EditorGUILayout.LabelField(quest.questName);
            }
            EditorGUILayout.EndVertical();
        }

        private List<Quest> GetQuests(string name)
        {
            var quests = FindAssetsByType<Quest>();
            if (string.IsNullOrEmpty(name))
            {
                return quests;
            }
            return quests.Where(q => q.questName.ToLower().Contains(name.ToLower())).ToList();
        }

        private void QuestForm()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.BeginVertical();
            DrawGuiComponents(_components);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
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
