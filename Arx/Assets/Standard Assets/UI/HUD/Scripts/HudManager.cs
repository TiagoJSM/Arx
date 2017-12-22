using Assets.Standard_Assets.QuestSystem.QuestStructures;
using GenericComponents.Behaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.HUD.Scripts
{
    public class HudManager : MonoBehaviour
    {
        private Coroutine _currentToast;
        private Queue<IEnumerator> _toastQueue;

        [SerializeField]
        private QuestSectionManager _questSection;
        [SerializeField]
        private CharacterStatusManager _characterStatus;

        public GameObject toastPanel;
        public Text toastText;

        public float Short
        {
            get
            {
                return 3;
            }
        }
        public float Medium
        {
            get
            {
                return 5;
            }
        }
        public float Long
        {
            get
            {
                return 7;
            }
        }

        public Quest ActiveQuest
        {
            get
            {
                return _questSection.ActiveQuest;
            }
            set
            {
                _questSection.ActiveQuest = value;
            }
        }

        public CharacterStatus CharacterStatus
        {
            get
            {
                return _characterStatus.CharacterStatus;
            }
            set
            {
                _characterStatus.CharacterStatus = value;
            }
        }

        public void Awake()
        {
            _toastQueue = new Queue<IEnumerator>();
            toastPanel.SetActive(false);
        }

        public void Toast(string text, float duration)
        {
            if (_currentToast == null)
            {
                _currentToast = StartCoroutine(ToastCoroutine(text, duration));
            }
            _toastQueue.Enqueue(ToastCoroutine(text, duration));
        }

        private IEnumerator ToastCoroutine(string text, float duration)
        {
            toastText.text = text;
            toastPanel.SetActive(true);
            yield return new WaitForSeconds(duration);
            if (!_toastQueue.Any())
            {
                toastPanel.SetActive(false);
                toastText.text = string.Empty;
                _currentToast = null;
                yield break;
            }
            var next = _toastQueue.Dequeue();
            _currentToast = StartCoroutine(next);
        }
    }
}
