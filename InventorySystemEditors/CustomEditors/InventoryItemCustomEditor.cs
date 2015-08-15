﻿using CommonEditors.GuiComponents.GuiComponents.CustomEditors;
using InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorySystemEditors.CustomEditors
{
    public class InventoryItemCustomEditor : EditorWithoutScript
    {
        private string[] _propertiesToExclude;

        public InventoryItemCustomEditor()
        {
            _propertiesToExclude = new string[]{ "maximumStack" };
        }

        protected override string[] AdditionalPropertiesToExclude()
        {
            var inventoryItem = target as InventoryItem;
            if (inventoryItem == null)
            {
                return base.AdditionalPropertiesToExclude();
            }
            if (!inventoryItem.canStack)
            {
                inventoryItem.maximumStack = 1;
                return _propertiesToExclude;
            }
            return new string[0];
        }
    }
}