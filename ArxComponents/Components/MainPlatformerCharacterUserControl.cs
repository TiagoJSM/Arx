using CommonInterfaces.Controllers;
using CommonInterfaces.Inventory;
using GenericComponents.UserControls;
using InventorySystem;
using InventorySystem.Controllers;
using QuestSystem;
using QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components
{
    [RequireComponent(typeof(ItemFinderController))]
    [RequireComponent(typeof(InventoryComponent))]
    [RequireComponent(typeof(QuestLogComponent))]
    public class MainPlatformerCharacterUserControl : PlatformerCharacterUserControl, IQuestSubscriber, IItemOwner, IPlayerControl
    {
        private ItemFinderController _itemFinderController;
        private InventoryComponent _inventoryComponent;
        private QuestLogComponent _questLogComponent;

        public event OnInventoryAdd OnInventoryItemAdd;
        public event OnInventoryRemove OnInventoryItemRemove;
        public event OnKill OnKill;

        public void AssignQuest(Quest quest)
        {
            throw new NotImplementedException();
        }

        public Quest GetQuest(string name)
        {
            throw new NotImplementedException();
        }

        public bool HasQuest(Quest quest)
        {
            throw new NotImplementedException();
        }
    }
}
