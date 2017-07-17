using Assets.Standard_Assets._2D.Scripts.Managers;
using Assets.Standard_Assets.Extensions;
using CommonInterfaces.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Checkpoints.Scripts
{
    public class CheckpointTrigger : MonoBehaviour, ICheckpoint
    {
        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(ValidateCheckpoint(collision))
            {
                CheckpointManager.Instance.SetCheckpoint(transform.position, this);
            }
        }

        private bool ValidateCheckpoint(Collider2D collision)
        {
            if (CheckpointManager.Instance.CurrentCheckpoint == this)
            {
                return false;
            }
            if (!collision.IsPlayer())
            {
                return false;
            }
            return true;
        }
    }
}
