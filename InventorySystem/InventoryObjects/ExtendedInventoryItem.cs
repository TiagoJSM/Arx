using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem.InventoryObjects
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
        public Texture2D icon;
        public abstract InventoryItemType ItemType { get; }
    }
}
