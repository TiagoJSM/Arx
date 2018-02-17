using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Stone_Goblin.Scripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MeleeEnemyController))]
    [RequireComponent(typeof(StoneGoblinController))]
    [RequireComponent(typeof(CombatModule))]
    public class StoneGoblinAnimationController : MonoBehaviour
    {
        private readonly int _HorizontalVelocity = Animator.StringToHash("Velocity");
        private readonly int _AttackType = Animator.StringToHash("Attack Type");
        private readonly int _DeathFront = Animator.StringToHash("Death front");
        private readonly int GainingMomentum = Animator.StringToHash("Gaining Momentum");
        private readonly int Rolling = Animator.StringToHash("Rolling");

        private Animator _animator;
        private MeleeEnemyController _controller;
        private StoneGoblinController _stoneGoblin;
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
        private bool DeathFront
        {
            set
            {
                _animator.SetBool(_DeathFront, value);
            }
        }

        public bool IsCurrentAnimationOver
        {
            get
            {
                var state = _animator.GetCurrentAnimatorStateInfo(0);
                if (state.normalizedTime >= 1)
                {
                    //Debug.Log(state.normalizedTime);
                }

                return state.normalizedTime >= 1;
            }
        }

        // Use this for initialization
        void Awake()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<MeleeEnemyController>();
            _stoneGoblin = GetComponent<StoneGoblinController>();
            _combatModule = GetComponent<CombatModule>();
        }

        void Update()
        {
            HorizontalVelocity = _controller.HorizontalSpeed;
            AttackType = (int)_combatModule.AttackType;
            DeathFront = _controller.Dead;
            _animator.SetBool(GainingMomentum, _stoneGoblin.GainingMomentum);
            _animator.SetBool(Rolling, _stoneGoblin.Rolling);
        }
    }
}
