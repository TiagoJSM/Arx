using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpeakersAttribute))]
public class SpeakersAttributeDrawer : PropertyDrawer
{
    private const string SpeakersPath = "_speakers";
    private const string ConversationPath = "_conversation";

    private SpeakersAttribute _attributeValue = null;

    private SpeakersAttribute SpeakersAttribute
    {
        get
        {
            if (_attributeValue == null)
            {
                _attributeValue = (SpeakersAttribute)attribute;
            }
            return _attributeValue;
        }
    }

    private float SpeechTextHeight
    {
        get
        {
            return EditorGUIUtility.singleLineHeight * 3;
        }
    }

    private float SpeakerFieldHeight
    {
        get
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var keys = GetKeys(property);

        if (keys == null)
        {
            return base.GetPropertyHeight(property, label);
        }
        var localizedTextPropertyHeight = EditorGUIUtility.singleLineHeight;
        var heightPerKey = EditorGUIUtility.singleLineHeight * 4;
        return (keys.Length * heightPerKey) + localizedTextPropertyHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var localizedTextsProp = GetLocalizedTextsProperty(property);
        var localizedtextsPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(localizedtextsPosition, localizedTextsProp);

        var speakersProperty = property.FindPropertyRelative(SpeakersPath);
        var baseSpeakerPosition = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);

        var keys = GetKeys(property);

        if (keys == null)
        {
            return;
        }

        speakersProperty.arraySize = keys.Length;

        for (var idx = 0; idx < speakersProperty.arraySize; idx++)
        {
            EditorGUI.PropertyField(baseSpeakerPosition, speakersProperty.GetArrayElementAtIndex(idx), new GUIContent("Speaker " + (idx + 1)));
            baseSpeakerPosition.y += SpeakerFieldHeight;
            baseSpeakerPosition.height = SpeechTextHeight;

            GUI.enabled = false;
            EditorGUI.TextArea(baseSpeakerPosition, keys[idx]);
            GUI.enabled = true;
            baseSpeakerPosition.y += SpeechTextHeight;
            baseSpeakerPosition.height = SpeakerFieldHeight;
        }
    }

    private string[] GetKeys(SerializedProperty property)
    {
        var localizedTexts = GetLocalizedTexts(property);
        if (localizedTexts == null)
        {
            return null;
        }
        var firstLocalization = localizedTexts.localizations.FirstOrDefault();
        if (firstLocalization == null)
        {
            return null;
        }
        return firstLocalization.Localizations.translation.Select(translation => translation.Key).ToArray();
    }

    private LocalizedTexts GetLocalizedTexts(SerializedProperty property)
    {
        var convoProperty = GetLocalizedTextsProperty(property);
        return convoProperty.objectReferenceValue as LocalizedTexts;
    }

    private SerializedProperty GetLocalizedTextsProperty(SerializedProperty property)
    {
        return property.FindPropertyRelative(ConversationPath);
    }
}