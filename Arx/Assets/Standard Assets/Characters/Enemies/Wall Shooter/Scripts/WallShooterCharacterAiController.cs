using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Wall_Shooter.Scripts
{
    [RequireComponent(typeof(WallShooterCharacterController))]
    public class WallShooterCharacterAiController : MonoBehaviour
    {
        private WallShooterCharacterController _character;

        [SerializeField]
        [Range(0, 10)]
        private float shootIntervalInSeconds = 1;
        [SerializeField]
        [Range(1, 10)]
        private int roundsPerBurst = 1;
        [SerializeField]
        [Range(0, 10)]
        private float burstRoundDelayInSeconds = 0.4f;

        private void Awake()
        {
            _character = GetComponent<WallShooterCharacterController>();
            StartCoroutine(ShootCoroutine());
        }

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                yield return ShootBurst();
                yield return new WaitForSeconds(shootIntervalInSeconds);
            }
        }

        private IEnumerator ShootBurst()
        {
            var remainingRounds = roundsPerBurst;

            do
            {
                _character.Shoot();
                if(remainingRounds == 1)
                {
                    yield break;
                }
                yield return new WaitForSeconds(burstRoundDelayInSeconds);
                --remainingRounds;
            } while (true);
        }
    }
}
