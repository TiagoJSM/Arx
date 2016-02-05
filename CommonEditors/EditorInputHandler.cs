using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Extensions;

namespace CommonEditors
{
    public interface IInputCombination
    {
        bool CanExecute();
        void Execute();
    }

    public class InputCombination : IInputCombination
    {
        private KeyCode? _pressedKey;
        private MouseButton? _button;
        private EventModifiers? _modifiers;
        private Action _action;

        public InputCombination(Action action, MouseButton? button, EventModifiers? modifiers, KeyCode? pressedKey = null)
        {
            _pressedKey = pressedKey;
            _button = button;
            _modifiers = modifiers;
            _action = action;
        }

        public bool CanExecute()
        {
            var keyboard = new Keyboard(Event.current);
            if (_modifiers != null)
            {
                if ((keyboard.Modifiers & _modifiers.Value) != _modifiers.Value)
                {
                    return false;
                }
            }

            if (_pressedKey != null)
            {
                if ((keyboard.Code & _pressedKey.Value) != _pressedKey.Value)
                {
                    return false;
                }
            }

            if (_button == null)
            {
                return true;
            }

            if (Event.current.type != EventType.mouseUp)
            {
                return false;
            }

            return Event.current.button == (int)_button.Value;
        }

        public void Execute()
        {
            _action();
        }
    }

    public class DuplicateEventCombination : IInputCombination
    {
        private Action _action;
        private bool _cancelDefaultDuplicateEvent;

        public DuplicateEventCombination(Action action, bool cancelDefaultDuplicateEvent = true)
        {
            _action = action;
            _cancelDefaultDuplicateEvent = cancelDefaultDuplicateEvent;
        }

        public bool CanExecute()
        {
            Event e = Event.current;
            if (e != null && e.type == EventType.ValidateCommand && e.commandName == "Duplicate")
            {
                return true;
            }
            return false;
        }

        public void Execute()
        {
            _action();
            if (_cancelDefaultDuplicateEvent)
            {
                Event.current.CancelDuplicate();
            }
        }
    }

    public class EditorInputHandler
    {
        private List<IInputCombination> _combinations;

        public EditorInputHandler(params IInputCombination[] combinations)
        {
            _combinations = new List<IInputCombination>(combinations);
        }

        public void Add(params IInputCombination[] combinations)
        {
            _combinations.AddRange(combinations);
        }

        public void HandleInput()
        {
            foreach (var combination in _combinations)
            {
                if (combination.CanExecute())
                {
                    combination.Execute();
                }
            }
        }
    }
}
