using Assets.Standard_Assets._2D.Scripts.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Friendly.Quest_Giver.Scripts
{
    public enum QuestMarkerStatus
    {
        QuestAvailable,
        QuestIncomplete,
        QuestQuestReadyToDeliver
    }

    public class QuestMarkerNotification : MonoBehaviour
    {
        [SerializeField]
        private InteractionNotification _notification;
        [SerializeField]
        private Sprite _questReadyToDeliver;
        [SerializeField]
        private Sprite _questIncomplete;
        [SerializeField]
        private Sprite _questAvailableSprite;

        public void ShowQuestAvailableMarker()
        {
            _notification.Show(_questAvailableSprite);
        }

        public void ShowQuestIncompleteMarker()
        {
            _notification.Show(_questIncomplete);
        }

        public void ShowQuestCompleteMarker()
        {
            _notification.Show(_questReadyToDeliver);
        }
        public void Hide()
        {
            _notification.Hide();
        }
    }
}
