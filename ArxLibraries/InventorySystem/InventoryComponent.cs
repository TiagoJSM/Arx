using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryComponent : MonoBehaviour
    {
        public Inventory Inventory { get; private set; }

        void Start()
        {
            Inventory = new Inventory();
        }
    }
}
