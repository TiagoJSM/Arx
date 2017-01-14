using CommonInterfaces.Inventory;
using System;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Conditions
{
    public interface ICondition
    {
        bool Complete { get; }
	}
}

