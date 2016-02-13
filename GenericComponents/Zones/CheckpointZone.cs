using CommonInterfaces.Management;
using GenericComponents.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Zones
{
    public class CheckpointZone : BaseZone, ICheckpoint
    {
        public Vector3 Position
        {
            get
            {
                return gameObject.transform.position;
            }
        }

        protected override void OnZoneEnter()
        {
            CheckpointManager.Instance.CurrentCheckpointPosition = this.Position;
        }
    }
}
