using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using ArxGame.Components;
using GenericComponents.Helpers;

public class AttackFinishBehaviour : StateMachineBehaviour
{
    private bool _notified;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _notified = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 1 && !_notified)
        {
            animator.gameObject.GetComponent<CloseCombatBehaviour>().NotifyAttackFinish();
            _notified = true;
        }
    }
}
