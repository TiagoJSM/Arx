using GenericComponents.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.States.InteractionStates
{
    [Serializable]
    public class DialogueState : BaseSequenceState
    {
        public TemplateSpeechController speechController;
        public string buttonNameToContinue;
        [TextArea(3, 10)]
        public string text;

        protected override void PerformOnStateEnter()
        {
            speechController.Visible = true;
            speechController.Text = text;
        }

        protected override void PerformOnStateUpdate()
        {
            var buttonDown = Input.GetButtonDown(buttonNameToContinue);
            if (buttonDown)
            {
                speechController.Continue();
            }
        }

        protected override void PerformOnStateExit()
        {
            speechController.Visible = false;
            speechController.Text = string.Empty;
        }

        public override bool Complete()
        {
            return speechController.SpeechEnded;
        }
    }
}
