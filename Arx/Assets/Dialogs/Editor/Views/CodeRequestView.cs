using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class CodeRequestView
{
    private const string SheetMimeType = "application/vnd.google-apps.spreadsheet";
    private static readonly Texture Tex = new Texture2D(32, 32);

    public static void OnGui(IDialogEditorContext context)
    {
        EditorGUILayout.BeginVertical();

        context.Code = EditorGUILayout.TextField("Code", context.Code);
        context.ClientId = EditorGUILayout.TextField("Client Id", context.ClientId);
        context.ClientSecret = EditorGUILayout.TextField("Client secret", context.ClientSecret);

        GUI.enabled = CanSetCode(context);
        if (GUILayout.Button("Set code"))
        {
            context.Token = GoogleApi.GetToken(context.Code, context.ClientId, context.ClientSecret);
            context.FileList = GetFileList(context.Token);
            context.ViewState = DialogViewState.FileSelectionView;
        }
        GUI.enabled = true;

        EditorGUILayout.EndVertical();
    }

    private static DialogFileList GetFileList(string token)
    {
        var files = 
            GoogleApi
                .GetFileList(token)
                .items
                .Where(file => file.mimeType == SheetMimeType)
                .Select(file => new DialogFile()
                {
                    Id = file.id,
                    Name = file.title,
                    Image = Tex
                })
                .OrderBy(file => file.Name)
                .ToArray();

        return new DialogFileList()
        {
            Files = files
        };
    }

    private static bool CanSetCode(IDialogEditorContext context)
    {
        if(
            string.IsNullOrEmpty(context.Code) ||
            string.IsNullOrEmpty(context.ClientId) ||
            string.IsNullOrEmpty(context.ClientSecret))
        {
            return false;
        }
        return true;
    }
}

