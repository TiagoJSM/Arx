using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Canyon_Engineer.Scripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MeleeEnemyController))]
    [RequireComponent(typeof(EngineerEnemyAiControl))]
    [RequireComponent(typeof(CombatModule))]
    public class EngineerAnimationController : MonoBehaviour
    {
        private readonly int _HorizontalVelocity = Animator.StringToHash("Velocity");
        private readonly int _AttackType = Animator.StringToHash("Attack Type");
        private readonly int _DeathFront = Animator.StringToHash("Death front");
        private readonly int _Surprised = Animator.StringToHash("Surprised");

        private Animator _animator;
        private MeleeEnemyController _controller;
        private EngineerEnemyAiControl _aiController;
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
        private bool Surprised
        {
            set
            {
                _animator.SetTrigger(_Surprised);
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
            _aiController = GetComponent<EngineerEnemyAiControl>();
            _combatModule = GetComponent<CombatModule>();
            _aiController.OnSurprised += ShowSurprised;
        }

        void Update()
        {
            HorizontalVelocity = _controller.HorizontalSpeed;
            AttackType = (int)_combatModule.ComboType;
            DeathFront = _controller.Dead;
        }

        private void OnDestroy()
        {
            _aiController.OnSurprised -= ShowSurprised;
        }

        private void ShowSurprised()
        {
            Surprised = true;
        }
    }
}
