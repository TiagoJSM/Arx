using Assets.Standard_Assets.QuestSystem.Conditions;
using Assets.Standard_Assets.QuestSystem.Editor.Components;
using Assets.Standard_Assets.QuestSystem.Editor.GuiComponent;
using Assets.Standard_Assets.QuestSystem.Editor.Utils;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.QuestSystem.RewardProviders;
using Assets.Standard_Assets.QuestSystem.Tasks;
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

        private QuestGuiComponent _questComponent;
        private List<TaskGuiComponent> _taskComponents;
        private List<ConditionGuiComponent> _conditionComponents;
        private List<RewardProviderGuiComponent> _rewardProviderComponents;
        private Quest _selectedQuest = null;
        private Vector2 _scrollPosition;
        private Vector2 _scrollQuestListPosition;
        private string _questSearch;

        private Quest _quest;

        [MenuItem("Window/Quest Editor")]
        static void Init()
        {
            var window = EditorWindow.GetWindow<QuestEditor>();
            window.titleContent = new GUIContent("Quest Editor");
        }

        public QuestEditor()
        {
            OnMouseUp += OnMouseUpHandler;
        }

        public void RemoveRewardProvider(RewardProviderGuiComponent rewardProviderGuiComponent)
        {
            _rewardProviderComponents.Remove(rewardProviderGuiComponent);
            _quest.rewardProviders.Remove(rewardProviderGuiComponent.RewardProvider);
        }

        public void RemoveCondition(ConditionGuiComponent conditionGuiComponent)
        {
            _conditionComponents.Remove(conditionGuiComponent);
            _quest.conditions.Remove(conditionGuiComponent.Condition);
        }

        public void RemoveTask(TaskGuiComponent taskGuiComponent)
        {
            _taskComponents.Remove(taskGuiComponent);
            _quest.tasks.Remove(taskGuiComponent.Task);
        }

        protected override void DoOnGui()
        {
            EditorGUILayout.BeginHorizontal();

            NewQuestButton();
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            QuestListPanel();
            GUILayout.Box(
                EditorGUIUtility.whiteTexture,
                new GUILayoutOption[] { GUILayout.ExpandHeight(true), GUILayout.Width(1) });
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
            if (button != MouseButton.Right || _quest == null)
            {
                return;
            }
            var toolsMenu = ContextMenuUtils.GetcontextMenuForQuestEditor(AddConditonWindow);
            toolsMenu.ShowAsContext();
        }

        private void AddConditonWindow(object data)
        {
            var type = data as Type;
            if(type == null)
            {
                return;
            }
            if (typeof(ICondition).IsAssignableFrom(type))
            {
                var condition = Activator.CreateInstance(type) as ICondition;
                _quest.conditions.Add(condition);
                var conditionComponent = new ConditionGuiComponent(condition, this);
                _conditionComponents.Add(conditionComponent);
            }
            else if (typeof(ITask).IsAssignableFrom(type))
            {
                var task = Activator.CreateInstance(type) as ITask;
                _quest.tasks.Add(task);
                var taskComponent = new TaskGuiComponent(task, this);
                _taskComponents.Add(taskComponent);
            }
            else if (typeof(IRewardProvider).IsAssignableFrom(type))
            {
                HandleAddRewardProvider(type);
            }
        }

        private void HandleAddRewardProvider(Type providerType)
        {
            IRewardProvider provider;
            if (typeof(ScriptableObject).IsAssignableFrom(providerType))
            {
                provider = ScriptableObject.CreateInstance(providerType) as IRewardProvider;
                var path = AssetDatabase.GetAssetPath(_quest);
                AssetDatabase.AddObjectToAsset(provider as ScriptableObject, path);
            }
            else
            {
                provider = Activator.CreateInstance(providerType) as IRewardProvider;
            }
            
            _quest.rewardProviders.Add(provider);
            var rewardProviderComponent = new RewardProviderGuiComponent(provider, this);
            _rewardProviderComponents.Add(rewardProviderComponent);
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
            _questComponent = new QuestGuiComponent(_quest);
            _taskComponents = new List<TaskGuiComponent>();
            _conditionComponents = new List<ConditionGuiComponent>();
            _rewardProviderComponents = new List<RewardProviderGuiComponent>();

            foreach (var task in _quest.tasks)
            {
                var taskComponent = new TaskGuiComponent(task, this);
                _taskComponents.Add(taskComponent);
            }
            foreach (var condition in _quest.conditions)
            {
                var conditionComponent = new ConditionGuiComponent(condition, this);
                _conditionComponents.Add(conditionComponent);
            }
            foreach (var rewardProvider in _quest.rewardProviders)
            {
                var rewardProviderComponent = new RewardProviderGuiComponent(rewardProvider, this);
                _rewardProviderComponents.Add(rewardProviderComponent);
            }            
        }

        private void QuestListPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(300));
            EditorGUILayout.LabelField("Quests");
            _questSearch = EditorGUILayout.TextField(_questSearch);
            EditorGUILayout.Separator();
            _scrollQuestListPosition = GUILayout.BeginScrollView(_scrollQuestListPosition);
            var quests = GetQuests(_questSearch);
            var selected = _selectedQuest == null ? NO_SELECTED_QUEST : quests.IndexOf(_selectedQuest);

            var newSelected = GUILayout.SelectionGrid(
                selected,
                quests.Select(f => new GUIContent(f.questName)).ToArray(),
                1,
                GUILayout.ExpandWidth(true));
            
            if (newSelected != selected && newSelected >= 0)
            {
                _selectedQuest = quests[newSelected];
                LoadedQuestScreen(_selectedQuest);
            }

            GUILayout.EndScrollView();
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
                DrawGuiComponents(new QuestGuiComponent[] { _questComponent });
                DrawGuiComponents(_taskComponents.ToArray());
                DrawGuiComponents(_conditionComponents.ToArray());
                DrawGuiComponents(_rewardProviderComponents.ToArray());
                if (!_conditionComponents.Any() && !_taskComponents.Any())
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
