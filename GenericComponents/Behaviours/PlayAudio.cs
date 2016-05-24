using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    public class PlayAudio : MonoBehaviour
    {
        public AudioClip sound;
        [Range(0, 1)]
        public float volume = 1;

        public void Play()
        {
            AudioSource.PlayClipAtPoint(sound, this.transform.position, volume);
        }
    }
}
