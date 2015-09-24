using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CombatSystem.DataStructures;
using CombatSystem.Interfaces;

namespace CombatSystem
{
    public class CombatController
    {
        private Dictionary<string, AttackConfiguration> _combatAttacksConfiguration;
        private string _performingAttack;
        private AttackConfiguration _currentAttackConfiguration;
        private float _elapsedTimeInCombo;

        public CombatController(ICombatSystem combatSystem)
        {
            _combatAttacksConfiguration = combatSystem.CombatAttacksConfiguration;
        }

        public void Elapse(float elapsedTimeInMillis)
        {
            //ToDo: play animation, check if can perform next attack
            if (_currentAttackConfiguration == null)
            {
                return;
            }
            var previousElapsedtime = _elapsedTimeInCombo;
            _elapsedTimeInCombo += elapsedTimeInMillis;
            TriggerActions(previousElapsedtime);
            if (_elapsedTimeInCombo < _currentAttackConfiguration.DurationMilliseconds)
            {
                return;
            }
            _currentAttackConfiguration.OnEnd();
            _currentAttackConfiguration = _currentAttackConfiguration.NextAttack;
            _elapsedTimeInCombo = 0.0f;

            if (_currentAttackConfiguration != null && !_currentAttackConfiguration.PreCondition())
            {
                _currentAttackConfiguration = null;
            }
        }

        public void Perform(string attack)
        {
            if (_performingAttack == attack)
            {
                _performingAttack = attack;
                return;
            }
            if (!_combatAttacksConfiguration.ContainsKey(attack))
            {
                return;
            }
            var currentAttackConfiguration = _combatAttacksConfiguration[_performingAttack];
            if (!currentAttackConfiguration.PreCondition())
            {
                return;
            }
            _performingAttack = attack;
            _currentAttackConfiguration = currentAttackConfiguration;
            _elapsedTimeInCombo = 0.0f;
            _currentAttackConfiguration.OnStart();
        }

        private void PerformActions(IEnumerable<IActionConfiguraton> actionConfigs)
        {
            actionConfigs
                .ToList()
                .ForEach(a => 
                    a.Action());
        }

        private IEnumerable<IActionConfiguraton> GetActionsInInterval(float startTime, float endTime)
        {
            return _currentAttackConfiguration.Actions.Where(a => a.PerformTime > startTime && a.PerformTime <= endTime);
        }

        private void TriggerActions(float previousElapsedtime)
        {
            if (_elapsedTimeInCombo == 0)
            {
                var zeroTimeActions = _currentAttackConfiguration.Actions.Where(a => a.PerformTime == 0);
                PerformActions(zeroTimeActions);
            }
            var actions = GetActionsInInterval(previousElapsedtime, _elapsedTimeInCombo);
            PerformActions(actions);
        }
    }
}
