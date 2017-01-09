using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Editor.GuiComponent
{
    public class QuestIoActionsMenuGuiComponent : FileIoActionsMenuGuiComponent
    {
        public QuestIoActionsMenuGuiComponent(UnityEngine.Object obj = null)
            : base(
                "Save quest", 
                "Please enter a file name to save the quest to",
                "Open quest",
                "asset",
                obj)
        {

        }

        protected override void AfterCreatingAsset(UnityEngine.Object obj, string path)
        {
            base.AfterCreatingAsset(obj, path);
            /*var quest = obj as Quest;
            foreach(var condition in quest.conditions)
            {
                AssetDatabase.AddObjectToAsset(condition, quest);
            }
            AssetDatabase.SaveAssets();*/
        }
    }
}
