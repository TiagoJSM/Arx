using Assets.Standard_Assets._2D.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Teleporters
{
    public class TeleporterSpawner : MonoBehaviour
    {
        public GameObject obj;

        void Start()
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            var position = CheckpointManager.Instance.CurrentCheckpointPosition;
            if (position != null)
            {
                obj.transform.position = position.Value;
            }
        }
    }
}
