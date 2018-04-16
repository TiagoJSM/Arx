using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Characters.Enemies.Desert_Thief.Scripts
{
    public class DodgeAttackState : BaseAiState<DesertThiefEnemyAiControl>
    {
        private float _opositeDirection;
        public override void OnStateEnter(object action)
        {
            base.OnStateEnter(action);
            base.StateController.StartDodge();
            _opositeDirection = Math.Sign(base.StateController.transform.position.x - base.StateController.Target.transform.position.x);
        }

        public override void Perform(object action)
        {
            base.Perform(action);
            base.StateController.Move(_opositeDirection);
        }

        public override void OnStateExit(object action)
        {
            base.StateController.EndDodge();
        }
    }
}
