using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class FileGridDataView
{
    private static Vector2 _scrollPosition;
    private static int _selectedSheet;

    private static GUIStyle _headerCellStyle;
    private static GUIStyle _valueCellStyle;
    private static GUIStyle _tableStyle;

    private static GUIStyle HeaderCellStyle
    {
        get
        {
            if (_headerCellStyle == null)
            {
                _headerCellStyle = new GUIStyle()
                {
                    border = new RectOffset(1, 1, 1, 1),
                    normal = new GUIStyleState()
                    {
                        background = new Texture2D(1, 1)
                    }
                };
                _headerCellStyle.normal.background.SetPixels(new[] { new Color(0.5f, 0.85f, 1f, 0.7f) });
            }
            return _headerCellStyle;
        }
    }

    private static GUIStyle ValueCellStyle
    {
        get
        {
            if (_valueCellStyle == null)
            {
                _valueCellStyle = new GUIStyle()
                {
                    border = new RectOffset(1, 1, 1, 1),
                    normal = new GUIStyleState()
                    {
                        background = new Texture2D(1, 1)
                    }
                };
                _valueCellStyle.normal.background.SetPixels(new[] { new Color(0.9f, 0.9f, 0.9f, 0.2f) });
            }
            return _valueCellStyle;
        }
    }

    private static GUIStyle TableStyle
    {
        get
        {
            if (_tableStyle == null) {
                _tableStyle = new GUIStyle()
                {
                    normal = new GUIStyleState()
                    {
                        background = new Texture2D(1, 1)
                    }
                };
                _tableStyle.normal.background.SetPixel(0, 0, new Color(0.3f, 0.3f, 0.3f, 0.3f));
            }
            return _tableStyle;
        }

    }

    public static void OnGui(IDialogEditorContext context)
    {
        if(context.ExcelFile == null)
        {
            RenderNoFileSelected(context);
        }
        else
        {
            RenderSheetList(context.ExcelFile);
            RenderTable(context.ExcelFile.Sheets[_selectedSheet]);
            RenderButtons(context);
        }
    }

    private static void RenderButtons(IDialogEditorContext context)
    {
        EditorGUILayout.BeginHorizontal();
        SaveButton(context);
        UpdateButton(context);
        EditorGUILayout.EndHorizontal();
    }

    private static void RenderSheetList(ExcelFile excelFile)
    {
        _selectedSheet = EditorGUILayout.Popup(_selectedSheet, excelFile.Sheets.Select(s => s.Name).ToArray());
    }

    private static void RenderNoFileSelected(IDialogEditorContext context)
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("No file selected, select one file before trying to check its content");
        if (GUILayout.Button("Go to file selection"))
        {
            context.ViewState = DialogViewState.FileSelectionView;
        }
        EditorGUILayout.EndVertical();
    }

    private static void RenderTable(ExcelSheet sheet)
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        EditorGUILayout.BeginVertical(TableStyle);
        RenderHeaders(sheet);

        for (var row = 0; row < sheet.RowCount; row++)
        {
            RenderRow(sheet.GetRow(row), ValueCellStyle);
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    private static void RenderRow(string[] rowValues, GUIStyle style)
    {
        EditorGUILayout.BeginHorizontal();
        for (var idx = 0; idx < rowValues.Length; idx++)
        {
            EditorGUILayout.LabelField(rowValues[idx], style);
        }
        EditorGUILayout.EndHorizontal();
    }

    private static void RenderHeaders(ExcelSheet sheet)
    {
        RenderRow(sheet.Headers, HeaderCellStyle);
    }

    private static void SaveButton(IDialogEditorContext context)
    {
        GUI.enabled = false;
        var path = default(string);
        if (Selection.activeObject != null)
        {
            path = AssetDatabase.GetAssetPath(Selection.activeObject);
            GUI.enabled = Directory.Exists(path);
        }

        if (GUILayout.Button("Save"))
        {
            var localized = ScriptableObject.CreateInstance<LocalizedTexts>();
            var sheet = context.ExcelFile.Sheets[_selectedSheet];
            PopulateLocalizedTexts(localized, sheet);
            AssetDatabase.CreateAsset(localized, path + "/Localized Texts.asset");
        }
        GUI.enabled = true;
    }

    private static void PopulateLocalizedTexts(LocalizedTexts localized, ExcelSheet sheet)
    {
        localized.localizations = sheet.Headers.Select((header, languageIdx) =>
        {
            var keys = sheet.Values.Select(value => value[0]).ToArray();

            return new LanguageGroup()
            {
                Language = header,
                Localizations = new LocalizedText()
                {
                    localization = keys.Select((key, rowIdx) =>
                    {
                        return new Translation()
                        {
                            Key = key,
                            Translated = sheet.GetRow(rowIdx)[languageIdx]
                        };
                    })
                    .ToArray()
                }
            };
        })
        .ToArray();
    }

    private static void UpdateButton(IDialogEditorContext context)
    {
        GUI.enabled = Selection.activeObject is LocalizedTexts;
        if (GUILayout.Button("Update"))
        {
            var sheet = context.ExcelFile.Sheets[_selectedSheet];
            var localizedText = Selection.activeObject as LocalizedTexts;
            PopulateLocalizedTexts(localizedText, sheet);
            EditorUtility.SetDirty(localizedText);
            AssetDatabase.SaveAssets();
        }
        GUI.enabled = true;
    }


}
