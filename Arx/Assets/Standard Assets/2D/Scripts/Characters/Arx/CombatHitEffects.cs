using Assets.Standard_Assets._2D.Cameras.Scripts;
using GenericComponents.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx
{
    [RequireComponent(typeof(CloseCombatBehaviour))]
    public class CombatHitEffects : MonoBehaviour
    {
        private CloseCombatBehaviour _closeCombat;

        [SerializeField]
        private float _amount = 0.5f;
        [SerializeField]
        private float _duration = 0.3f;
        [SerializeField]
        private float _slowDownTime = 0.3f;

        private void Awake()
        {
            _closeCombat = GetComponent<CloseCombatBehaviour>();

            _closeCombat.OnHit += OnHitHandler;
        }

        private void OnHitHandler(AttackType attackType)
        {
            ShakeCamera();
            StartCoroutine(SlowDownTime());
        }

        private void ShakeCamera()
        {
            var camShake = Camera.main.GetComponent<CameraShake>();
            if(camShake != null)
            {
                camShake.ShakeCamera(_amount, _duration);
            }
        }

        private IEnumerator SlowDownTime()
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(_slowDownTime);
            Time.timeScale = 1.0f;
        }
    }
}
