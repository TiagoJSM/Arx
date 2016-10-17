using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Controllers.Interaction
{
    public abstract class TemplateSpeechController : MonoBehaviour
    {
        [SerializeField]
        private bool _visible;

        public OnScrollEnd OnScrollEnd;
        public OnVisibilityChange OnVisibilityChange;

        public abstract string Text { get; set; }

        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if(_visible != value)
                {
                    _visible = value;
                    OnVisibleChange();
                    OnVisibilityChange?.Invoke(_visible);
                }
            }
        }
        public abstract bool SpeechEnded { get; }

        public abstract void Say(string text);
        public abstract bool Continue();
        public abstract void Close();
        protected virtual void OnVisibleChange() { }
    }
}
