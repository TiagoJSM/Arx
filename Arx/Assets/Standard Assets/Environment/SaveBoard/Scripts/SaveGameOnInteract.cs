using CommonInterfaces.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces;
using Assets.Standard_Assets._2D.Scripts.Game_State;
using Assets.Standard_Assets._2D.Scripts.Managers;

namespace Assets.Standard_Assets.Environment.SaveBoard.Scripts
{
    public class SaveGameOnInteract : MonoBehaviour, IInteractionTriggerController
    {
        public event OnInteract OnInteract;
        public event OnStopInteraction OnStopInteraction;

        public void Interact(GameObject interactor)
        {
            var saveData = ArxSaveData.Current;
            saveData.PlayerPosition = transform.position;
            ArxGameSaveHandler.Save();
        }

        public void StopInteraction()
        {
        }
    }
}
