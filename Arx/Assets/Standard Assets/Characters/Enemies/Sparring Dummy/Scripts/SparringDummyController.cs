using Assets.Standard_Assets.QuestSystem.Attributes;
using Assets.Standard_Assets.QuestSystem.EditorProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces.Controllers;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.QuestSystem.Tasks;

namespace Assets.Standard_Assets.Characters.Enemies.Sparring_Dummy.Scripts
{
    public class SparringDummyController : PlatformerCharacterController
    {
        private QuestLogComponent _questLog;

        [TaskSelector]
        [SerializeField]
        private TaskSelector _singleAttack;

        [TaskSelector]
        [SerializeField]
        private TaskSelector _thirdComboAttack;

        [TaskSelector]
        [SerializeField]
        private TaskSelector _strongAttack;

        public override float Attacked(
            GameObject attacker, 
            int damage, 
            Vector3? hitPoint, 
            DamageType damageType, 
            AttackTypeDetail attackType = AttackTypeDetail.Generic, 
            int comboNumber = 1)
        {
            var result = base.Attacked(attacker, damage, hitPoint, damageType, attackType, comboNumber);

            if(attackType == AttackTypeDetail.GroundStrong)
            {
                CompleteTask(_strongAttack);
            }
            else if(attackType == AttackTypeDetail.GroundLight && comboNumber == 1)
            {
                CompleteTask(_singleAttack);
            }
            else if (attackType == AttackTypeDetail.GroundLight && comboNumber == 3)
            {
                CompleteTask(_thirdComboAttack);
            }

            return result;
        }

        protected override void Awake()
        {
            base.Awake();
            _questLog = FindObjectOfType<QuestLogComponent>();
        }

        private void CompleteTask(TaskSelector selector)
        {
            var quest = _questLog.GetQuest(selector.Quest.questId);
            if(quest.QuestStatus == QuestStatus.Active)
            {
                var task = quest.GetTask<SettableTask>(selector.TaskName);
                task.SetComplete();
            }
        }
    }
}
