using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GenericComponents.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        protected LevelManager() { }

        public void RestartCurrentLevelFromCheckpoint()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GoToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
