using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Desert_Thief.Scripts
{
    public class DaggerThrownBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            var desertThief = animator.GetComponent<DesertThiefEnemyAiControl>();
            desertThief.ThrowDagger = false;
        }
    }

}
