using Assets.Standard_Assets._2D.Scripts.Game_State;
using Assets.Standard_Assets._2D.Scripts.Managers;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.Main_Menu.Scripts
{
    public class MainMenuManager : MonoBehaviour
    {
        [SceneField]
        [SerializeField]
        private SceneField _scene;
        public Button loadGame;

        public void NewGame()
        {
            ArxSaveData.Current = new ArxSaveData();
            ArxGameSaveHandler.Save(_scene.SceneName);
            LevelManager.Instance.GoToScene(ArxSaveData.Current.LevelName);
        }

        public void LoadGame()
        {
            ArxSaveData.Current = ArxGameSaveHandler.savedGames.First();
            GameStateManager.Instance.GameStateClone = ArxSaveData.Current.GameState;
            var playerPosition = ArxSaveData.Current.PlayerPosition;
            if(playerPosition == null)
            {
                LevelManager.Instance.GoToScene(ArxSaveData.Current.LevelName);
            }
            else
            {
                LevelManager.Instance.GoToScene(ArxSaveData.Current.LevelName, playerPosition.Value);
            }
        }

        void Awake()
        {
            ArxGameSaveHandler.Load();
            loadGame.interactable = ArxGameSaveHandler.savedGames.Count != 0;
        }
    }
}
