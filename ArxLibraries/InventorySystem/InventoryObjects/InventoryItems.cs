using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorySystem.InventoryObjects
{
    public class InventoryItems
    {
        private List<IInventoryItem> _items;
        private int? _maxCount;

        public int? MaxCount 
        {
            get
            {
                return _maxCount;
            }
            set
            {
                if (value == null)
                {
                    _maxCount = null;
                    return;
                }
                if(value <= 0)
                {
                    return;
                }
                _maxCount = value;
            }
        }

        public InventoryItems()
        {
            _items = new List<IInventoryItem>();
        }

        public bool Add(IInventoryItem item)
        {
            if (_maxCount != null && _items.Count >= _maxCount)
            {
                return false;
            }
            _items.Add(item);
            return true;
        }

        public bool Remove(IInventoryItem item)
        {
            return _items.Remove(item);
        }
    }
}
