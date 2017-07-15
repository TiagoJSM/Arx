using Assets.Standard_Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Guardian_Eye.Scripts
{
    public class GuardianEyeRoom : MonoBehaviour
    {
        private bool _activated;

        [SerializeField]
        private GameObject[] _walls;
        [SerializeField]
        private GuardianEye _guardianEye;

        private void Awake()
        {
            _guardianEye.gameObject.SetActive(false);
            SetWallsActive(false);
            _guardianEye.OnKilled += OnKilledHandler;
        }

        private void OnKilledHandler(GuardianEye character)
        {
            SetWallsActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_activated)
            {
                return;
            }
            if (collision.IsPlayer())
            {
                SetWallsActive(true);
                _guardianEye.gameObject.SetActive(true);
                _activated = true;
            }
        }

        private void SetWallsActive(bool active)
        {
            for (var idx = 0; idx < _walls.Length; idx++)
            {
                _walls[idx].SetActive(active);
            }
        }
    }
}
