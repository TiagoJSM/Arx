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

        protected LevelManager()
        {
            SceneManager.sceneLoaded += SceneLoadedHandler;
        }

        public void RestartCurrentLevelFromCheckpoint()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GoToScene(string sceneName, string location = null)
        {
            _location = location;
            SceneManager.LoadScene(sceneName);
        }

        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadMode)
        {
            if (string.IsNullOrEmpty(_location))
            {
                return;
            }
            var gameObject = GameObject.Find(_location);
            if (gameObject == null)
            {
                Debug.LogError("Location was not found.");
                return;
            }
            var players = GameObject.FindGameObjectsWithTag("Player");
            for (var idx = 0; idx < players.Length; idx++)
            {
                players[idx].transform.position = gameObject.transform.position;
            }
        }
    }
}
