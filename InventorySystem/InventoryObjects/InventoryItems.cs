using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem.InventoryObjects
{
    public class InventoryItems
    {
        public IInventoryItem Item { get; set; }
        public int Count { get; set; }
    }
}
