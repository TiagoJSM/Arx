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

        protected override void PerformOnStateEnter()
        {
            _startPosition = target.position;
        }

        protected override void PerformOnStateUpdate()
        {
            var elapsed = Time.time - StateEnterTime;
            var elapsedPercentage = elapsed / time;
            target.position = Vector3.Lerp(_startPosition, moveToPosition.position, elapsedPercentage);
        }
    }
}
