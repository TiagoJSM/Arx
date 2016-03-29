using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utils
{
    public static class ObjectUtils
    {
        public static void DisableAllBehaviours<T>()
        {
            var behaviours =
                UnityEngine.Object
                    .FindObjectsOfType<MonoBehaviour>()
                    .Where(b => b.GetComponent<T>() != null)
                    .ToArray();
            foreach (var behaviour in behaviours)
            {
                behaviour.enabled = false;
            }
        }

        public static void DisableAllBehaviours<T>(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                DisableAllBehaviours<T>(gameObject);
            }
        }

        public static void DisableAllBehaviours<T>(GameObject gameObject)
        {
            var behaviours = gameObject.GetComponents<T>().OfType<MonoBehaviour>().ToArray();
            foreach (var behaviour in behaviours)
            {
                behaviour.enabled = false;
            }
        }

        public static void EnableAllBehaviours<T>()
        {
            var behaviours =
                Resources.
                    FindObjectsOfTypeAll<MonoBehaviour>()
                    .Where(b => b.GetComponent<T>() != null)
                    .ToArray();
            foreach (var behaviour in behaviours)
            {
                behaviour.enabled = true;
            }
        }

        public static void EnableAllBehaviours<T>(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                EnableAllBehaviours<T>(gameObject);
            }
        }

        public static void EnableAllBehaviours<T>(GameObject gameObject)
        {
            var behaviours = gameObject.GetComponents<T>().OfType<MonoBehaviour>().ToArray();
            foreach (var behaviour in behaviours)
            {
                behaviour.enabled = true;
            }
        }
    }
}
