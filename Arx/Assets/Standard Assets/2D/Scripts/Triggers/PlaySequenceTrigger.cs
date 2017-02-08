using AnimatorSequencer;
using Assets.Standard_Assets.InventorySystem.Triggers;
using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Triggers
{
    public class PlaySequenceTrigger : ItemUseTrigger
    {
        public AnimationSequenceBehaviour animationSequence;

        protected override bool DoUse(IItemOwner owner, IInventoryItem item)
        {
            animationSequence.Run();
            return true;
        }
    }
}
