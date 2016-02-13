using GenericComponents.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Factories
{
    public class PlayerSpawner : MonoBehaviour
    {
        public GameObject player;

        void Awake()
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            var position = CheckpointManager.Instance.CurrentCheckpointPosition;
            var spawnPoint = position == null ? this.transform.position : position.Value;
            Instantiate(player, spawnPoint, Quaternion.identity);
        }
    }
}
