using GenericComponents.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class InteractiveDialog
{
    [SerializeField]
    private LocalizedTexts _conversation;

    [SerializeField]
    private SpeechController[] _speakers;

    public int DialogsCount { get { return _speakers.Length; } }

    public Tuple<SpeechController, string> GetDialogContext(int idx)
    {
        return new Tuple<SpeechController, string>(_speakers[idx], _conversation[idx]);
    }
}