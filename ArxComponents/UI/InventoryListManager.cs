using CommonInterfaces.Inventory;
using InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ArxGame.UI
{
    public class InventoryListManager : MonoBehaviour
    {
        public GameObject InventoryItemPrefab;
        public GameObject Content;
        public Text description;

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
            itemManager.OnClick += OnClickHandler;
        }

        private void OnClickHandler(IInventoryItem item)
        {
            var inventoryItem = item as InventoryItem;
            if(inventoryItem == null)
            {
                return;
            }
            description.text = inventoryItem.description;
        }
    }
}
