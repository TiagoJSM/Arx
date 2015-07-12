using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using Utils;

namespace InventorySystemEditors.GuiComponent
{
    public class InventoryItemIoActionsMenuGuiComponent : FileIoActionsMenuGuiComponent
    {
        private Type[] _inventoryItemTypes;
        private string[] _inventoryItemNames;
        private int _index;

        public Type SelectedInventoryType
        {
            get
            {
                return _inventoryItemTypes[_index];
            }
        }

        public InventoryItemIoActionsMenuGuiComponent(UnityEngine.Object obj = null)
            : base(
                "Save item", 
                "Please enter a file name to save the item to",
                "Open item",
                "asset",
                obj)
        {
            _inventoryItemTypes = IntrospectionUtils.GetAllCompatibleTypes<InventoryItem>().ToArray();
            _inventoryItemNames = _inventoryItemTypes.Select(t => t.Name).ToArray();
        }

        public override void OnGui()
        {
            _index = EditorGUILayout.Popup(_index, _inventoryItemNames);
            base.OnGui();
        }
    }
}
