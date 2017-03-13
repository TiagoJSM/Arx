using GenericComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.Speech.Scripts
{
    public class SpeechBubleController : MonoBehaviour
    {
        private readonly int OpenSpeechBubble = Animator.StringToHash("Open Speech Bubble");
        private readonly int CloseSpeechBubble = Animator.StringToHash("Close Speech Bubble");

        [SerializeField]
        private bool _visible;
        [SerializeField]
        private RectTransform _speechBubble;
        [SerializeField]
        private ScrollRect _scrollRect;
        private RectTransform _content;
        [SerializeField]
        private Text _nameText;
        [SerializeField]
        private Image _continueConversationButton;
        [SerializeField]
        private Text _continueConversation;

        [SerializeField]
        private Animator _speechAnimator;
        [SerializeField]
        [TextArea(3, 10)]
        private string _text;
        [SerializeField]
        private Text _textUi;

        public OnScrollEnd OnScrollEnd;
        public OnVisibilityChange OnVisibilityChange;

        public string Name
        {
            get
            {
                return _nameText.text;
            }
            set
            {
                _nameText.text = value;
            }
        }

        public string Text
        {
            get
            {
                return _textUi.text;
            }
            set
            {
                _textUi.text = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    OnVisibleChange();
                    if(OnVisibilityChange != null)
                    {
                        OnVisibilityChange(_visible);
                    }
                }
            }
        }

        public bool SpeechEnded
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

        public void ResetSpeech()
        {
            _content.localPosition = new Vector3();
            if (SpeechEnded)
            {
                SetCloseConversationMessage();
            }
            else
            {
                SetContinueConversationMessage();
            }
        }

        public bool ScrollPageDown()
        {
            if (SpeechEnded)
            {
                if (OnScrollEnd != null)
                {
                    OnScrollEnd();
                }
                Close();
                return true;
            }
            //ToDo: scroll should use coroutine to make it smooth
            var speechBubbleHeight = _speechBubble.rect.height;
            ScrollDown(speechBubbleHeight);
            if (SpeechEnded)
            {
                SetCloseConversationMessage();
            }
            return false;
        }

        public void ScrollDown(float scroll)
        {
            var transformed = _content.anchoredPosition + new Vector2(0, scroll);
            _content.anchoredPosition = transformed;
        }

        public bool Continue()
        {
            return ScrollPageDown();
        }

        public void Say(string text)
        {
            ResetSpeech();
            Text = text;
            Visible = true;
        }

        public void Close()
        {
            Text = string.Empty;
            Visible = false;
        }

        protected void OnVisibleChange()
        {
            var trigger = Visible ? OpenSpeechBubble : CloseSpeechBubble;
            _speechAnimator.SetTrigger(trigger);
        }

        private void LateUpdate()
        {
            var globalScale = _speechBubble.transform.lossyScale;
            if (globalScale.x < 0)
            {
                var localScale = _speechBubble.transform.localScale;
                _speechBubble.transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            }
        }

        private void Start()
        {
            _content = _scrollRect.content;
            _content.localPosition = new Vector3();
            Text = _text;
            _nameText.text = string.Empty;
        }

        private void SetContinueConversationMessage()
        {
            _continueConversation.text = "Continue";
        }
        private void SetCloseConversationMessage()
        {
            _continueConversation.text = "Close";
        }
    }
}
