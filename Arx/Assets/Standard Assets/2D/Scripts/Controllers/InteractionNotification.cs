using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    public class InteractionNotification : MonoBehaviour
    {
        private const string SHOW = "Show";

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private SpriteRenderer _interactionButton;

        public void Show()
        {
            _animator.SetBool(SHOW, true);
        }

        public void Hide()
        {
            _animator.SetBool(SHOW, false);
        }

        private void Start()
        {
            _interactionButton.sprite = InputManager.Instance.GetInputDevice().InteractSprite;
        }

        private void LateUpdate()
        {
            var globalScale = transform.lossyScale;
            if (globalScale.x < 0)
            {
                var localScale = transform.localScale;
                transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            }
        }
    }
}
