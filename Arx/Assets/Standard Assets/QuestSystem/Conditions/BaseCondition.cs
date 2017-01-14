using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Inventory;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Conditions
{
    [Serializable]
    public abstract class BaseCondition : ICondition
    {
        public string conditionName;
        public string conditionDescription;

        public abstract bool Complete {get; }
    }
}
