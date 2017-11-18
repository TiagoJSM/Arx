using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Standard_Assets._2D.Scripts.Managers;

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
    Throw,
    Vertical,
    Roll
}

public enum DeviceAxis
{
    Movement,
    AimAnalog,
}

public interface IInputDevice
{
    bool MouseSupport { get; }
    Sprite InteractSprite { get; }
    Sprite Up { get; }

    bool GetButtonDown(DeviceButton button);
    bool GetButton(DeviceButton button);
    bool GetButtonUp(DeviceButton button);
    Vector2 GetAxis(DeviceAxis axis);
    void Update();
}

public class InputManager : Singleton<InputManager>
{
    private KeyCode[] _keyboardMouseKeys;
    private KeyCode[] _gamepadKeys;

    private InputSource? _currentSource;
    private IInputDevice _currentDevice;

    protected InputManager()
    {
        var values = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToArray();
        _keyboardMouseKeys = values.Where(key => key >= KeyCode.Backspace && key <= KeyCode.Mouse6).ToArray();
        _gamepadKeys = values.Where(key => key >= KeyCode.JoystickButton0 && key <= KeyCode.JoystickButton10).ToArray();
    }

    public IInputDevice GetInputDevice(InputSource? definedSource = null)
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

        return InputDevice();
    }

    private IInputDevice InputDevice()
    {
        switch (_currentSource)
        {
            case InputSource.KBM:
                if (!(_currentDevice is KeyboardDevice))
                {
                    _currentDevice = new KeyboardDevice();
                }
                break;
            case InputSource.WIN_XBOX:
                if (!(_currentDevice is WindowsXboxGamepad))
                {
                    _currentDevice = new WindowsXboxGamepad();
                }
                break;
            default:
                _currentDevice = null;
                break;
        }
        return _currentDevice;
    }

    private bool IsKeyboardMouseInput()
    {
        for (var idx = 0; idx < _keyboardMouseKeys.Length; idx++)
        {
            if (Input.GetKey(_keyboardMouseKeys[idx]))
            {
                return true;
            }
        }
        if (Input.GetAxis("XMouse") != 0 || Input.GetAxis("YMouse") != 0)
        {
            return true;
        }
        return false;
    }

    private bool IsGamepadInput()
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

    private InputSource? GetCurrentInputSource(InputSource? definedSource = null)
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

    private void Update()
    {
        if(_currentDevice != null)
        {
            _currentDevice.Update();
        }
    }
}