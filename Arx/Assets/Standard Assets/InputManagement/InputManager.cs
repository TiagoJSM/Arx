using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public enum InputSource
{
    KBM,                //Keyboard and mouse
    WIN_XBOX            //xbox controller on windows
}

public enum DeviceButton
{
    PrimaryAttack,
    SecundaryAttack,
    Jump,
    Interact,
    InGameMenu,
    SetWeaponSocket1,
    SetWeaponSocket2,
    AimWeapon,
    ShootWeapon,
    Throw
}

public enum DeviceAxis
{
    Movement,
    AimAnalog,
}

public interface IInputDevice
{
    bool MouseSupport { get; }

    bool GetButtonDown(DeviceButton button);
    bool GetButton(DeviceButton button);
    bool GetButtonUp(DeviceButton button);
    Vector2 GetAxis(DeviceAxis axis);
}

public static class InputManager
{
    private static KeyCode[] _keyboardMouseKeys;
    private static KeyCode[] _gamepadKeys;

    private static InputSource? _currentSource;
    private static IInputDevice _currentDevice;

    static InputManager()
    {
        var values = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToArray();
        _keyboardMouseKeys = values.Where(key => key >= KeyCode.Backspace && key <= KeyCode.Mouse6).ToArray();
        _gamepadKeys = values.Where(key => key >= KeyCode.JoystickButton0 && key <= KeyCode.JoystickButton10).ToArray();
    }

    public static IInputDevice GetInputDevice(InputSource? definedSource = null)
    {
        var currentInput = GetCurrentInputSource(definedSource);
        
        if (currentInput != null)
        {
            _currentSource = currentInput;
        }
        
        if(_currentSource == null)
        {
            _currentSource = InputSource.KBM;
        }

        switch (_currentSource)
        {
            case InputSource.KBM:       return new KeyboardDevice();
            case InputSource.WIN_XBOX:  return new WindowsXboxGamepad();
        }

        return null;
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
        if(Input.GetAxis("XMouse") != 0 || Input.GetAxis("YMouse") != 0)
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