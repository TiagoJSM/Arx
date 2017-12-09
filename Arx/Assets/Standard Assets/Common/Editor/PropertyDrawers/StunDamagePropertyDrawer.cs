using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Common.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(StunDamage))]
    public class StunDamagePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var allAttachsStunProp = property.FindPropertyRelative("allAttacksStun");
            var allAttachsStun = allAttachsStunProp.boolValue;

            var height = EditorGUI.GetPropertyHeight(allAttachsStunProp);

            if (!allAttachsStun)
            {
                var stunFilters = property.FindPropertyRelative("stunFilters");
                height += EditorGUI.GetPropertyHeight(stunFilters);
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var initialXPosition = position.x;
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            EditorGUI.indentLevel = 0;
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var allAttachsStunProp = property.FindPropertyRelative("allAttacksStun");
            var allAttachsStun = allAttachsStunProp.boolValue;
            EditorGUI.PropertyField(position, allAttachsStunProp);
            EditorGUI.indentLevel = 1;
            position.x = initialXPosition;
            position.y += EditorGUIUtility.singleLineHeight;

            if (!allAttachsStun)
            {
                var stunFilters = property.FindPropertyRelative("stunFilters");
                EditorGUI.PropertyField(position, stunFilters, true);
            }

            EditorGUI.EndProperty();
        }
    }
}
