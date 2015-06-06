using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryComponent : MonoBehaviour
    {
        private IItemOwner _owner;
        public Inventory Inventory { get; private set; }

        void Start()
        {
            _owner = this.gameObject.GetComponent<IItemOwner>();
            Inventory = new Inventory(_owner);
        }
    }
}
