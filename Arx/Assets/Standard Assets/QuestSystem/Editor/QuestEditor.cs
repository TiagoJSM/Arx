using Assets.Standard_Assets.QuestSystem.Conditions;
using Assets.Standard_Assets.QuestSystem.Editor.Components;
using Assets.Standard_Assets.QuestSystem.Editor.GuiComponent;
using Assets.Standard_Assets.QuestSystem.Editor.Utils;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using CommonEditors;
using CommonEditors.GuiComponents;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Editor
{
    public class QuestEditor : ExtendedEditorWindow
    {
        private static int NO_SELECTED_QUEST = -1;

        private List<BaseGuiComponent> _components;
        private Quest _selectedQuest = null;
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
            OnMouseUp += OnMouseUpHandler;
        }

        protected override void DoOnGui()
        {
            EditorGUILayout.BeginHorizontal();

            NewQuestButton();
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            QuestListPanel();
            EditorGUILayout.Separator();
            QuestForm();

            EditorGUILayout.EndHorizontal();
        }

        private void NewQuestButton()
        {
            if (GUILayout.Button("New Quest"))
            {
                NewQuestScreen();
            }
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
            var condition = Activator.CreateInstance(type) as ICondition;
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
            var path = EditorUtility.SaveFilePanelInProject(
                                "Save quest",
                                null,
                                "asset",
                                "Please enter a file name to save the quest to");
            if (!string.IsNullOrEmpty(path))
            {
                var quest = ScriptableObject.CreateInstance<Quest>();
                quest.questId = Guid.NewGuid().ToString();
                LoadedQuestScreen(quest);
                AssetDatabase.CreateAsset(quest, path);
                _selectedQuest = quest;
            }
        }

        private void LoadedQuestScreen(Quest quest)
        {
            _quest = quest;
            _components = new List<BaseGuiComponent>() { new QuestGuiComponent(_quest) };
            foreach (var condition in _quest.conditions)
            {
                var conditionComponent = new ConditionGuiComponent(condition);
                _components.Add(conditionComponent);
            }
        }

        private void QuestListPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(300));
            EditorGUILayout.LabelField("Quests");
            _questSearch = EditorGUILayout.TextField(_questSearch);
            EditorGUILayout.Separator();
            var quests = GetQuests(_questSearch);
            var selected = _selectedQuest == null ? NO_SELECTED_QUEST : quests.IndexOf(_selectedQuest);

            var newSelected = GUILayout.SelectionGrid(
                selected,
                quests.Select(f => new GUIContent(f.questName)).ToArray(),
                1,
                GUILayout.ExpandWidth(true));
            
            if(newSelected != selected && newSelected >= 0)
            {
                _selectedQuest = quests[newSelected];
                LoadedQuestScreen(_selectedQuest);
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

            if (_quest != null)
            {
                DrawGuiComponents(_components);
                if (_components.Count == 1)
                {
                    EditorGUILayout.LabelField("Right click to add elements");
                }
            }
                        
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
