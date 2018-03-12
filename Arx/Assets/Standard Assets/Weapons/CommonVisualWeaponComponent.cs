using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Standard_Assets.Weapons
{
    [RequireComponent(typeof(SortingGroup))]
    public class CommonVisualWeaponComponent : MonoBehaviour
    {
        private SortingGroup _sortingGroup;

        public int OrderInLayer
        {
            get
            {
                return _sortingGroup.sortingOrder;
            }
            set
            {
                _sortingGroup.sortingOrder = value;
            }
        }

        public LayerMask SortingLayer
        {
            get
            {
                return LayerMask.NameToLayer(_sortingGroup.sortingLayerName);
            }

            set
            {
                _sortingGroup.sortingLayerName = LayerMask.LayerToName(value);
            }
        }

        private void Awake()
        {
            _sortingGroup = GetComponent<SortingGroup>();
        }
    }
}
