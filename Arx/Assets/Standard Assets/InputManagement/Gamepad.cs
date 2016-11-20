using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum GamepadButton
{
    Button1,
    Button2,
    Button3,
    Button4,
    LeftShoulder,
    RightShoulder,
    LeftTrigger,
    RightTrigger
}

public enum GamepadDPad
{
    Up,
    Right,
    Down,
    Left,
}

public enum GamepadAnalog
{
    Left,
    Right
}

public class Gamepad
{
    private InputData _inputData;

    public static Gamepad GetGamepad()
    {
        var data = InputManager.GetInputData();
        if(data.InputSource == null)
        {
            return null;
        }
        return new Gamepad(data);
    }

    private Gamepad(InputData inputData)
    {
        _inputData = inputData;
    }

    public bool GetButtonDown(GamepadButton button)
    {
        var buttonName = FormatButtonName(button);
        if (button == GamepadButton.LeftTrigger || button == GamepadButton.RightTrigger)
        {
            return Input.GetAxis(buttonName) > 0.5f;
        }
        return Input.GetButtonDown(buttonName);
    }

    public bool GetButton(GamepadButton button)
    {
        var buttonName = FormatButtonName(button);
        if (button == GamepadButton.LeftTrigger || button == GamepadButton.RightTrigger)
        {
            return Input.GetAxis(buttonName) > 0.5f;
        }
        return Input.GetButton(FormatButtonName(button));
    }

    public bool GetButtonUp(GamepadButton button)
    {
        var buttonName = FormatButtonName(button);
        if (button == GamepadButton.LeftTrigger || button == GamepadButton.RightTrigger)
        {
            return Input.GetAxis(buttonName) < 0.5f;
        }
        return Input.GetButtonUp(FormatButtonName(button));
    }

    public bool GetDPadDown(GamepadDPad dPAd)
    {
        switch (dPAd)
        {
            case GamepadDPad.Up: return Input.GetAxis(FormatDPadYAxis(dPAd)) > 0;
            case GamepadDPad.Down: return Input.GetAxis(FormatDPadYAxis(dPAd)) < 0;
            case GamepadDPad.Right: return Input.GetAxis(FormatDPadXAxis(dPAd)) > 0;
            case GamepadDPad.Left: return Input.GetAxis(FormatDPadXAxis(dPAd)) < 0;
        }
        return false;
    }

    public Vector2 GetAxis(GamepadAnalog analog)
    {
        return new Vector2(
            Input.GetAxis(FormatXAxis(analog)),
            Input.GetAxis(FormatYAxis(analog)));
    }

    private string FormatButtonName(GamepadButton button)
    {
        return _inputData.InputSource + "_" + button.ToString();
    }

    private string FormatXAxis(GamepadAnalog analog)
    {
        return _inputData.InputSource + "_" + analog.ToString() + "x";
    }

    private string FormatYAxis(GamepadAnalog analog)
    {
        return _inputData.InputSource + "_" + analog.ToString() + "y";
    }

    private string FormatDPadXAxis(GamepadDPad dPAd)
    {
        return _inputData.InputSource + "_" + dPAd.ToString() + "x";
    }

    private string FormatDPadYAxis(GamepadDPad dPAd)
    {
        return _inputData.InputSource +"_" + dPAd.ToString() + "y";
    }
}

