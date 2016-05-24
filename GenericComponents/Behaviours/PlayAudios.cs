using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    [Serializable]
    public class AudioClipData
    {
        public AudioClip sound;
        [Range(0, 1)]
        public float volume = 1;
    }

    public class PlayAudios : MonoBehaviour
    {
        public AudioClipData[] clips;
        [Range(0, 1)]
        public float masterVolume = 1;

        public void Play(int index)
        {
            var clipData = clips[index];
            AudioSource.PlayClipAtPoint(clipData.sound, this.transform.position, clipData.volume * masterVolume);
        }
    }

}
