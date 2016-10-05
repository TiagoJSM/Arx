using ArxGame.Components.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChainThrowStateBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.gameObject.GetComponent<ChainThrowCombatBehaviour>().PerformingChainThrowAnimation(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.gameObject.GetComponent<ChainThrowCombatBehaviour>().PerformingChainThrowAnimation(false);
    }

}
