using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonEditors
{
    public delegate void OnKey(Keyboard keyboard);
    public delegate void OnMouse(MouseButton button, Vector2 position);
    public delegate void OnMouseDrag(MouseButton button, Vector2 position, Vector2 delta);
    public delegate void OnMouseMove(Vector2 position, Vector2 delta);
}
