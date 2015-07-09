using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace QuestSystemEditors.CustomEditors
{
    public class ConditionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //Sort of a hack, I had to discover the name of the property for script to remove it
            //by iterating the serializedObject properties
            DrawPropertiesExcluding(this.serializedObject, "m_Script");
        }
    }
}
