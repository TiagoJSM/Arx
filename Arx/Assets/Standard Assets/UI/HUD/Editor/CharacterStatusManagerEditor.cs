using Assets.Standard_Assets.UI.HUD.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.UI.HUD.Editor
{
    [CustomEditor(typeof(CharacterStatusManager))]
    public class CharacterStatusManagerEditor : UnityEditor.Editor
    {
        private int _points = 0;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var manager = (CharacterStatusManager)target;

            EditorGUILayout.LabelField("Testing fields");
            _points = EditorGUILayout.IntField("Points", _points);

            if (GUILayout.Button("Gain life points"))
            {
                manager.Health += _points;
            }
            if (GUILayout.Button("Lose life points"))
            {
                manager.Health -= _points;
            }
            if (GUILayout.Button("Instantiate Hearts"))
            {
                manager.InstantiateHearts(true);
            }
        }
    }
}
