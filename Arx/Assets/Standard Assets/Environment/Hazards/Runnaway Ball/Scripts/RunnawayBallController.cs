using Assets.Standard_Assets.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Runnaway_Ball.Scripts
{
    public class RunnawayBallController : MonoBehaviour
    {
        private Coroutine _routine;

        [SerializeField]
        private AudioSource _roll;
        [SerializeField]
        private AudioSource[] _impact;

        public void StartPlatformRoll()
        {
            StartNewRoutine(_roll.FadeIn(0.2f));
        }

        public void StopPlatformRoll()
        {
            StartNewRoutine(_roll.FadeOut(0.5f));
        }

        public void Impact()
        {
            _impact.PlayRandom();
        }

        private void StartNewRoutine(IEnumerator routine)
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }
            _routine = StartCoroutine(routine);
        }
    }
}
