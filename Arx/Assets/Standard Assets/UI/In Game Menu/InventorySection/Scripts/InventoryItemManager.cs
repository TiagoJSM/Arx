using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.InventorySection.Scripts
{
    public class InventoryItemManager : MonoBehaviour, IPointerClickHandler, ISubmitHandler, ISelectHandler
    {
        public Text itemName;
        public Text itemCount;
        public Image icon;

        public InventoryItems InventoryItems { get; set; }

        public Action<IInventoryItem> OnPreviewItem;

        void Update()
        {
            itemName.text = InventoryItems.Item.Name;
            itemCount.text = InventoryItems.Count.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 1)
            {
                OnPreviewItemHandler();
            }
            if (eventData.clickCount == 2)
            {
                UseItem();
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            UseItem();
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnPreviewItemHandler();
        }

        private void OnPreviewItemHandler()
        {
            if (OnPreviewItem != null)
            {
                OnPreviewItem(InventoryItems.Item);
            }
        }

        private void UseItem()
        {
            var item = InventoryItems.Item as InventoryItem;
            if (item != null)
            {
                item.UseItem();
            }
        }
    }
}
