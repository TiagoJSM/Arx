using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Conditions
{
    [Serializable]
    public class OnEnemyKilledCondition : ICondition
    {
        public int killCount;

        public bool Complete { get; set; }
    }
}
