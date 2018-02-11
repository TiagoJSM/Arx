using Assets.Standard_Assets._2D.Scripts.Characters;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets._2D.Scripts.Hazards;
using Assets.Standard_Assets.Characters.CharacterBehaviour;
using CommonInterfaces.Enums;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Stone_Goblin.Scripts
{
    [RequireComponent(typeof(CharacterStatus))]
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(DamageOnTouch))]
    public class StoneGoblinController : MonoBehaviour
    {
        private float _move;
        private bool _rollAttack;
        private CharacterStatus _characterStatus;
        private CharacterController2D _characterController2D;
        private DamageOnTouch _damageOnTouch;

        [SerializeField]
        private float _rollSpeed = 30;
        [SerializeField]
        private float _gainRollMomentumDuration = 4;

        public bool GainingMomentum { get; private set; }
        public bool Rolling { get; private set; }

        public void RollAttack(Direction direction)
        {
            StartCoroutine(RollAttackRoutine(direction));
        }

        public void StopRollAttack()
        {
            _characterStatus.Immortal = false;
        }

        private IEnumerator RollAttackRoutine(Direction direction)
        {
            GainingMomentum = true;
            Rolling = true;
            yield return new WaitForSeconds(_gainRollMomentumDuration/2);
            _damageOnTouch.enabled = true;
            yield return new WaitForSeconds(_gainRollMomentumDuration/2);
            GainingMomentum = false;

            var elapsed = 0.0f;

            while(elapsed < 2)
            {
                elapsed += Time.deltaTime;
                _characterController2D.move(
                    new Vector3(
                        _rollSpeed * Time.deltaTime * direction.DirectionValue(), 0.0f, 0.0f));
                yield return null;
            }

            Rolling = false;
            _damageOnTouch.enabled = false;
        }

        private void Awake()
        {
            _characterStatus = GetComponent<CharacterStatus>();
            _characterController2D = GetComponent<CharacterController2D>();
            _damageOnTouch = GetComponent<DamageOnTouch>();
        }
    }
}
