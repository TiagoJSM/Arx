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
    public class InventoryItemManager : MonoBehaviour, IPointerClickHandler
    {
        public Text itemName;
        public Text itemCount;
        public Image icon;

        public InventoryItems InventoryItems { get; set; }

        public Action<IInventoryItem> OnClick;

        void Update()
        {
            itemName.text = InventoryItems.Item.Name;
            itemCount.text = InventoryItems.Count.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 1)
            {
                if (OnClick != null)
                {
                    OnClick(InventoryItems.Item);
                }
            }
            if (eventData.clickCount == 2)
            {
                var item = InventoryItems.Item as InventoryItem;
                if (item != null)
                {
                    item.UseItem();
                }
            }
        }
    }
}
