using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Enemies.Melee_enemy
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MeleeEnemyController))]
    public class MeleeEnemyAnimationController : MonoBehaviour
    {
        private readonly int _HorizontalVelocity = Animator.StringToHash("Velocity");
        private readonly int _Attacking = Animator.StringToHash("Attacking");

        private Animator _animator;
        private MeleeEnemyController _controller;

        public float HorizontalVelocity
        {
            set
            {
                _animator.SetFloat(_HorizontalVelocity, Mathf.Abs(value));
            }
        }
        private bool Attacking
        {
            set
            {
                _animator.SetBool(_Attacking, value);
            }
        }

        public bool IsCurrentAnimationOver
        {
            get
            {
                var clip = _animator.GetCurrentAnimatorClipInfo(0);
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
        }

        void Update()
        {
            HorizontalVelocity = _controller.HorizontalSpeed;
            Attacking = _controller.Attacking;
        }
    }
}
