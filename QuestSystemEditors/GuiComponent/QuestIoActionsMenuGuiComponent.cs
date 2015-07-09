using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestSystemEditors.GuiComponent
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
    }
}
