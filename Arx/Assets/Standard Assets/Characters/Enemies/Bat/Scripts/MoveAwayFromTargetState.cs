using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Characters.Enemies.Bat.Scripts
{
    public class MoveAwayFromTargetState : BaseAiState<BatCharacterAiController>
    {
        public override void OnStateEnter(object action)
        {
            base.OnStateEnter(action);
            StateController.MoveAwayFromTarget();
        }
    }
}
