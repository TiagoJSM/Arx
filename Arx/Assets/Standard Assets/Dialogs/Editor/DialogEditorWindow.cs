using Assets.Standard_Assets.Dialogs.Editor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class DialogEditorWindow : EditorWindow, IDialogEditorContext
{
    private delegate void DialogViewOnGui(IDialogEditorContext context);

    public string Code { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Token { get; set; }
    public DialogViewState ViewState { get; set; }
    public ExcelFile ExcelFile { get; set; }
    public DialogFileList FileList { get; set; }

    private Dictionary<DialogViewState, DialogViewOnGui> _views = 
        new Dictionary<DialogViewState, DialogViewOnGui>()
        {
            { DialogViewState.ConnectView, ConnectToGoogleApiView.OnGui },
            { DialogViewState.CodeRequestView, CodeRequestView.OnGui },
            { DialogViewState.FileSelectionView, FileSelectionView.OnGui },
            { DialogViewState.FileGridDataView, FileGridDataView.OnGui },
            { DialogViewState.AssetsUpdateView, AssetsUpdateView.OnGui }
        };

    public DialogEditorWindow()
    {
        Code = string.Empty;
        /*ExcelFile = new ExcelFile()
        {
            Sheets = new ExcelSheet[] 
            {
                new ExcelSheet()
                {
                    Name = "sheet1",
                    Data = new string[][]
                    {
                        new string[]
                        {
                            "EN", "PT", "GR"
                        },
                        new string[]
                        {
                            "Hello", "Ola", "Gia'sou"
                        },
                        new string[]
                        {
                            "Bye", "Adeus", "Andiou"
                        }
                    }
                },
                new ExcelSheet()
                {
                    Name = "sheet2",
                    Data = new string[][]
                    {
                        new string[]
                        {
                            "EN", "PT", "GR"
                        },
                        new string[]
                        {
                            "Day", "Dia", "Mera"
                        },
                    }
                }
            }
        };*/
        /*var fileTexture = new Texture2D(32, 32);
        FileList = new DialogFileList()
        {
            Files = new int[400].Select((i, idx) =>
                new DialogFile()
                {
                    Name = "DemoDemo DemoDemo DemoDemoDemoDemoDemoDemoDemoDemoDemoDemoDemoDemoDemoDemoDemo" + idx,
                    Image = fileTexture
                })
                .ToArray()
        };*/
    }

    [MenuItem("Window/Dialog Editor")]
    private static void Init()
    {
        GetWindow<DialogEditorWindow>();
    }

    private void OnGUI()
    {
        TopMenu();
        EditorGUILayout.BeginVertical(ContextStyle());
        _views[ViewState](this);
        EditorGUILayout.EndVertical();
    }

    private void TopMenu()
    {
        EditorGUILayout.BeginHorizontal();

        var toolBarButton = (DialogViewState)GUILayout.Toolbar(
            (int)ViewState, 
            new[]
            {
                "Connect to service",
                "Set code",
                "View file list",
                "Selected file",
                "Assets update view"
            });

        if(toolBarButton != ViewState)
        {
            ViewState = toolBarButton;
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndHorizontal();
    }

    private GUIStyle ContextStyle()
    {
        return new GUIStyle()
        {
            margin = new RectOffset(20, 20, 20, 20)
        };
    }
}
