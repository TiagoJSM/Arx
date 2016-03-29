using CommonInterfaces.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GenericComponents.Controllers.Interaction
{
    [RequireComponent(typeof(ScrollRect))]
    [RequireComponent(typeof(RectTransform))]
    public class SpeechController : TemplateSpeechController
    {
        private Canvas _canvas;
        private bool _speechEnded;
        private RectTransform _scrollRectRectTransform;
        private RectTransform _contentRectTransform;
        private ScrollRect _scrollRect;
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

        public override bool SpeechEnded { get { return _speechEnded; } }

        void Start()
        {
            _canvas = GetComponentInChildren<Canvas>();
            _scrollRect = GetComponent<ScrollRect>();
            _scrollRectRectTransform = GetComponent<RectTransform>();
            _contentRectTransform = _scrollRect.content;
            _contentRectTransform.position = new Vector3();
            Text = _text;
        }

        public void Reset()
        {
            _contentRectTransform.position = new Vector3();
            _speechEnded = false;
        }

        public void ScrollPageDown()
        {
            var speechBubbleHeight = _scrollRectRectTransform.rect.height;
            var textBubbleHeight = _scrollRect.content.rect.height;
            ScrollDown(speechBubbleHeight);
        }

        public void ScrollDown(float scroll)
        {
            var transformed = _contentRectTransform.anchoredPosition + new Vector2(0, scroll);
            _contentRectTransform.anchoredPosition = transformed;

            if (_scrollRect.verticalNormalizedPosition <= 0)
            {
                _speechEnded = true;
                if (OnScrollEnd != null)
                {
                    OnScrollEnd();
                }
            }
        }

        public override void Continue()
        {
            ScrollPageDown();
        }

        protected override void OnVisibleChange()
        {
            _canvas.enabled = Visible;
        }
    }
}
