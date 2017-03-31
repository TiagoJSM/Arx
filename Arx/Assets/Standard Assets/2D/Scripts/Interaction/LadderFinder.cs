using Assets.Standard_Assets._2D.Scripts.Characters.Arx;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Interaction
{

    public class LadderFinder : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _ladderLayer;
        [SerializeField]
        private MainCharacterNotification _notification;

        public GameObject LadderGameObject { get; private set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_ladderLayer.IsInAnyLayer(collision.gameObject))
            {
                return;
            }
            _notification.ShowLadder();
            LadderGameObject = collision.gameObject;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!_ladderLayer.IsInAnyLayer(collision.gameObject))
            {
                return;
            }
            _notification.HideInteraction();
            LadderGameObject = null;
        }
    }
}
