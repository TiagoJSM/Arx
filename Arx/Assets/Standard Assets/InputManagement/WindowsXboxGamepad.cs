using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WindowsXboxGamepad : IInputDevice
{
    public bool MouseSupport { get { return false; } }

    public Vector2 GetAxis(DeviceAxis axis)
    {
        switch (axis)
        {
            case DeviceAxis.AimAnalog:  return new Vector2(Input.GetAxis("AimAnalogX"), Input.GetAxis("AimAnalogY"));
            case DeviceAxis.Movement:   return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        return Vector2.zero;
    }

    public bool GetButton(DeviceButton button)
    {
        var buttonName = button.ToString();
        return Input.GetButton(buttonName) || Input.GetAxis(buttonName) > 0.5f;
    }

    public bool GetButtonDown(DeviceButton button)
    {
        var buttonName = button.ToString();
        return Input.GetButtonDown(buttonName) || Input.GetAxis(buttonName) > 0.5f;
    }

    public bool GetButtonUp(DeviceButton button)
    {
        var buttonName = button.ToString();
        return Input.GetButtonUp(buttonName) || Input.GetAxis(buttonName) < 0.5f;
    }
}

