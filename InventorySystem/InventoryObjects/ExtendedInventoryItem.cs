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

    public class ExtendedInventoryItem : InventoryItem
    {
        public Texture2D icon;
        public InventoryItemType ItemType;
    }
}
