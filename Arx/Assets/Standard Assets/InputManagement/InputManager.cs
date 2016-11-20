using UnityEngine;
using System.Collections;
using System;
using System;
using System.Collections.Generic;
using System.Linq;

public enum InputSource
{
    KBM,                //Keyboard and mouse
    WIN_XBOX            //xbox controller on windows
}

public static class InputManager
{
    private static KeyCode[] _keyboardMouseKeys;
    private static KeyCode[] _gamepadKeys;

    private static InputSource? _currentSource;

    static InputManager()
    {
        var values = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToArray();
        _keyboardMouseKeys = values.Where(key => key >= KeyCode.Backspace && key <= KeyCode.Mouse6).ToArray();
        _gamepadKeys = values.Where(key => key >= KeyCode.JoystickButton0 && key <= KeyCode.JoystickButton10).ToArray();
    }

    public static InputData GetInputData(InputSource? definedSource = null)
    {
        _currentSource = GetCurrentInputSource(definedSource);

        return new InputData(_currentSource);
    }

    private static bool IsKeyboardMouseInput()
    {
        for(var idx = 0; idx < _keyboardMouseKeys.Length; idx++)
        {
            if (Input.GetKey(_keyboardMouseKeys[idx]))
            {
                return true;
            }
        }
        if(Input.GetAxis("X Mouse") != 0 || Input.GetAxis("Y Mouse") != 0)
        {
            return true;
        }
        return false;
    }

    private static bool IsGamepadInput()
    {
        var joysticks = Input.GetJoystickNames();
        if (!joysticks.Any())
        {
            return false;
        }
        for (var idx = 0; idx < _gamepadKeys.Length; idx++)
        {
            if (Input.GetKey(_gamepadKeys[idx]))
            {
                return true;
            }
        }
        return false;
    }

    private static InputSource? GetCurrentInputSource(InputSource? definedSource = null)
    {
        if (definedSource == null)
        {
            if (IsKeyboardMouseInput())
            {
                return InputSource.KBM;
            }
            if (IsGamepadInput())
            {
                return InputSource.WIN_XBOX;
            }
        }
        else
        {
            return definedSource.Value;
        }

        return null;
    }
}

public class InputData
{
    public InputSource? InputSource { get; private set; }

    public InputData(InputSource? inputSource)
    {
        InputSource = inputSource;
    }
}