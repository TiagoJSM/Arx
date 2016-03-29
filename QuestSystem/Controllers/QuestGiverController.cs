using CommonInterfaces.Controllers.Interaction;
using GenericComponents.Controllers.Interaction;
using QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QuestSystem.Controllers
{
    public class QuestGiverController : MonoBehaviour
    {
        private IInteractionTriggerController _interactionTriggerController;
        private SpeechController _speechController;
        private GameObject _interactor;

        [SerializeField]
        [TextArea(3, 10)]
        private string _questExplanationDialog;
        [SerializeField]
        [TextArea(3, 10)]
        private string _duringQuestDialog;
        [SerializeField]
        [TextArea(3, 10)]
        private string _afterQuestDialog;

        public Quest quest;

        // Use this for initialization
        void Start()
        {
            _interactionTriggerController = gameObject.GetComponent<IInteractionTriggerController>();
            _speechController = GetComponentInChildren<SpeechController>();
            _interactionTriggerController.OnInteract += OnInteractHandler;
            _interactionTriggerController.OnStopInteraction += OnStopInteractionHandler;
            _speechController.OnScrollEnd += OnScrollEndHandler;
        }

        public void AssignQuestToSubscriber(IQuestSubscriber subscriber)
        {
            if (subscriber.HasQuest(quest))
            {
                return;
            }
            subscriber.AssignQuest(quest);
        }

        private void OnInteractHandler(GameObject interactor)
        {
            if (_interactor != null)
            {
                _interactor = interactor;
            }
            if (!_speechController.Visible)
            {
                SetAppropriateText(interactor);
                _speechController.Reset();
                _speechController.Visible = true;
                return;
            }
            _speechController.ScrollPageDown();
        }

        private void OnStopInteractionHandler()
        {
            _speechController.Visible = false;
            _interactor = null;
        }

        private void OnScrollEndHandler()
        {
            var questSubscriber = _interactor.GetComponent<IQuestSubscriber>();
            if (questSubscriber != null)
            {
                AssignQuestToSubscriber(questSubscriber);
            }
            _interactionTriggerController.StopInteraction();
        }

        private void SetAppropriateText(GameObject interactor)
        {
            var questSubscriber = interactor.GetComponent<IQuestSubscriber>();
            if (questSubscriber == null)
            {
                _speechController.Text = _questExplanationDialog;
                return;
            }
            var subscriberQuest = questSubscriber.GetQuest(quest.name);
            if (subscriberQuest.QuestStatus == QuestStatus.Inactive)
            {
                _speechController.Text = _questExplanationDialog;
                return;
            }
            if (subscriberQuest.QuestStatus == QuestStatus.Active)
            {
                _speechController.Text = _duringQuestDialog;
                return;
            }
            _speechController.Text = _afterQuestDialog;
        }
    }
}
