using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;

namespace Assets.Standard_Assets.Environment.Hazards.Runnaway_Ball.Scripts
{
    public class RotatingBallStateBehaviour : StateMachineBehaviour
    {
        private Vector3 _previousPosition;
        private readonly int _rotation = Animator.StringToHash("Rotation");

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            var movement = animator.transform.position - _previousPosition;
            var distance = Vector3.Distance(_previousPosition, animator.transform.position);
            var speed = movement.x * Time.deltaTime;
            var absX = Math.Abs(movement.x);
            Debug.Log(speed);
            animator.SetFloat(_rotation, speed);
            _previousPosition = animator.transform.position;
        }
    }
}
