using Assets.Standard_Assets._2D.Scripts.Game_State;
using Assets.Standard_Assets.InventorySystem;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Standard_Assets.Extensions;
using Assets.Standard_Assets.InventorySystem.InventoryObjects;

namespace Assets.Standard_Assets._2D.Scripts.Managers
{
    public class GameStateManager : Singleton<GameStateManager>
    {
        private const string PLAYER_TAG = "Player";

        private GameState _state;

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            var tmp = Instance;
        }

        public GameState GameState
        {
            get
            {
                StoreGameState();
                return _state.DeepClone();
            }
            set
            {
                AssignGameState();
                _state = value.DeepClone();
            }
        }

        protected GameStateManager()
        {
        }

        private void Awake()
        {
            var levelManager = LevelManager.Instance;
            if (_state == null)
            {
                _state = new GameState();
            }
            levelManager.BeforeSceneLoad += StoreGameState;
            levelManager.OnSceneLoaded += AssignGameState;
        }

        private void StoreGameState()
        {
            var playerGo = GameObject.FindGameObjectWithTag(PLAYER_TAG);
            if(playerGo == null)
            {
                return;
            }
            StoreQuests(playerGo);
            StoreItems(playerGo);
        }

        private void AssignGameState()
        {
            var playerGo = GameObject.FindGameObjectWithTag(PLAYER_TAG);
            if (playerGo == null)
            {
                return;
            }
            RestoreQuests(playerGo);
            RestoreItems(playerGo);
        }

        private void RestoreItems(GameObject playerGo)
        {
            var component = playerGo.GetComponent<InventoryComponent>();
            if (component == null)
            {
                return;
            }
            var resourceItems = Resources.LoadAll<InventoryItem>("Items");
            var itemStates = _state.ItemsStates;
            var items = new InventoryItems[itemStates.Length];
            for(var idx = 0; idx < itemStates.Length; idx++)
            {
                var itemState = itemStates[idx];
                var resource = resourceItems.First(r => r.Id == itemState.ItemId);
                items[idx] = new InventoryItems()
                {
                    Count = itemState.Quantity,
                    Item = Instantiate(resource)
                };
            }
            component.Inventory.SetItems(items);
        }

        private void RestoreQuests(GameObject playerGo)
        {
            var questLog = playerGo.GetComponent<QuestLogComponent>();
            if (questLog == null)
            {
                return;
            }
            questLog.SetQuestsStates(_state.QuestsStates);
        }

        private void StoreQuests(GameObject playerGo)
        {
            var questLog = playerGo.GetComponent<QuestLogComponent>();
            if(questLog == null)
            {
                return;
            }
            _state.QuestsStates =
                questLog
                    .GetQuests()
                    .Select(quest =>
                        new QuestState()
                        {
                            QuestId = quest.questId,
                            QuestStatus = quest.QuestStatus,
                            Tasks = quest.tasks.ToArray().DeepClone()
                        })
                    .ToArray();
        }

        private void StoreItems(GameObject playerGo)
        {
            var component = playerGo.GetComponent<InventoryComponent>();
            if (component == null)
            {
                return;
            }
            _state.ItemsStates = 
                component
                    .Inventory
                    .InventoryItems
                    .Select(item => 
                        new ItemState()
                        {
                            ItemId = item.Item.Id,
                            Quantity = item.Count
                        })
                    .ToArray();
        }
    }
}
