using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ConnectToGoogleApiView
{
    public static void OnGui(IDialogEditorContext context)
    {
        EditorGUILayout.BeginVertical();

        context.ClientId = EditorGUILayout.TextField("Client ID", context.ClientId);

        GUI.enabled = !string.IsNullOrEmpty(context.ClientId);
        if (GUILayout.Button("Connect"))
        {
            GoogleApi.RequestCodeOnBrowser(context.ClientId);
            context.ViewState = DialogViewState.CodeRequestView;
        }
        GUI.enabled = true;

        EditorGUILayout.EndVertical();
    }
}

