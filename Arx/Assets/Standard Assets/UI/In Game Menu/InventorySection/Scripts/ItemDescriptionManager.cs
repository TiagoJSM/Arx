using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.In_Game_Menu.InventorySection.Scripts
{
    public class ItemDescriptionManager : MonoBehaviour
    {
        private InventoryItem _item;

        [SerializeField]
        private Text _title;
        [SerializeField]
        private Text _description;

        public InventoryItem Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                HandleItem();
            }
        }

        private void HandleItem()
        {
            _title.text = _item.Name;
            _description.text = _item.description;
        }
    }
}
