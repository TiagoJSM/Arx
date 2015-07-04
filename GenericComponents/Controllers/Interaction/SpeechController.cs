using CommonInterfaces.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GenericComponents.Controllers.Interaction
{
    public class SpeechController : MonoBehaviour
    {
        private Canvas _canvas;

        private RectTransform _scrollRectRectTransform;
        private RectTransform _contentRectTransform;
        [SerializeField]
        [TextArea(3, 10)]
        private string _text;

        public OnScrollEnd OnScrollEnd;

        public ScrollRect scrollRect;
        public Text textUi;

        public string Text
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

        public bool Visible 
        {
            get { return _canvas.enabled; } 
            set 
            {
                if (value)
                {
                    _contentRectTransform.position = new Vector3();
                }
                _canvas.enabled = value;
            } 
        }

        void Start()
        {
            _canvas = GetComponentInChildren<Canvas>();
            _scrollRectRectTransform = scrollRect.GetComponent<RectTransform>();
            _contentRectTransform = scrollRect.content;
            _contentRectTransform.position = new Vector3();
            _canvas.enabled = false;
            Text = _text;
        }

        public void ScrollPageDown()
        {
            var speechBubbleHeight = _scrollRectRectTransform.rect.height;
            var textBubbleHeight = scrollRect.content.rect.height;
            ScrollDown(speechBubbleHeight);
        }

        public void ScrollDown(float scroll)
        {
            var transformed = _contentRectTransform.anchoredPosition + new Vector2(0, scroll);
            _contentRectTransform.anchoredPosition = transformed;

            if (scrollRect.verticalNormalizedPosition <= 0)
            {
                if(OnScrollEnd != null)
                {
                    OnScrollEnd();
                }
            }
        }
    }
}
