using Assets.Standard_Assets._2D.Scripts.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx
{
    public class MainCharacterNotification : MonoBehaviour
    {
        [SerializeField]
        private InteractionNotification _notification;

        public void ShowInteraction()
        {
            _notification.Show(InputManager.Instance.GetInputDevice().InteractSprite);
        }

        public void ShowTeleporter()
        {
            _notification.Show(InputManager.Instance.GetInputDevice().Up);
        }

        public void HideInteraction()
        {
            _notification.Hide();
        }

        public void HideTeleporter()
        {
            _notification.Hide();
        }
    }
}
