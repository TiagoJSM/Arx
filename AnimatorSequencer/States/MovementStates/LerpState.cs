using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.MovementStates
{
    [Serializable]
    public class LerpState : BaseSequenceState
    {
        private Vector3 _startPosition;

        [SerializeField]
        public Transform moveToPosition;
        [SerializeField]
        public Transform target;
        [SerializeField]
        public float time = 1;
        [SerializeField]
        public float distanceThreshold = 0.01f;
        [SerializeField]
        public bool ignoreZ = true;

        protected override void PerformOnStateEnter()
        {
            _startPosition = target.position;
        }

        protected override void PerformOnStateUpdate()
        {
            var elapsed = Time.time - StateEnterTime;
            var elapsedPercentage = elapsed / time;
            var result = Vector3.Lerp(_startPosition, moveToPosition.position, elapsedPercentage);
            if (ignoreZ)
            {
                result.z = target.position.z;
            }
            target.position = result;
        }

        public override bool Complete()
        {
            var distance = default(float);
            if (ignoreZ)
            {
                distance = Vector2.Distance(moveToPosition.position.ToVector2(), target.position.ToVector2());
            }
            else
            {
                distance = Vector3.Distance(moveToPosition.position, target.position);
            }
            return distance < distanceThreshold;
        }
    }
}
