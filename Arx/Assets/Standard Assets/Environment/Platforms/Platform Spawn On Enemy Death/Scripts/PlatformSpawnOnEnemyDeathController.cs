using Assets.Standard_Assets._2D.Scripts.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Platforms.Platform_Spawn_On_Enemy_Death.Scripts
{
    public class PlatformSpawnOnEnemyDeathController : MonoBehaviour
    {
        private int _enemiesAlive;

        [SerializeField]
        private BasePlatformerController[] _characters;
        [SerializeField]
        private GameObject[] _platforms;
        [SerializeField]
        private float _spawnInterval = 0.25f;

        private void Start()
        {
            _enemiesAlive = _characters.Length;
            for (var idx = 0; idx < _characters.Length; idx++)
            {
                _characters[idx].OnKilled += OnKilledHandler;
            }
            for (var idx = 0; idx < _platforms.Length; idx++)
            {
                _platforms[idx].SetActive(false);
            }
        }

        private void OnKilledHandler(BasePlatformerController character)
        {
            character.OnKilled -= OnKilledHandler;
            _enemiesAlive--;
            if (_enemiesAlive == 0)
            {
                StartCoroutine(SpawnRoutine());
            }
        }

        private IEnumerator SpawnRoutine()
        {
            for(var idx = 0; idx < _platforms.Length; idx++)
            {
                _platforms[idx].SetActive(true);
                yield return new WaitForSeconds(_spawnInterval);
            }
        }
    }
}
