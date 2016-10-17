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
        private bool _closed;

        public TemplateSpeechController speechController;
        public string buttonNameToContinue;
        [TextArea(3, 10)]
        public string text;

        protected override void PerformOnStateEnter()
        {
            _closed = false;
            speechController.Say(text);
        }

        protected override void PerformOnStateUpdate()
        {
            var buttonDown = Input.GetButtonDown(buttonNameToContinue);
            if (buttonDown)
            {
                _closed = speechController.Continue();
            }
        }

        protected override void PerformOnStateExit()
        {
        }

        public override bool Complete()
        {
            return _closed;
        }
    }
}
