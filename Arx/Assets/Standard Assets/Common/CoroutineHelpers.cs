using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Common
{
    public static class CoroutineHelpers
    {
        public static IEnumerator MoveTo(Vector3 start, Vector3 end, float time, Transform obj, Action onEnd = null)
        {
            var elapsed = 0.0f;

            while (time > elapsed)
            {
                obj.position = Vector3.Lerp(start, end, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            obj.position = end;
            if(onEnd != null)
            {
                onEnd();
            }
        }
    }
}
