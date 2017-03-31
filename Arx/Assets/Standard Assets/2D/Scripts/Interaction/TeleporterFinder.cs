using Assets.Standard_Assets._2D.Scripts.Characters.Arx;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Interaction
{
    public class TeleporterFinder : MonoBehaviour
    {
        private ITeleporter _currentTeleporter;

        [SerializeField]
        private MainCharacterNotification _notification;

        public ITeleporter FindTeleporter()
        {
            var collider =
                Physics2D
                    .OverlapPointAll(this.transform.position)
                    .FirstOrDefault(c => c.GetComponent<ITeleporter>() != null);

            if (collider == null)
            {
                return null;
            }

            return collider.GetComponent<ITeleporter>();
        }

        private void Update()
        {
            var teleporter = FindTeleporter();

            if (_currentTeleporter != null && teleporter == null)
            {
                _currentTeleporter = null;
                _notification.HideInteraction();
            }
            else if(teleporter != null && (_currentTeleporter == null || teleporter != _currentTeleporter))
            {
                _currentTeleporter = teleporter;
                _notification.ShowTeleporter();
            }
        }
    }
}
