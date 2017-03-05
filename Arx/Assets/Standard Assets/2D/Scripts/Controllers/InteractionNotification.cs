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

        public void ShowInteraction()
        {
            Show(InputManager.Instance.GetInputDevice().InteractSprite);
        }

        public void ShowTeleporter()
        {
            Show(InputManager.Instance.GetInputDevice().Up);
        }

        public void HideInteraction()
        {
            _animator.SetBool(SHOW, false);
        }

        public void HideTeleporter()
        {
            _animator.SetBool(SHOW, false);
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

        private void Show(Sprite sprite)
        {
            if (_animator.GetBool(SHOW))
            {
                return;
            }
            _interactionButton.sprite = sprite;
            _animator.SetBool(SHOW, true);
        }
    }
}
