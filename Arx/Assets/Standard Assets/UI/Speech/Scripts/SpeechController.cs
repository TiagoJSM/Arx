using GenericComponents.Behaviours;
using GenericComponents.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.Speech.Scripts
{
    public class SpeechController : TemplateSpeechController
    {
        private SpeechBubleController _speechBubble;

        [SerializeField]
        private CharacterIdentity _name;
        [SerializeField]
        private SpeechBubleController _speechBubblePrefab;
        [SerializeField]
        private GameObject speechPivot;

        public override string Text
        {
            get
            {
                return _speechBubble.Text;
            }
            set
            {
                _speechBubble.Text = value;
            }
        }

        public override bool SpeechEnded
        {
            get
            {
                return _speechBubble.SpeechEnded;
            }
        }

        public void ResetSpeech()
        {
            _speechBubble.ResetSpeech();
        }

        public bool ScrollPageDown()
        {
            return _speechBubble.ScrollPageDown();
        }

        public void ScrollDown(float scroll)
        {
            _speechBubble.ScrollDown(scroll);
        }

        public override bool Continue()
        {
            return _speechBubble.Continue();
        }

        public override void Say(string text)
        {
            _speechBubble.Say(text);
        }

        public override void Close()
        {
            _speechBubble.Close();
        }

        private void Awake()
        {
            _speechBubble = Instantiate(_speechBubblePrefab, speechPivot.transform, false);
            _speechBubble.Name = _name == null ? "" : _name.Name;

            _speechBubble.OnScrollEnd += OnScrollEndHandler;
            _speechBubble.OnVisibilityChange += OnVisibilityChangeHandler;
        }

        private void OnDestroy()
        {
            _speechBubble.OnScrollEnd -= OnScrollEndHandler;
            _speechBubble.OnVisibilityChange -= OnVisibilityChangeHandler;
        }

        private void OnScrollEndHandler()
        {
            if(OnScrollEnd != null)
            {
                OnScrollEnd();
            }
        }

        private void OnVisibilityChangeHandler(bool visibility)
        {
            if(OnVisibilityChange != null)
            {
                OnVisibilityChange(visibility);
            }
        }
    }
}
