using CommonInterfaces.Inventory;
using InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ArxGame.UI
{
    public class InventoryItemManager : MonoBehaviour
    {
        public Text itemName;
        public Text itemCount;
        public Image icon;

        public InventoryItems InventoryItems { get; set; }

        void Update()
        {
            itemName.text = InventoryItems.Item.Name;
            itemCount.text = InventoryItems.Count.ToString();
        }
    }
}
