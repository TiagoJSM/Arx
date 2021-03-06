﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Game_State
{
    [Serializable]
    public class GameState
    {
        public QuestState[] QuestsStates { get; set; }
        public ItemState[] ItemsStates { get; set; }
        public List<string> OpenChests { get; set; }

        public Watershed Watershed { get; set; }

        public GameState()
        {
            QuestsStates = new QuestState[0];
            ItemsStates = new ItemState[0];
            Watershed = new Watershed();
            OpenChests = new List<string>();
        }
    }
}
