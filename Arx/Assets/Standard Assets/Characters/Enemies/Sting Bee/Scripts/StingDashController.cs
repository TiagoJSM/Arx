using Assets.Standard_Assets._2D.Scripts.Hazards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Sting_Bee.Scripts
{
    [RequireComponent(typeof(CharacterController2D))]
    public class StingDashController : MonoBehaviour
    {
        private CharacterController2D _character;

        [SerializeField]
        private float _dashSpeed = 36.0f;
        [SerializeField]
        private DamageOnTouch _damageOnTouch;
        [SerializeField]
        private float _yOffset = 0.5f;
        [SerializeField]
        private float _dashFinalPositionOffset = 5f;

        public IEnumerator StingDash(GameObject target)
        {
            var direction = (target.transform.position + new Vector3(0, _yOffset) - transform.position).normalized;
            var dashFinalPosition = (target.transform.position + new Vector3(0, _yOffset)) + (_dashFinalPositionOffset * direction);
            return DashRoutine(dashFinalPosition);
        }

        private void Awake()
        {
            _character = GetComponent<CharacterController2D>();
            _damageOnTouch.Active = false;
        }

        private IEnumerator DashRoutine(Vector3 position)
        {
            _damageOnTouch.Active = true;
            var duration = Vector3.Distance(position, transform.position) / _dashSpeed;
            var startPosition = transform.position;
            var direction = (position - transform.position).normalized;
            var t = 0.0f;

            while (!transform.position.ToVector2().Approximately(position) && t < duration)
            {
                _character.move(direction * _dashSpeed * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }
            _damageOnTouch.Active = false;
        }
    }
}
