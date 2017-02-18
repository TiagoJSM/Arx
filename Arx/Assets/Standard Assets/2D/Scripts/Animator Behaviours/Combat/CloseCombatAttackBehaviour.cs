﻿using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using ArxGame.Components;
using GenericComponents.Helpers;
using GenericComponents.Enums;

public class CloseCombatAttackBehaviour : StateMachineBehaviour
{
    private bool _notified;

    [SerializeField]
    private AttackType _attackType;
    [SerializeField]
    private AttackStyle _attackStyle;
    [SerializeField]
    private int _combo;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.gameObject.GetComponent<CloseCombatBehaviour>().NotifyAttackStart(_attackType, _attackStyle, _combo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.gameObject.GetComponent<CloseCombatBehaviour>().NotifyAttackFinish();
    }
}
