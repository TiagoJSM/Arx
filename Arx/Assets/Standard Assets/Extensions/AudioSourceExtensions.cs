using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Extensions
{
    public static class AudioSourceExtensions
    {
        public static void PlayRandom(this AudioSource[] sources)
        {
            if(sources.Length > 0)
            {
                var source = sources.Random();
                source.Play();
            }
        }

        public static IEnumerator FadeOut(this AudioSource audioSource, float fadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

        public static IEnumerator FadeIn(this AudioSource audioSource, float fadeTime)
        {
            float startVolume = 0.2f;

            audioSource.volume = 0;
            audioSource.Play();

            while (audioSource.volume < 1.0f)
            {
                audioSource.volume += startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }

            audioSource.volume = 1f;
        }
    }
}
