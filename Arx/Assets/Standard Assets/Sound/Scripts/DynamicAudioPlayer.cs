using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Sound.Scripts
{
    [Serializable]
    public struct AudioConfiguration
    {
        public AudioSource audio;
        public float minPlayInterval;
        public float maxPlayInterval;
    }

    public class DynamicAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioConfiguration[] sounds;

        private void Start()
        {
            for(var idx = 0; idx < sounds.Length; idx++)
            {
                StartCoroutine(SoundRoutine(sounds[idx]));
            }
        }

        private IEnumerator SoundRoutine(AudioConfiguration config)
        {
            while (true)
            {
                if (!config.audio.isPlaying)
                {
                    config.audio.Play();
                }

                var waitTime = UnityEngine.Random.Range(config.minPlayInterval, config.maxPlayInterval);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }
}
