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

        public InventoryComponent InventoryComponent { get; set; }
        void Awake()
        {
            //var inventory = InventoryComponent.Inventory;
            var inventory = new Inventory(this);
            inventory.AddItem(new InventoryItem()
            {
                Id = "abc",
                Name = "item 1",
                description = "desc1"
            });
            inventory.AddItem(new InventoryItem()
            {
                Id = "abcd",
                Name = "item 2",
                description = "desc2"
            });
            inventory.AddItem(new InventoryItem()
            {
                Id = "abcd",
                Name = "item 2",
                description = "desc2"
            });
            foreach (var item in inventory.InventoryItems)
            {
                listManager.Add(item);
            }
        }
    }
}
