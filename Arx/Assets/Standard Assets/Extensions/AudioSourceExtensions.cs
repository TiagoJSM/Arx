using Extensions;
using System;
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
    }
}
