using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.QuestStructures
{
    public class QuestLogComponent : MonoBehaviour
    {
        private QuestLog _questLog;
        
        void Start()
        {
            var subscriber = this.gameObject.GetComponent<IQuestSubscriber>();
            _questLog = new QuestLog(subscriber);
        }
    }
}
