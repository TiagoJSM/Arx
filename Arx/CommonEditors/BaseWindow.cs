using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonEditors
{
    public abstract class BaseWindow
    {
        public Rect ClientRect { get; set; }
        public string Title { get; set; }

        public abstract void WindowFunction(int id);
    }
}
