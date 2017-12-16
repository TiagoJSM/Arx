using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Enemy_Activation_Trigger.Scripts
{
    public delegate void OnActivate(EnemyActivationTrigger trigger);

    public class EnemyActivationTrigger : MonoBehaviour
    {
        private bool _activated;

        [SerializeField]
        private BasePlatformerController[] _characters;

        public event OnActivate OnActivate;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_activated)
            {
                return;
            }
            if(!collision.isTrigger && collision.gameObject.IsPlayer())
            {
                for(var idx = 0; idx < _characters.Length; idx++)
                {
                    _characters[idx].gameObject.SetActive(true);
                }
                _activated = true;
                if(OnActivate != null)
                {
                    OnActivate(this);
                }
            }
        }
    }
}
