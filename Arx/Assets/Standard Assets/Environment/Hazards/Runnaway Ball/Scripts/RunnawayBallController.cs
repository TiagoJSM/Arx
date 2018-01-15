using Assets.Standard_Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Runnaway_Ball.Scripts
{
    public class RunnawayBallController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _roll;
        [SerializeField]
        private AudioSource[] _impact;

        public void StartPlatformRoll()
        {
            _roll.Play();
        }

        public void StopPlatformRoll()
        {
            _roll.Stop();
        }

        public void Impact()
        {
            _impact.PlayRandom();
        }
    }
}
