using InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryItemPickable : MonoBehaviour
    {
        [SerializeField]
        private GameObject _rootObject;

        public InventoryItem item;

        public void PickUp()
        {
            Destroy(_rootObject);
        }
    }
}
