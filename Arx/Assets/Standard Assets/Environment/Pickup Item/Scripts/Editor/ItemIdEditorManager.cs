using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Standard_Assets.Environment.Pickup_Item.Scripts.Editor
{
    [InitializeOnLoad]
    public class ItemIdEditorManager
    {
        static ItemIdEditorManager()
        {
            EditorApplication.hierarchyWindowChanged += VerifyItemId;
        }

        private static void VerifyItemId()
        {

        }
    }
}
