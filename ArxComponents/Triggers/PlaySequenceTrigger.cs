using InventorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces.Inventory;
using AnimatorSequencer;
using InventorySystem.Triggers;

namespace ArxGame.Triggers
{
    public class PlaySequenceTrigger : ItemUseTrigger
    {
        public AnimationSequenceBehaviour animationSequence;

        public override bool Use(IItemOwner owner, IInventoryItem item)
        {
            animationSequence.Run();
            return true;
        }
    }
}
