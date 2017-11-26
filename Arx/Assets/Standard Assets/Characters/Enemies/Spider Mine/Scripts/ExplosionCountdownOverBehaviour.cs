using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Spider_Mine.Scripts
{
    public class ExplosionCountdownOverBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            var spiderMine = animator.GetComponent<SpiderMineAi>();
            spiderMine.CountdownOver = true;
        }
    }
}
