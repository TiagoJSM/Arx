using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Characters.Enemies.Stone_Goblin.Scripts
{
    public class RollAttackState : BaseAiState<StoneGoblinAiController>
    {
        public override void OnStateEnter(object action)
        {
            base.OnStateEnter(action);
            StateController.RollAttack();
        }
    }
}
