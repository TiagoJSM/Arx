using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Standard_Assets._2D.Scripts.Managers
{
    public class ListenerLevelFadeManager : Singleton<ListenerLevelFadeManager>
    {
        private Coroutine _listenerVolumeRoutine;

        private void LevelWasLoaded(Scene scene, LoadSceneMode mode)
        {
            FadeInListener();
        }

        private void FadeInListener()
        {
            _listenerVolumeRoutine = StartCoroutine(ListenerVolumeFade(0, 1));
        }

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            
            var manager = Instance;
            SceneManager.sceneLoaded += manager.LevelWasLoaded;
            //First time needs to be called on instantiation, for test level purposes
            manager.FadeInListener();
        }

        private IEnumerator ListenerVolumeFade(float startVolume, float targetVolume, float time = 2)
        {
            var elapsed = 0.0f;

            while(elapsed < time)
            {
                AudioListener.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }

            AudioListener.volume = targetVolume;
        }
    }
}
