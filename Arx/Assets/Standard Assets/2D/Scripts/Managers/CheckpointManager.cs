using Assets.Standard_Assets._2D.Scripts.Game_State;
using CommonInterfaces.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Managers
{
    public class CheckpointManager : Singleton<CheckpointManager>
    {
        protected CheckpointManager() { }

        //The separation between CurrentCheckpoint and CurrentCheckpointPosition is because we may not have a checkpoint
        //But we can still have a checkpoint position, like when the game starts we may want to place the character in different places
        //According to game saved state
        public ICheckpoint CurrentCheckpoint { get; private set; }
        public Vector3? CurrentCheckpointPosition { get; private set; }
        public GameState CheckpointData { get; private set; }

        public void SetCheckpoint(Vector3? position, ICheckpoint checkpoint = null, GameState checkpointData = null)
        {
            CurrentCheckpointPosition = position;
            CheckpointData = checkpointData ?? GameStateManager.Instance.GameStateClone;
            CurrentCheckpoint = checkpoint;
        }

        public void ReloadGameState()
        {
            var gameStateManager = GameStateManager.Instance;

            gameStateManager.GameStateClone = 
                CheckpointData == null
                    ? gameStateManager.DefaultGameState
                    : CheckpointData;
        }
    }
}
