using Assets.Standard_Assets.Common.Attributes;
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Common.Editor.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerAttributeDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUI.LabelField(position, "The property has to be a layer for LayerAttribute to work!");
                return;
            }

            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}
