using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using GenericComponents.Controllers.Characters;

namespace Assets.Standard_Assets._2D.Scripts.Characters
{
    public delegate void OnCharacterFound(GenericComponents.Controllers.Characters.BasePlatformerController controller);

    public class CharacterFinder : MonoBehaviour
    {
        public event OnCharacterFound OnCharacterFound;

        public LayerMask characterLayer;

        void OnTriggerEnter2D(Collider2D collider)
        {
            if(OnCharacterFound == null)
            {
                return;
            }
            var controller = collider.GetComponent<GenericComponents.Controllers.Characters.BasePlatformerController>();
            if (controller == null)
            {
                return;
            }
            if (characterLayer.IsInAnyLayer(controller.gameObject))
            {
                OnCharacterFound(controller);
            }
        }
    }
}
