using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Teleports.Teleport_On_Use.Scripts
{
    public class TeleportOnUseNotification : MonoBehaviour
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
            _interactionButton.sprite = InputManager.Instance.GetInputDevice().Up;
        }
    }
}
