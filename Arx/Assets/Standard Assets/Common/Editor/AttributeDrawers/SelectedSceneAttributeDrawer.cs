using Assets.Standard_Assets.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Standard_Assets.Common.Editor.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(SceneFieldAttribute))]
    public class SceneFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginProperty(_position, GUIContent.none, _property);
            SerializedProperty sceneAsset = _property.FindPropertyRelative("m_SceneAsset");
            SerializedProperty sceneName = _property.FindPropertyRelative("m_SceneName");
            _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
            if (sceneAsset != null)
            {
                EditorGUI.BeginChangeCheck();

                UnityEngine.Object value = EditorGUI.ObjectField(_position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
                if (EditorGUI.EndChangeCheck())
                {
                    sceneAsset.objectReferenceValue = value;
                    if (sceneAsset.objectReferenceValue != null)
                    {
                        sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;
                    }
                }
            }
            EditorGUI.EndProperty();
        }
    }
}
