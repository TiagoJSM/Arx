using CommonInterfaces.Inventory;
using System;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem
{
	public delegate void OnKill(GameObject obj);
	public delegate void OnInventoryAdd(IInventoryItem obj);
    public delegate void OnInventoryRemove(IInventoryItem obj);
}

