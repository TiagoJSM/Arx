using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.MovementStates
{
    [Serializable]
    public class SmoothDumpState : BaseSequenceState
    {
        private float _maxSpeed;

        [SerializeField]
        public Transform moveToPosition;
        [SerializeField]
        public Transform target;
        [SerializeField]
        public float time = 1;
        [SerializeField]
        public float distanceThreshold = 0.01f;

        private Vector3 _velocity;

        public SmoothDumpState()
        {
            _maxSpeed = Mathf.Infinity;
        }

        protected override void PerformOnStateEnter()
        {
            //animator.applyRootMotion = true;
        }

        protected override void PerformOnStateUpdate()
        {
            target.position = Vector3.SmoothDamp(target.position, moveToPosition.position, ref _velocity, time, _maxSpeed);
        }

        public override bool Complete()
        {
            var distance = Vector3.Distance(moveToPosition.position, target.position);
            return distance < distanceThreshold;
        }
    }
}
