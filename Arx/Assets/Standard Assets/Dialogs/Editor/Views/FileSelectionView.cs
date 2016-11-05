using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class FileSelectionView
{
    private static Vector2 _scrollPosition;
    private static int _selectedItem = -1;

    public static void OnGui(IDialogEditorContext context)
    {
        if(context.FileList == null)
        {
            RenderFilesNotLoaded(context);
        }
        else if(context.FileList.Files.Length == 0)
        {
            RenderEmptyFileList(context);
        }
        else
        {
            RenderFileList(context);
        }
    }

    private static void RenderFilesNotLoaded(IDialogEditorContext context)
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("No files loaded, possibly you need to connect to the service");
        if(GUILayout.Button("Go to service connection"))
        {
            context.ViewState = DialogViewState.ConnectView;
        }
        EditorGUILayout.EndVertical();
    }

    private static void RenderEmptyFileList(IDialogEditorContext context)
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("No files found, create some dialog files and try again");
        EditorGUILayout.EndVertical();
    }

    private static void RenderFileList(IDialogEditorContext context)
    {
        var buttonSkin = new GUIStyle(GUI.skin.button);
        EditorGUILayout.BeginVertical();

        buttonSkin.alignment = TextAnchor.MiddleLeft;
        buttonSkin.padding = new RectOffset(10, 10, 10, 10);

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, false);
        var selected = GUILayout.SelectionGrid(
            _selectedItem,
            context.FileList.Files.Select(f => new GUIContent(f.Name)).ToArray(),
            6,
            buttonSkin,
            GUILayout.MaxWidth(500),
            GUILayout.ExpandWidth(true));

        EditorGUILayout.EndScrollView();
        
        if(_selectedItem != selected)
        {
            _selectedItem = selected;
            context.ExcelFile = GetExcelFile(context);
            context.ViewState = DialogViewState.FileGridDataView;
        }

        EditorGUILayout.EndVertical();
    }

    private static ExcelFile GetExcelFile(IDialogEditorContext context)
    {
        var fileId = context.FileList.Files[_selectedItem].Id;
        var model = GoogleApi.GetSheets(context.Token, fileId);
        return new ExcelFile()
        {
            Sheets = model.sheets.Select(sheet =>
            {
                var sheetName = sheet.properties.title;
                var sheetValues = GoogleApi.GetValuesInSheet(context.Token, fileId, sheetName);
                return new ExcelSheet()
                {
                    Name = sheet.properties.title,
                    Data = sheetValues
                };
            })
            .ToArray()
        };
    }
}

