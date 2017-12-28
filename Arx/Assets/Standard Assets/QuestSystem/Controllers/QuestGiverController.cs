using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets.Characters.Friendly.Quest_Giver.Scripts;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using GenericComponents.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Controllers
{
    [RequireComponent(typeof(InteractibleCharacterController))]
    public class QuestGiverController : MonoBehaviour
    {
        private InteractibleCharacterController _interactible;
        private IQuestSubscriber _questSubscriber;

        [SerializeField]
        private QuestMarkerNotification _notification;
        [SerializeField]
        private InteractiveDialogComponent _questExplanationDialog;
        [SerializeField]
        private InteractiveDialogComponent _duringQuestDialog;
        [SerializeField]
        private InteractiveDialogComponent _afterQuestDialog;

        public Quest quest;

        // Use this for initialization
        private void Start()
        {
            _interactible = GetComponent<InteractibleCharacterController>();
            _interactible.OnInteractionComplete += GiveQuest;
        }

        private void AssignQuestToSubscriber(IQuestSubscriber subscriber)
        {
            var subscriberQuest = subscriber.GetQuest(quest.questId);
            if (subscriberQuest.QuestStatus != QuestStatus.Inactive)
            {
                return;
            }
            _notification.Hide();
            subscriber.AssignQuest(quest);
        }

        private void Update()
        {
            var questSubscriber = FindObjectOfType<QuestLogComponent>();
            SetAppropriateText(questSubscriber.gameObject);
            SetAppropriateNotification(questSubscriber);
        }

        private void SetAppropriateNotification(QuestLogComponent questSubscriber)
        {
            var subscriberQuest = questSubscriber.GetQuest(quest.questId);
            if(subscriberQuest.QuestStatus == QuestStatus.Inactive)
            {
                _notification.ShowQuestAvailableMarker();
            }
            else
            {
                _notification.Hide();
            }
        }

        private void SetAppropriateText(GameObject interactor)
        {
            _questSubscriber = interactor.GetComponent<IQuestSubscriber>();
            if (_questSubscriber == null)
            {
                _interactible.Dialog = _questExplanationDialog;
                return;
            }
            var subscriberQuest = _questSubscriber.GetQuest(quest.questId);
            if (subscriberQuest.QuestStatus == QuestStatus.Inactive)
            {
                _interactible.Dialog = _questExplanationDialog;
                return;
            }
            var tasksComplete = subscriberQuest.AllTasksComplete;
            if (subscriberQuest.QuestStatus == QuestStatus.Active && !tasksComplete)
            {
                _interactible.Dialog = _duringQuestDialog;
                return;
            }
            _interactible.Dialog = _afterQuestDialog;
        }

        private void GiveQuest(GameObject interctor)
        {
            if (_questSubscriber != null)
            {
                AssignQuestToSubscriber(_questSubscriber);
            }
        }
    }
}
