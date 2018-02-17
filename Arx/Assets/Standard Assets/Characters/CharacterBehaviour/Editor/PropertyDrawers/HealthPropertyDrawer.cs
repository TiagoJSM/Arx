using GenericComponents.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.CharacterBehaviour.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Health))]
    public class HealthPropertyDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var lifePointsRect = new Rect(position.x, position.y, position.width / 2, position.height);
            var maxLifePointsRect = new Rect(position.x + position.width / 2, position.y, position.width / 2 * 2, position.height);

            var lifeProp = property.FindPropertyRelative("lifePoints");
            var maxLifeProp =  property.FindPropertyRelative("maxLifePoints");

            EditorGUI.DelayedIntField(lifePointsRect, lifeProp, GUIContent.none);
            EditorGUIUtility.labelWidth = 14f;
            EditorGUI.DelayedIntField(maxLifePointsRect, maxLifeProp, new GUIContent("/"));

            if (!EditorApplication.isPlaying)
            {
                if (lifeProp.intValue <= 0)
                {
                    lifeProp.intValue = 1;
                }
                if (maxLifeProp.intValue <= 0)
                {
                    maxLifeProp.intValue = 1;
                }
                if (lifeProp.intValue > maxLifeProp.intValue)
                {
                    maxLifeProp.intValue = lifeProp.intValue;
                }
            }
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
