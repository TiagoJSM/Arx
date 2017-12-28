using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets.InventorySystem;
using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Loot
{
    [RequireComponent(typeof(BasePlatformerController))]
    public class DropLootOnDeath : MonoBehaviour
    {
        private BasePlatformerController _platformer;

        [SerializeField]
        private InventoryItem _item;
        [SerializeField]
        private InventoryItemPickable _pickablePrefab;

        private void Awake()
        {
            _platformer = GetComponent<BasePlatformerController>();
            _platformer.OnKilled += OnKilledHandler;
        }

        private void OnDestroy()
        {
            _platformer.OnKilled -= OnKilledHandler;
        }

        private void OnKilledHandler(BasePlatformerController character)
        {
            var pickable = Instantiate(_pickablePrefab, transform.position, Quaternion.identity);
            pickable.Item = _item;
        }
    }
}
