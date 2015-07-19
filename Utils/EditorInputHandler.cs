using CommonEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utils
{
    public class InputCombination
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
            if(_modifiers != null)
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

    public class EditorInputHandler
    {
        private InputCombination[] _combinations;

        public EditorInputHandler(params InputCombination[] combinations)
        {
            _combinations = combinations;
        }

        public void HandleInput()
        {
            foreach(var combination in _combinations)
            {
                if (combination.CanExecute())
                {
                    combination.Execute();
                    return;
                }
            }
        }
    }
}
