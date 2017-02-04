using Assets.Standard_Assets._2D.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Spawners
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
