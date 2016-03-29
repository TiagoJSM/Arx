using ArxGame.GameSave;
using GenericComponents.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ArxGame.LevelManagers.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public string newGameLevel;
        public Button loadGame;

        public void NewGame()
        {
            ArxSaveData.Current = new ArxSaveData();
            ArxGameSaveHandler.Save(newGameLevel);
            LevelManager.Instance.GoToScene(newGameLevel);
        }

        public void LoadGame()
        {
            ArxSaveData.Current = ArxGameSaveHandler.savedGames.First();
            LevelManager.Instance.GoToScene(ArxSaveData.Current.LevelName);
        }

        void Awake()
        {
            ArxGameSaveHandler.Load();
            loadGame.interactable = ArxGameSaveHandler.savedGames.Count != 0;
        }
    }
}
