using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WindowsXboxGamepad : IInputDevice
{
    private const float AbsoluteAxis = 0.5f;

    private Sprite[] _sprites;

    private Dictionary<string, bool> _upDetected;
    private Dictionary<string, bool> _downDetected;

    public bool MouseSupport { get { return false; } }
    public Sprite InteractSprite
    {
        get
        {
            return _sprites.FirstOrDefault(sprite => sprite.name == "B");
        }
    }

    public WindowsXboxGamepad()
    {
        _sprites = Resources.LoadAll<Sprite>("Xbox");
        _upDetected = new Dictionary<string, bool>();
        _downDetected = new Dictionary<string, bool>();

        var values =  Enum.GetValues(typeof(DeviceButton));
        foreach(var value in values)
        {
            _upDetected.Add(value.ToString(), false);
            _downDetected.Add(value.ToString(), false);
        }
    }

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
        return Input.GetButton(buttonName) || Input.GetAxis(buttonName) > AbsoluteAxis;
    }

    public bool GetButtonDown(DeviceButton button)
    {
        var buttonName = button.ToString();
        var down = Input.GetButtonDown(buttonName) || (Input.GetAxis(buttonName) > AbsoluteAxis && !_downDetected[buttonName]);

        if (down)
        {
            _downDetected[buttonName] = true;
        }

        return down;
    }

    public bool GetButtonUp(DeviceButton button)
    {
        var buttonName = button.ToString();
        var up =  Input.GetButtonUp(buttonName) || (Input.GetAxis(buttonName) < AbsoluteAxis && !_upDetected[buttonName]);

        if (up)
        {
            _upDetected[buttonName] = true;
        }

        return up;
    }

    public void Update()
    {
        var values = Enum.GetValues(typeof(DeviceButton));
        foreach (var value in values)
        {
            _upDetected[value.ToString()] = (Input.GetAxis(value.ToString()) < AbsoluteAxis);
            _downDetected[value.ToString()] = (Input.GetAxis(value.ToString()) > AbsoluteAxis);
        }
    }
}

