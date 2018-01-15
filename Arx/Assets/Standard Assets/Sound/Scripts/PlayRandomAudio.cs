using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Sound.Scripts
{
    public class PlayRandomAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioSource[] _clips;

        public void Play()
        {
            var idx = UnityEngine.Random.Range(0, _clips.Length);
            _clips[idx].Play();
        }
    }
}
