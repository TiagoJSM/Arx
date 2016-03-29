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
                }
            }
        }
        public abstract bool SpeechEnded { get; }

        public abstract void Continue();
        protected virtual void OnVisibleChange() { }
    }
}
