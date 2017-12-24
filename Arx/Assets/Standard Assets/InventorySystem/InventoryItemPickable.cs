using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.InventorySystem
{
    public class InventoryItemPickable : MonoBehaviour
    {
        [SerializeField]
        private GameObject _rootObject;
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private InventoryItem item;

        public InventoryItem Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
                spriteRenderer.sprite = item.inGameImage;
            }
        }

        public void PickUp()
        {
            Destroy(_rootObject);
        }

        private void Awake()
        {
            if (item.inGameImage != null)
            {
                spriteRenderer.sprite = item.inGameImage;
            }
        }
    }
}
