using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.States.MovementStates
{
    [Serializable]
    public class TimedRotationState : BaseSequenceState
    {
        [SerializeField]
        public Transform target;
        [SerializeField]
        public float time = 1;
        [SerializeField]
        public float rotationValuePerSecond = 1;

        protected override void PerformOnStateUpdate()
        {
            var rotation = rotationValuePerSecond * ElapsedTimeSinceLastUpdate;
            var eulerAngle = target.rotation.eulerAngles;
            eulerAngle.z = eulerAngle.z + rotation;
            target.rotation = Quaternion.Euler(eulerAngle);
        }

        public override bool Complete()
        {
            if (float.IsInfinity(time))
            {
                return false;
            }
            return ElapsedTime > time;
        }
    }
}
