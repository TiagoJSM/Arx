using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.InventorySystem.InventoryObjects
{
    public enum InventoryItemType
    {
        QuestItem,
        Weapon,
        Consumable,
        Note
    }

    public abstract class ExtendedInventoryItem : InventoryItem
    {
        public abstract InventoryItemType ItemType { get; }
    }
}
