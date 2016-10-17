using CommonInterfaces.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GenericComponents.Controllers.Interaction
{
    public class SpeechController : TemplateSpeechController
    {
        private readonly int OpenSpeechBubble = Animator.StringToHash("Open Speech Bubble");
        private readonly int CloseSpeechBubble = Animator.StringToHash("Close Speech Bubble");

        [SerializeField]
        private RectTransform _speechBubble;
        [SerializeField]
        private ScrollRect _scrollRect;
        private RectTransform _content;

        [SerializeField]
        private Animator _speechAnimator;
        [SerializeField]
        [TextArea(3, 10)]
        private string _text;

        public Text textUi;

        public override string Text
        {
            get
            {
                return textUi.text;
            }
            set
            {
                textUi.text = value;
            }
        }

        public override bool SpeechEnded
        {
            get
            {
                var speechBubbleHeight = _speechBubble.rect.height;
                if (_content.rect.height <= (_content.anchoredPosition.y + speechBubbleHeight))
                {
                    return true;
                }
                return false;
            }
        }

        void Start()
        {
            _content = _scrollRect.content;
            _content.localPosition = new Vector3();
            Text = _text;
        }

        public void Reset()
        {
            _content.localPosition = new Vector3();
        }

        public bool ScrollPageDown()
        {
            if (SpeechEnded)
            {
                OnScrollEnd?.Invoke();
                Close();
                return true;
            }
            //ToDo: scroll should use coroutine to make it smooth
            var speechBubbleHeight = _speechBubble.rect.height;
            ScrollDown(speechBubbleHeight);
            return false;
        }

        public void ScrollDown(float scroll)
        {
            var transformed = _content.anchoredPosition + new Vector2(0, scroll);
            _content.anchoredPosition = transformed;
        }

        public override bool Continue()
        {
            return ScrollPageDown();
        }

        public override void Say(string text)
        {
            Reset();
            Text = text;
            Visible = true;
        }

        public override void Close()
        {
            Text = string.Empty;
            Visible = false;
        }

        protected override void OnVisibleChange()
        {
            var trigger = Visible ? OpenSpeechBubble : CloseSpeechBubble;
            _speechAnimator.SetTrigger(trigger);
        }
    }
}
