using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Zones
{
    public abstract class BaseZone : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<IPlayerControl>();
            if (player == null)
            {
                return;
            }
            OnZoneEnter();
        }

        protected abstract void OnZoneEnter();
    }
}
