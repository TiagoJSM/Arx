using CommonInterfaces.Inventory;
using InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.UI
{
    public class InventoryListManager : MonoBehaviour
    {
        public GameObject InventoryItemPrefab;
        public GameObject Content;

        public void Add(InventoryItems items)
        {
            var itemNode = Instantiate(InventoryItemPrefab);
            var itemManager = itemNode.GetComponent<InventoryItemManager>();
            itemNode.transform.SetParent(Content.transform, false);
            if (itemManager == null)
            {
                Debug.Log("'InventoryItemPrefab' doesn't contain a component of type 'InventoryItemManager'");
                return;
            }
            itemManager.InventoryItems = items;
        }
    }
}
