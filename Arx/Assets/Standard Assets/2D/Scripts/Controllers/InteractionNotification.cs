using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    public class InteractionNotification : MonoBehaviour
    {
        private SpriteRenderer _interactionButton;

        [SerializeField]
        private InteractibleCharacterController _interactibleCharacter;

        private void Start()
        {
            _interactibleCharacter.OnInteract += HideNotification;
            _interactibleCharacter.OnInteractionComplete += ShowInteraction;
            _interactibleCharacter.OnStopInteraction += ShowInteraction;
        }

        private void OnDestroy()
        {
            _interactibleCharacter.OnInteract -= HideNotification;
            _interactibleCharacter.OnInteractionComplete -= ShowInteraction;
            _interactibleCharacter.OnStopInteraction -= ShowInteraction;
        }

        private void ShowInteraction(GameObject interactor)
        {
            ShowInteraction();
        }

        private void ShowInteraction()
        {
            _interactionButton.enabled = true;
        }

        private void HideNotification(GameObject interactor)
        {
            _interactionButton.enabled = false;
        }
    }
}
