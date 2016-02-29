using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.Zones
{
    public class PlaySequenceOnPlayerTrigger : MonoBehaviour
    {
        private bool played = false;

        public AnimationSequenceBehaviour animationSequence;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (played)
            {
                return;
            }
            animationSequence.Run();
            played = true;
        }
    }
}
