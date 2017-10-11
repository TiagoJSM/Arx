using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Characters.Enemies.Canyon_Engineer.Scripts
{
    public class SurprisedState : BaseAiState<EngineerEnemyAiControl>
    {
        public override void OnStateEnter(object action)
        {
            base.OnStateEnter(action);
            base.StateController.ShowSurprise();
        }
    }
}
