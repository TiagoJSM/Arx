using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Animator_Behaviours.Combat
{
    public class GroundCloseCombatStateMachineBehaviour : StateMachineBehaviour
    {
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            base.OnStateMachineEnter(animator, stateMachinePathHash);
            animator.gameObject.GetComponent<CloseCombatBehaviour>().NotifyEnterCombatState();
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            base.OnStateMachineExit(animator, stateMachinePathHash);
            animator.gameObject.GetComponent<CloseCombatBehaviour>().NotifyCombatFinish();
        }
    }
}
