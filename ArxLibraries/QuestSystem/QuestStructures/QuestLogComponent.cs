using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QuestSystem.QuestStructures
{
    public class QuestLogComponent : MonoBehaviour
    {
        private QuestLog _questLog;
        
        void Start()
        {
            //ToDo: might not work, if so extension methods need to be created to sort this out
            var subscriber = this.gameObject.GetComponent<IQuestSubscriber>();
            _questLog = new QuestLog(subscriber);
        }
    }
}
