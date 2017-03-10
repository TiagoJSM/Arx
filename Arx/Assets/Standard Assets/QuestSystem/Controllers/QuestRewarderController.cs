using Assets.Standard_Assets.Characters.Friendly.Quest_Giver.Scripts;
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
        private QuestMarkerNotification _notification;
        [SerializeField]
        private Quest _quest;

        private void Awake()
        {
            _interactible = GetComponent<InteractibleCharacterController>();
            _interactible.OnInteractionComplete += GiveReward;
        }

        private void Update()
        {
            var questSubscriber = FindObjectOfType<QuestLogComponent>();
            var quest = questSubscriber.GetQuest(_quest.questId);
            

            if(quest.QuestStatus == QuestStatus.Complete)
            {
                _notification.Hide();
            }
            else if (quest.QuestStatus == QuestStatus.Active && quest.AllTasksComplete)
            {
                _notification.ShowQuestCompleteMarker();
            }
            else if (quest.QuestStatus == QuestStatus.Active && !quest.AllTasksComplete)
            {
                _notification.ShowQuestIncompleteMarker();
            }
        }

        private void GiveReward(GameObject interactor)
        {
            var quest = interactor.GetComponent<QuestLogComponent>().GetQuest(_quest.questId);
            if (quest.AllTasksComplete)
            {
                quest.Complete();
            }
        }
    }
}
