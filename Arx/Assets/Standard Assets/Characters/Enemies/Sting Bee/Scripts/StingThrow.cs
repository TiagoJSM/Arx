using Assets.Standard_Assets.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Sting_Bee.Scripts
{
    public class StingThrow : MonoBehaviour
    {
        private GameObject _target;

        [SerializeField]
        private Projectile _projectile;
        [SerializeField]
        private Transform _projectileOrigin;
        [SerializeField]
        private float _yOffset = 4f;

        public bool PreparingStingThrow { get; private set; }

        public IEnumerator StartStingThrow(GameObject target)
        {
            if (!PreparingStingThrow)
            {
                _target = target;
                PreparingStingThrow = true;
                yield return new WaitWhile(() => PreparingStingThrow);
            }
        }

        public void StopStingThrow()
        {
            PreparingStingThrow = false;
        }

        public void ThrowSting()
        {
            var projectile = Instantiate(_projectile, _projectileOrigin.position, Quaternion.identity);
            projectile.Direction = (_target.transform.position + new Vector3(0, _yOffset) - transform.position).normalized;
            StopStingThrow();
        }
    }
}
