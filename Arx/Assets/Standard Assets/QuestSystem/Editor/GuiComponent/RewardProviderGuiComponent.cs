using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.QuestSystem.RewardProviders;
using Assets.Standard_Assets.Utility.Editor;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Editor.GuiComponent
{
    public class RewardProviderGuiComponent : BaseGuiComponent
    {
        private const string FoldoutTextTemplate = "Reward Provider - {0}";

        private bool _showCondition = true;
        private QuestEditor _editor;

        public IRewardProvider RewardProvider { get; private set; }

        public RewardProviderGuiComponent(IRewardProvider rewardProvider, QuestEditor editor)
        {
            RewardProvider = rewardProvider;
            _editor = editor;
        }

        public override void OnGui()
        {
            var currentIdentLevel = EditorGUI.indentLevel;
            _showCondition = EditorGUILayout.Foldout(_showCondition, RewardProvider.GetType().Name);
            if (_showCondition)
            {
                EditorGUI.indentLevel++;
                var fields = ExposeFields.GetFields(RewardProvider);
                ExposeFields.Expose(fields);
            }
            EditorGUI.indentLevel = currentIdentLevel;
            if (GUILayout.Button("Remove"))
            {
                _editor.RemoveRewardProvider(this);
            }
        }
    }
}
