using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem.InventoryObjects
{
    public abstract class InventoryItem : ScriptableObject, IInventoryItem
    {
        [SerializeField]
        private string _name;
        [SerializeField]
        private string _id;

        public bool canDiscard;
        public bool canStack;
        public int maximumStack;
        [TextArea]
        public string description;
        public bool removeOnUse;

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

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

        public abstract bool CanUse();
        public abstract void UseItem();
    }
}
