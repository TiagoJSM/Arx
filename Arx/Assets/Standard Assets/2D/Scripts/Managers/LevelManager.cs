using Assets.Standard_Assets.UI.Loading.Screen_Transition.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Standard_Assets._2D.Scripts.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        private string _location;
        private Vector3? _position;
        private ScreenTransitionController _screenTransition;

        protected LevelManager()
        {
            SceneManager.sceneLoaded += SceneLoadedHandler;
        }

        public event Action BeforeSceneLoad;
        public event Action OnSceneLoaded;

        public void RestartCurrentLevelFromCheckpoint()
        {
            var checkpointManager = CheckpointManager.Instance;
            _position = checkpointManager.CurrentCheckpointPosition;
            checkpointManager.ReloadGameState();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GoToScene(string sceneName, string location = null)
        {
            if(BeforeSceneLoad != null)
            {
                BeforeSceneLoad();
            }
            _location = location;
            SceneManager.LoadScene(sceneName);
        }

        public void GoToScene(string sceneName, Vector3 position)
        {
            if (BeforeSceneLoad != null)
            {
                BeforeSceneLoad();
            }
            _position = position;
            SceneManager.LoadScene(sceneName);
        }

        private void Awake()
        {
            _screenTransition = Instantiate(Resources.Load<ScreenTransitionController>("Screen Transition/Screen Transition"));
            DontDestroyOnLoad(_screenTransition);
            _screenTransition.FadeIn();
        }

        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadMode)
        {
            if (!string.IsNullOrEmpty(_location))
            {
                var gameObject = GameObject.Find(_location);
                if (gameObject == null)
                {
                    Debug.LogError("Location was not found.");
                }
                else
                {
                    var player = GetPlayer();
                    player.transform.position = gameObject.transform.position;
                }
            }
            else if(_position != null)
            {
                var player = GetPlayer();
                player.transform.position = _position.Value;
            }
            if (OnSceneLoaded != null)
            {
                OnSceneLoaded();
            }
            _screenTransition.FadeIn();
        }

        private GameObject GetPlayer()
        {
            return GameObject.FindGameObjectWithTag("Player");
        }
    }
}
