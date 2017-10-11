using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace Assets.Standard_Assets._2D.Scripts.Footsteps
{
    public class FootstepPlayer
    {
        [SerializeField]
        private AudioSource[] _footsteps;

        public void PlayFootsteps()
        {
            var footstep = _footsteps.Random();
            footstep.Play();
        }
    }
}
