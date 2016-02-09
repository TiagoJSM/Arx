using CommonInterfaces.Inventory;
using InventorySystem.InventoryItemUseValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem.InventoryObjects
{
    public class InventoryItem : ScriptableObject, IInventoryItem
    {
        [SerializeField]
        private string _name;

        public bool canDiscard;
        public bool canStack;
        public int maximumStack;
        public string description;
        public bool removeOnUse;

        public UseValidation UseValidation;
        public InventoryItemUse itemUse;

        public string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public IItemOwner Owner { get; set; }

        public InventoryItem()
        {
            maximumStack = 1;
        }
    }
}
