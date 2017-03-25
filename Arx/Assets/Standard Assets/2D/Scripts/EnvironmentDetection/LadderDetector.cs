using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.EnvironmentDetection
{
    public class LadderDetector : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _ladderLayer;

        public GameObject LadderGameObject { get; private set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_ladderLayer.IsInAnyLayer(collision.gameObject))
            {
                return;
            }
            LadderGameObject = collision.gameObject;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!_ladderLayer.IsInAnyLayer(collision.gameObject))
            {
                return;
            }
            LadderGameObject = null;
        }
    }
}
