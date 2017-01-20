using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Controllers
{
    [RequireComponent(typeof(InteractibleCharacterController))]
    public class QuestRewarderController : MonoBehaviour
    {
        private InteractibleCharacterController _interactible;

        [SerializeField]
        private Quest _quest;

        private void Awake()
        {
            _interactible.OnInteractionComplete += GiveReward;
        }

        private void GiveReward(GameObject interactor)
        {
            var quest = interactor.GetComponent<QuestLogComponent>().GetQuest(_quest.questId);
            quest.GiveReward();
        }
    }
}
