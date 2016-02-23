using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorSequencer.Zones
{
    public class PlaySequenceOnPlayerTrigger : MonoBehaviour
    {
        private bool played = false;

        public Animator animator;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (played)
            {
                return;
            }

            var controller = animator.runtimeAnimatorController as AnimatorController;
            if (controller == null)
            {
                Debug.LogError("Missing controller");
                return;
            }
            var baseLayer = controller.layers[0];
            var editorBehaviours =
                    baseLayer
                        .stateMachine
                        .states
                        .SelectMany(animStates => animStates.state.behaviours)
                        .ToArray();

            Debug.Log(editorBehaviours.First().GetType() + " " + editorBehaviours.First().GetInstanceID());
            animator.enabled = true;
            animator.Play(0);
            played = true;
        }
    }
}
