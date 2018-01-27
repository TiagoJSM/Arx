using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Platforms.Ladder.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class LadderDropOnGrab : LadderBehaviour
    {
        private readonly int Collapse = Animator.StringToHash("Collapse");

        private Animator _animator;
        private bool _awaitPlayer = true;

        public void ResetPlatform()
        {
            _awaitPlayer = true;
        }

        public override void OnLadderGrab()
        {
            if (_awaitPlayer)
            {
                _awaitPlayer = false;
                _animator.SetTrigger(Collapse);
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
    }
}
