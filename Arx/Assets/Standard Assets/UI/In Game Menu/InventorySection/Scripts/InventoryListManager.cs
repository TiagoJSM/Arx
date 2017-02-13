using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using Assets.Standard_Assets.UI.In_Game_Menu.InventorySection.Scripts;
using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.InventorySection.Scripts
{
    public class InventoryListManager : MonoBehaviour
    {
        [SerializeField]
        private ItemDescriptionManager _itemDescription;

        public GameObject InventoryItemPrefab;
        public GameObject Content;

        public InventoryItemManager[] InventoryItems
        {
            get
            {
                return Content.GetComponentsInChildren<InventoryItemManager>();
            }
        }

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
            itemManager.OnPreviewItem += OnPreviewHandler;
        }

        public void RemoveAllItems()
        {
            var items = Content.GetComponentsInChildren<InventoryItemManager>(true);
            for (var idx = 0; idx < items.Length; idx++)
            {
                Destroy(items[idx].gameObject);
            }
            Content.transform.DetachChildren();
        }

        private void OnPreviewHandler(IInventoryItem item)
        {
            var inventoryItem = item as InventoryItem;
            if (inventoryItem == null)
            {
                return;
            }
            _itemDescription.Item = inventoryItem;
        }
    }
}
