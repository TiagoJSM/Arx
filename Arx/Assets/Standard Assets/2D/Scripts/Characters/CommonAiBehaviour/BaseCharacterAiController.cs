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
        protected void SetActiveCoroutine(IEnumerator coroutine)
        {
            StopActiveCoroutine();
            StartCoroutine(coroutine);
        }

        protected void StopActiveCoroutine()
        {
            StopAllCoroutines();
        }
    }
}
