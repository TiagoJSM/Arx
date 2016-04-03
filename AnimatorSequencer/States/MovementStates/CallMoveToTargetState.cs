using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Extensions;
using CommonInterfaces.Enums;

namespace AnimatorSequencer.States.MovementStates
{
    [Serializable]
    public class MoveDirectionUnityEvent : UnityEvent<Direction>
    {
    }

    [Serializable]
    public class CallMoveToTargetState : BaseSequenceState
    {
        [SerializeField]
        public Transform moveToPosition;
        [SerializeField]
        public MoveDirectionUnityEvent moveEvent;
        [SerializeField]
        public float distanceThreshold = 1f;
        [SerializeField]
        public bool ignoreZ = true;

        protected override void PerformOnStateFixedUpdate()
        {
            var target = moveEvent.GetPersistentTarget(0) as Component;
            if (target == null)
            {
                return;
            }
            var targetTransform = target.gameObject.transform;
            var direction = targetTransform.position.x < moveToPosition.position.x ? Direction.Right : Direction.Left;
            moveEvent.Invoke(direction);
        }

        public override bool Complete()
        {
            var distance = default(float);
            var target = moveEvent.GetPersistentTarget(0) as Component;
            if(target == null)
            {
                Debug.LogError("Event target must be a Component");
                return false; ;
            }
            var targetTransform = target.gameObject.transform;
            if (ignoreZ)
            {
                distance = Vector2.Distance(moveToPosition.position.ToVector2(), targetTransform.position.ToVector2());
            }
            else
            {
                distance = Vector3.Distance(moveToPosition.position, targetTransform.position);
            }
            
            return distance < distanceThreshold;
        }
    }
}
