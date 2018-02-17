using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.CommonAiBehaviour
{
    public class BaseCharacterAiController : MonoBehaviour
    {
        private Coroutine _active;

        protected void SetActiveCoroutine(IEnumerator coroutine)
        {
            StopActiveCoroutine();
            _active = StartCoroutine(WrapperRoutine(coroutine));
        }

        protected void StopActiveCoroutine()
        {
            if(_active != null)
            {
                StopCoroutine(_active);
            }
        }

        private IEnumerator WrapperRoutine(IEnumerator coroutine)
        {
            yield return coroutine;
            _active = null;
        }
    }
}
