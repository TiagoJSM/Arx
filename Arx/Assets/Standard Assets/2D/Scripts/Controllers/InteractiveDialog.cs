using Assets.Standard_Assets.UI.Speech.Scripts;
using GenericComponents.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InteractiveDialogContext
{
    public SpeechController speechController;
    public string text;
    public bool interacterSpeaking;

    public InteractiveDialogContext(SpeechController speechController, string text, bool interacterSpeaking)
    {
        this.speechController = speechController;
        this.text = text;
        this.interacterSpeaking = interacterSpeaking;
    }
}

[Serializable]
public class InteractiveDialog
{
    [SerializeField]
    private LocalizedTexts _conversation;

    [SerializeField]
    private SpeechController[] _speakers;

    [SerializeField]
    private bool[] _interactersSpeaking;

    public int DialogsCount { get { return _speakers.Length; } }

    public InteractiveDialogContext GetDialogContext(int idx)
    {
        return new InteractiveDialogContext(_speakers[idx], _conversation[idx], _interactersSpeaking[idx]);
    }
}