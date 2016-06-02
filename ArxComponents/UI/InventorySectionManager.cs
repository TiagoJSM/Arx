using CommonInterfaces.Inventory;
using InventorySystem;
using InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.UI
{
    public class InventorySectionManager : MonoBehaviour, IItemOwner
    {
        public InventoryListManager listManager;

        public InventoryComponent InventoryComponent { get; private set; }

        //void Start()
        //{
        //    Initialize(null);
        //}

        public void Initialize(InventoryComponent inventoryComponent)
        {
            InventoryComponent = inventoryComponent;

            var inventory = InventoryComponent.Inventory;

            //var inventory = new Inventory(this);
            //inventory.AddItem(new QuestInventoryItem()
            //{
            //    Id = "abc",
            //    Name = "item 1",
            //    description = "desc1"
            //});
            //inventory.AddItem(new QuestInventoryItem()
            //{
            //    Id = "abcd",
            //    Name = "item 2",
            //    description = "desc2"
            //});
            //inventory.AddItem(new QuestInventoryItem()
            //{
            //    Id = "abcd",
            //    Name = "item 2",
            //    description = "desc2"
            //});
            foreach (var item in inventory.InventoryItems)
            {
                listManager.Add(item);
            }
        }
    }
}
