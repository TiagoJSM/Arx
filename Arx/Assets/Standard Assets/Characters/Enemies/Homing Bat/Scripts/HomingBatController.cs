using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Homing_Bat.Scripts
{
    public class HomingBatController : MonoBehaviour, ICharacter
    {
        public bool CanBeAttacked { get { return true; } }
        public GameObject CharacterGameObject { get { return gameObject; } }
        public bool InPain { get { return false; } }
        public int LifePoints { get { return 1; } }
        public int MaxLifePoints { get { return 1; } }

        [SerializeField]
        private float _entranceDelay;
        [SerializeField]
        private float _entranceTime = 0.5f;
        [SerializeField]
        private Transform _entrancePosition;
        [SerializeField]
        private float _focusDelay = 0.5f;
        [SerializeField]
        private float _focusTime = 1.0f;
        [SerializeField]
        private float _homingDuration = 0.5f;

        public int Attacked(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType = AttackTypeDetail.Generic, int comboNumber = 1, bool showDamaged = false)
        {
            Destroy(gameObject);
            return damage;
        }

        public void EndGrappled()
        {
            
        }

        public void Kill()
        {
            
        }

        public bool StartGrappled(GameObject grapple)
        {
            return false;
        }

        private void Awake()
        {
            var target = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG);
            StartCoroutine(BatRoutine(target));
        }

        private IEnumerator BatRoutine(GameObject target)
        {
            yield return new WaitForSeconds(_entranceDelay);
            yield return CoroutineHelpers.MoveTo(transform.position, _entrancePosition.position, _entranceTime, transform);
            yield return new WaitForSeconds(_focusDelay);
            yield return new WaitForSeconds(_focusTime);
            yield return CoroutineHelpers.MoveTo(transform.position, () => target.transform.position, _homingDuration, transform);
            Destroy(gameObject);
        }
    }
}
