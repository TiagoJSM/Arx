using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Geyser.Scripts
{
    public class GeyserController : MonoBehaviour
    {
        [SerializeField]
        private float _preparationTime = 3;
        [SerializeField]
        private float _warningTime = 1;
        [SerializeField]
        private float _shootingTime = 1;
        [SerializeField]
        private Collider2D _damageCollider;
        [SerializeField]
        private ParticleSystem _geyserWarning;
        [SerializeField]
        private ParticleSystem _geyserShooting;
        [SerializeField]
        private float _delay;

        private void Awake()
        {
            _damageCollider.enabled = false;
            _geyserShooting.Stop();
            _geyserWarning.Stop();
            StartCoroutine(GeyserRoutine());
        }

        private IEnumerator GeyserRoutine()
        {
            if (_delay > 0)
            {
                yield return new WaitForSeconds(_delay);
            }
            while (true)
            {
                yield return PrepareGeyser();
                yield return GeyserShotWarning();
                yield return GeyserShooting();
            }
        }

        private YieldInstruction PrepareGeyser()
        {
            _geyserShooting.Stop();
            _damageCollider.enabled = false;
            return new WaitForSeconds(_preparationTime);
        }

        private YieldInstruction GeyserShotWarning()
        {
            _geyserWarning.Play();
            return new WaitForSeconds(_warningTime);
        }

        private YieldInstruction GeyserShooting()
        {
            _geyserWarning.Stop();
            _geyserShooting.Play();
            _damageCollider.enabled = true;
            return new WaitForSeconds(_shootingTime);
        }
    }
}
