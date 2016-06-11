using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.Director;

namespace GenericComponents.Animation.Playables
{
    class CrossFadePlayable : AnimationPlayable
    {
        private float m_TransitionDuration = 1.0f;
        private float m_TransitionTime = Mathf.Infinity;
        private float m_TransitionStart = 0.0f;

        private bool m_InTransition = false;

        private AnimationMixerPlayable mixer;

        public CrossFadePlayable()
        {
            mixer = new AnimationMixerPlayable();
            Playable.Connect(mixer, this);
            mixer.AddInput(new AnimationPlayable());
            mixer.AddInput(new AnimationPlayable());
        }

        public void Play(AnimationPlayable playable)
        {
            Playable.Connect(playable, mixer, 0, 0);
        }

        public void Crossfade(AnimationPlayable playable, float transitionDuration)
        {
            Playable.Connect(playable, mixer, 0, 1);
            m_TransitionDuration = transitionDuration;
            m_TransitionTime = 0.0f;
            m_TransitionStart = Time.time;
            m_InTransition = true;
        }
        public override void PrepareFrame(FrameData info)
        {
            if (m_TransitionTime <= m_TransitionDuration)
            {
                m_TransitionTime = Time.time - m_TransitionStart;
                float weight = Mathf.Clamp01(m_TransitionTime / m_TransitionDuration);
                mixer.SetInputWeight(0, 1.0f - weight);
                mixer.SetInputWeight(1, weight);
            }
            else if (m_InTransition)
            {
                AnimationPlayable destinationPlayable = mixer.GetInput(1) as AnimationPlayable;
                mixer.RemoveAllInputs();
                Playable.Connect(destinationPlayable, mixer, 0, 0);
                mixer.SetInputWeight(0, 1.0f);
                m_InTransition = false;
            }
        }
    }
}
