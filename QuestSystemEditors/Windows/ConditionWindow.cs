using CommonEditors;
using CommonEditors.GuiComponents;
using QuestSystem.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QuestSystemEditors.Windows
{
    public class ConditionWindow : BaseWindow
    {
        public ICondition Condition { get; private set; }

        public OnClick OnClick;

        public ConditionWindow(ICondition condition)
        {
            Condition = condition;
            Title = "Condition";
            ClientRect = new Rect(0, 0, 200, 50);
        }

        public override void WindowFunction(int id)
        {
            var e = Event.current;
            if (e.type == EventType.MouseDown && e.button == (int)MouseButton.Left)
            {
                var defaultRect = ClientRect;
                defaultRect.x = 0;
                defaultRect.y = 0;
                if (defaultRect.Contains(e.mousePosition))
                {
                    if (OnClick != null)
                    {
                        OnClick(this);
                    }
                }
            }

            GUILayout.Label(Condition.GetType().Name);
            GUI.DragWindow();
        }
    }
}
