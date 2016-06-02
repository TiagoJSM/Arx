using ArxGame.UI;
using CommonInterfaces.UI;
using InventorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components
{
    [RequireComponent(typeof(InventoryComponent))]
    public class UiController : MonoBehaviour, IUiController
    {
        private GameObject _inGameMenu;
        private InventoryComponent _inventoryComponent;

        public GameObject inGameMenuPrefab;

        public bool IsMenuOn
        {
            get
            {
                return _inGameMenu != null;
            }
        }

        public void TurnOn()
        {
            if (IsMenuOn)
            {
                return;
            }
            _inGameMenu = Instantiate(inGameMenuPrefab);
            var menu = _inGameMenu.GetComponent<UiMenuManager>();
            menu.inventoryComponent = _inventoryComponent;
        }

        public void TurnOff()
        {
            if (!IsMenuOn)
            {
                return;
            }
            Destroy(_inGameMenu);
            _inGameMenu = null;
        }

        public void Toggle()
        {
            if (IsMenuOn)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }

        void Start()
        {
            _inventoryComponent = GetComponent<InventoryComponent>();
        }
    }
}
