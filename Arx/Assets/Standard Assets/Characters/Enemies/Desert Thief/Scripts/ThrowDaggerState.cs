using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Characters.Enemies.Desert_Thief.Scripts
{
    public class ThrowDaggerState : BaseAiState<DesertThiefEnemyAiControl>
    {
        public override void OnStateEnter(object action)
        {
            base.OnStateEnter(action);
            base.StateController.ThrowDaggerAtEnemy();
        }

        public override void OnStateExit(object action)
        {
            base.StateController.ThrowDagger = false;
        }
    }
}
