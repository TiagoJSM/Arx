using ArxGame.GameSave;
using GenericComponents.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.LevelManagers
{
    public class DreamingBeyondImaginationManager : MonoBehaviour
    {
        public GameObject startSceneTrigger;
        public GameObject arx;

        public void IntroScenePlayed()
        {
            var saveData = ArxSaveData.Current;
            saveData.DreamingBeyondImagination.IntroPlayed = true;
            saveData.PlayerPosition = arx.transform.position;
            ArxGameSaveHandler.Save();
        }

        void Awake()
        {
            var saveData = ArxSaveData.Current;
            CheckpointManager.Instance.CurrentCheckpointPosition = saveData.PlayerPosition;

            if (saveData.DreamingBeyondImagination.IntroPlayed)
            {
                startSceneTrigger.SetActive(false);
            }
        }
    }
}
