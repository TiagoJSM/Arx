using Assets.Standard_Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Platforms.Timed_Collapsing.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class CollapsingPlatformController : MonoBehaviour
    {
        private readonly int Collapse = Animator.StringToHash("Collapse");

        private Animator _animator;
        private bool _awaitPlayer = true;

        public void ResetPlatform()
        {
            _awaitPlayer = true;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_awaitPlayer && collision.gameObject.IsPlayer())
            {
                _animator.SetTrigger(Collapse);
                _awaitPlayer = false;
            }
        }
    }
}
