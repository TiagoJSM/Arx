using CommonEditors.GuiComponents;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CommonEditors
{
    public abstract class ExtendedEditorWindow : EditorWindow
    {
        public OnKey OnKeyUp;
        public OnKey OnKeyDown;
        public OnMouse OnMouseUp;
        public OnMouse OnMouseDown;
        public OnMouseDrag OnMouseDrag;
        public OnMouseMove OnMouseMove;

        private Dictionary<EventType, Action> EventMap { get; set; }

        public ExtendedEditorWindow()
        {
            this.EventMap = new Dictionary<EventType, Action>
            {
                { 
                    EventType.KeyDown, () => 
                    {
                        if (OnKeyDown != null)
                        {
                            OnKeyDown(new Keyboard(Event.current));
                        }
                    }
                },
                { 
                    EventType.KeyUp, () => 
                    {
                        if (OnKeyUp != null)
                        {
                            OnKeyUp(new Keyboard(Event.current));
                        }
                    }
                },
                { 
                    EventType.MouseDown, () => 
                    {
                        if (OnMouseDown != null)
                        {
                            OnMouseDown((MouseButton)Event.current.button, Event.current.mousePosition);
                        }
                    }
                },
                { 
                    EventType.MouseUp, () => 
                    {
                        if (OnMouseUp != null)
                        {
                            OnMouseUp((MouseButton)Event.current.button, Event.current.mousePosition);
                        }
                    }
                },
                { 
                    EventType.MouseDrag, () => 
                    {
                        if (OnMouseDrag != null)
                        {
                            OnMouseDrag((MouseButton)Event.current.button, Event.current.mousePosition, Event.current.delta);
                        }
                    }
                },
                { 
                    EventType.MouseMove, () => 
                    {
                        if (OnMouseMove != null)
                        {
                            OnMouseMove(Event.current.mousePosition, Event.current.delta);
                        }
                    }
                }
            };
        }

        private void OnGUI()
        {
            var controlId =
                GUIUtility.GetControlID(FocusType.Passive);

            var controlEvent =
                Event.current.GetTypeForControl(controlId);

            if (this.EventMap.ContainsKey(controlEvent))
            {
                this.EventMap[controlEvent].Invoke();
            }
            DoOnGui();
        }

        protected void DrawWindows(IEnumerable<BaseWindow> windows)
        {
            var idx = 0;
            BeginWindows();
            foreach (var window in windows)
            {
                window.ClientRect = GUILayout.Window(idx++, window.ClientRect, window.WindowFunction, window.Title);
            }
            EndWindows();
        }

        protected void DrawGuiComponents(IEnumerable<BaseGuiComponent> components)
        {
            foreach (var component in components)
            {
                component.OnGui();
            }
        }

        protected abstract void DoOnGui();
    }
}
