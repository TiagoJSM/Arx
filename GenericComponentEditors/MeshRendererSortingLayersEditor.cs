using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Utils;

namespace GenericComponentEditors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MeshRenderer))]
    public class MeshRendererSortingLayersEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            SerializedProperty sortingLayerID = serializedObject.FindProperty("m_SortingLayerID");
            SerializedProperty sortingOrder = serializedObject.FindProperty("m_SortingOrder");
            //MeshRenderer renderer = target as MeshRenderer;
            Rect firstHoriz = EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginProperty(firstHoriz, GUIContent.none, sortingLayerID);
            string[] layerNames = EditorUtils.GetSortingLayerNames();
            int[] layerID = EditorUtils.GetSortingLayerUniqueIDs();
            int selected = -1;
            int sID = sortingLayerID.intValue;
            for (int i = 0; i < layerID.Length; i++)
                if (sID == layerID[i])
                    selected = i;
            if (selected == -1)
                for (int i = 0; i < layerID.Length; i++)
                    if (layerID[i] == 0)
                        selected = i;
            selected = EditorGUILayout.Popup("Sorting Layer", selected, layerNames);
            sortingLayerID.intValue = layerID[selected];
            EditorGUI.EndProperty();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sortingOrder, new GUIContent("Order in Layer"));
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
