using CommonInterfaces.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Managers
{
    public class CheckpointManager : Singleton<CheckpointManager>
    {
        protected CheckpointManager() { }

        public Vector3? CurrentCheckpointPosition { get; set; }
    }
}
