using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.States.ControlStates
{
    [Serializable]
    public class ChangeActiveState : BaseSequenceState
    {
        [SerializeField]
        public GameObject[] gameObjects;
        public bool activeStateToSet;

        protected override void PerformOnStateEnter()
        {
            for (var idx = 0; idx < gameObjects.Length; idx++)
            {
                gameObjects[idx].SetActive(activeStateToSet);
            }
        }

        public override bool Complete()
        {
            return true;
        }
    }
}
