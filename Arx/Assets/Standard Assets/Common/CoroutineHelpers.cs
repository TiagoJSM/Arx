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

        public static IEnumerator DeathMovement(
            GameObject go, 
            float horizontalDirection,
            Action onEnd = null,
            float horizontalSpeed = 10,
            float verticalUpSpeed = 18,
            float verticalDownSpeed = -20)
        {
            horizontalDirection = Mathf.Sign(horizontalDirection);

            var startTime = Time.time;
            while (true)
            {
                var elapsed = Time.time - startTime;
                if (elapsed > 0.7)
                {
                    break;
                }
                go.transform.position += new Vector3(horizontalDirection * horizontalSpeed, verticalUpSpeed, 0) * Time.deltaTime;
                yield return null;
            }

            startTime = Time.time;
            while (true)
            {
                var elapsed = Time.time - startTime;
                if (elapsed > 5)
                {
                    break;
                }
                go.transform.position += new Vector3(horizontalDirection * horizontalSpeed, verticalDownSpeed, 0) * Time.deltaTime;
                yield return null;
            }
            if (onEnd != null)
            {
                onEnd();
            }
        }
    }
}
