using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Inventory;
using UnityEngine;
using Assets.Standard_Assets.InventorySystem.Triggers;

namespace Assets.Standard_Assets._2D.Scripts.Interaction
{
    public class OpenPathOnItemUse : ItemUseTrigger
    {
        [SerializeField]
        private Collider2D _collider;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private string _animationOnOpen = "Open";

        protected override bool DoUse(IItemOwner owner, IInventoryItem item)
        {
            _collider.enabled = false;

            if(_animator != null)
            {
                _animator.Play(_animationOnOpen);
            }
            return true;
        }
    }
}
