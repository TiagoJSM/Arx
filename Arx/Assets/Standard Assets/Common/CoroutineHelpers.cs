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

        public static IEnumerator Flash(Action onEnd, params GameObject[] gos)
        {
            return Flash(0.3f, 0.6f, 3.5f, onEnd, gos);
        }

        public static IEnumerator Flash(float fadeOutPeriod, float fadeInPeriod, float duration, Action onEnd, params GameObject[] gos)
        {
            var startTime = Time.time;
            while (true)
            {
                SetActive(gos, false);
                yield return new WaitForSeconds(fadeOutPeriod);
                SetActive(gos, true);
                yield return new WaitForSeconds(fadeInPeriod);

                var delta = Time.time - startTime;
                if(delta >= duration)
                {
                    break;
                }
            }
            onEnd();
        }

        public static IEnumerator Flash(Action onEnd, params Renderer[] sprites)
        {
            return Flash(0.3f, 0.6f, 3.5f, onEnd, sprites);
        }

        public static IEnumerator Flash(float fadeOutPeriod, float fadeInPeriod, float duration, Action onEnd, params Renderer[] sprites)
        {
            var startTime = Time.time;
            while (true)
            {
                var delta = 0.0f;
                while(delta < fadeOutPeriod)
                {
                    SetColor(sprites, 1.0f, 0.4f, fadeOutPeriod / delta);
                    delta += Time.deltaTime;
                    yield return null;
                }
                delta = 0.0f;
                while (delta < fadeInPeriod)
                {
                    SetColor(sprites, 0.4f, 1.0f, fadeInPeriod / delta);
                    delta += Time.deltaTime;
                    yield return null;
                }

                var elapsed = Time.time - startTime;
                if (elapsed >= duration)
                {
                    break;
                }
            }
            onEnd();
        }

        private static void SetColor(Renderer[] sprites, float a, float b, float t)
        {
            var alpha = Mathf.Lerp(a, b, t);
            for (var idx = 0; idx < sprites.Length; idx++)
            {
                var color = sprites[idx].material.color;
                color.a = 0.4f;
                sprites[idx].material.color = color;
            }
        }

        public static IEnumerator FollowTargetCoroutine(
            Transform self, 
            GameObject target, 
            Action<float> horizontalMove,
            Func<bool> isTargetInRange)
        {
            if (target == null)
            {
                yield break;
            }

            while (true)
            {
                if (isTargetInRange())
                {
                    yield break;
                }
                var currentPosition = self.position;
                var xDifference = target.transform.position.x - currentPosition.x;
                horizontalMove(xDifference);
                yield return null;
            }
        }

        private static void SetActive(GameObject[] gos, bool active)
        {
            for(var idx = 0; idx < gos.Length; idx++)
            {
                gos[idx].SetActive(active);
            }
        }
    }
}
