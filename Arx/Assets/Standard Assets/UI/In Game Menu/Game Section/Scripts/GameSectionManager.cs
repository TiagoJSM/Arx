using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Standard_Assets.UI.Game_Section.Scripts
{
    public class GameSectionManager : MonoBehaviour
    {
        public void ReloadedLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
