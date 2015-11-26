using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace CommonEditors.GuiComponents.GuiComponents.CustomEditors
{
    public class EditorWithoutScript : Editor
    {
        //Sort of a hack, I had to discover the name of the property for script to remove it
        //by iterating the serializedObject properties
        private const string ScriptPropertyName = "m_Script";

        public override void OnInspectorGUI()
        {
            var propertiesToExclude = AdditionalPropertiesToExclude().ToList();
            propertiesToExclude.Add(ScriptPropertyName);

            DrawPropertiesExcluding(this.serializedObject, propertiesToExclude.ToArray());
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual string[] AdditionalPropertiesToExclude()
        {
            return new string[0];
        }
    }
}
