using GenericComponents.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents
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
            if(position != null)
            {
                obj.transform.position = position.Value;
            }
        }
    }
}
