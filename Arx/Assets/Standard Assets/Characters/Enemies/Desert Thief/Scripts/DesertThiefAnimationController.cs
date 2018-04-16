using CommonInterfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Desert_Thief.Scripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MeleeEnemyController))]
    [RequireComponent(typeof(DesertThiefEnemyAiControl))]
    [RequireComponent(typeof(CombatModule))]
    public class DesertThiefAnimationController : MonoBehaviour
    {
        private readonly int _HorizontalVelocity = Animator.StringToHash("Velocity");
        private readonly int _AttackType = Animator.StringToHash("Attack Type");
        private readonly int _DeathFront = Animator.StringToHash("Death front");
        private readonly int _DeathBehind = Animator.StringToHash("Death behind");
        private readonly int _Attacked = Animator.StringToHash("Attacked");
        private readonly int _ThrowDagger = Animator.StringToHash("Throw Dagger");
        private readonly int Roll = Animator.StringToHash("Roll");
        private readonly int RollingState = Animator.StringToHash("Base Layer.Roll");

        private Animator _animator;
        private MeleeEnemyController _controller;
        private DesertThiefEnemyAiControl _aiController;
        private CombatModule _combatModule;

        public float HorizontalVelocity
        {
            set
            {
                _animator.SetFloat(_HorizontalVelocity, Mathf.Abs(value));
            }
        }
        private int AttackType
        {
            set
            {
                _animator.SetInteger(_AttackType, value);
            }
        }
        private bool DeathBehind
        {
            set
            {
                _animator.SetBool(_DeathBehind, value);
            }
        }
        private bool DeathFront
        {
            set
            {
                _animator.SetBool(_DeathFront, value);
            }
        }
        private bool Attacked
        {
            set
            {
                _animator.SetBool(_Attacked, value);
            }
        }

        public bool IsCurrentAnimationOver
        {
            get
            {
                var state = _animator.GetCurrentAnimatorStateInfo(0);
                return state.normalizedTime >= 1;
            }
        }

        // Use this for initialization
        void Awake()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<MeleeEnemyController>();
            _aiController = GetComponent<DesertThiefEnemyAiControl>();
            _combatModule = GetComponent<CombatModule>();
        }

        void Update()
        {
            var hitFromTheFront = false;
            if (_controller.Direction == Direction.Right)
            {
                hitFromTheFront = _controller.LastHitDirection > 0;
            }
            else
            {
                hitFromTheFront = _controller.LastHitDirection < 0;
            }
            HorizontalVelocity = _controller.HorizontalSpeed;
            AttackType = (int)_combatModule.AttackType;
            DeathFront = _controller.Dead && hitFromTheFront;
            DeathBehind = _controller.Dead && !hitFromTheFront;
            Attacked = _aiController.Attacked;
            _animator.SetBool(_ThrowDagger, _aiController.ThrowDagger);
            _animator.SetBool(Roll, _aiController.Dodging);

            var currentState = _animator.GetCurrentAnimatorStateInfo(0);
            if (_animator.GetCurrentAnimatorClipInfo(0).Length > 0)
            {
                var c = _animator.GetCurrentAnimatorClipInfo(0)[0];
                if (currentState.fullPathHash == RollingState)
                {
                    _animator.speed = c.clip.length / _aiController.DodgeDuration;
                }
                else
                {
                    _animator.speed = 1;
                }
            }
            else
            {
                _animator.speed = 1;
            }

        }

        private void ThrowDaggerHandler()
        {
            
        }
    }
}
