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
    public class CombatHitEffects : MonoBehaviour
    {
        [SerializeField]
        private float _shakeAmount = 0.3f;
        [SerializeField]
        private float _shakeDuration = 0.2f;
        [SerializeField]
        private float _slowDownTime = 0.08f;
        [SerializeField]
        private float _strongShakeAmount = 0.5f;
        [SerializeField]
        private float _strongShakeDuration = 0.3f;
        [SerializeField]
        private float _longSlowDownTime = 0.2f;
        [SerializeField]
        private GameObject _hitParticle;

        public void EnemyHit()
        {
            ShakeCamera(_shakeAmount, _shakeDuration);
            StartCoroutine(SlowDownTime(_slowDownTime));
        }

        public void EnemyStrongHit()
        {
            ShakeCamera(_strongShakeAmount, _strongShakeDuration);
            StartCoroutine(SlowDownTime(_longSlowDownTime));
        }

        public void HitByEnemy()
        {
            ShakeCamera(_strongShakeAmount, _strongShakeDuration);
            StartCoroutine(SlowDownTime(_longSlowDownTime, 0.3f));
        }

        public void ShowHitParticle(Vector3 position, float positionVariation = 2f)
        {
            Vector3 variation = UnityEngine.Random.insideUnitCircle* positionVariation;
            var instantiated = Instantiate(_hitParticle, position + variation, Quaternion.identity);
            Destroy(instantiated, 1.0f);
        }

        private void ShakeCamera(float shakeAmount, float shakeDuration)
        {
            var camShake = Camera.main.GetComponent<CameraShake>();
            if(camShake != null)
            {
                camShake.ShakeCamera(shakeAmount, shakeDuration);
            }
        }

        private IEnumerator SlowDownTime(float slowDownTime, float timeScale = 0.0f)
        {
            Time.timeScale = timeScale;
            yield return new WaitForSecondsRealtime(slowDownTime);
            Time.timeScale = 1.0f;
        }
    }
}
