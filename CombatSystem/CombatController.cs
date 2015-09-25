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
        private Dictionary<string, ComboConfiguration> _combatAttacksConfiguration;
        private string _performingAttack;
        private ComboConfiguration _currentComboConfiguration;
        private float _elapsedTimeInCombo;
        private bool _gotInputForNextCombo;

        public CombatController(ICombatSystem combatSystem)
        {
            _combatAttacksConfiguration = combatSystem.CombatAttacksConfiguration;
        }

        public void Elapse(float elapsedTimeInMillis)
        {
            //ToDo: play animation
            if (_currentComboConfiguration == null)
            {
                return;
            }
            var previousElapsedtime = _elapsedTimeInCombo;
            _elapsedTimeInCombo += elapsedTimeInMillis;
            TriggerActions(previousElapsedtime);
            if (_elapsedTimeInCombo < _currentComboConfiguration.DurationMilliseconds)
            {
                return;
            }
            ExecuteOnEnd(_currentComboConfiguration);
            _currentComboConfiguration = _currentComboConfiguration.NextCombo;
            _elapsedTimeInCombo = 0.0f;

            if (_currentComboConfiguration == null)
            {
                _gotInputForNextCombo = false;
                return;
            }

            if (!_gotInputForNextCombo || !CheckPreConditions(_currentComboConfiguration))
            {
                _gotInputForNextCombo = false;
                _currentComboConfiguration = null;
            }
            ExecuteOnStart(_currentComboConfiguration);
        }

        public void Perform(string attack)
        {
            if (attack == null)
            {
                return;
            }
            if (_performingAttack == attack)
            {
                CheckNextComboInputTime();
                _performingAttack = attack;
                return;
            }
            if (!_combatAttacksConfiguration.ContainsKey(attack))
            {
                return;
            }
            var currentAttackConfiguration = _combatAttacksConfiguration[attack];
            if (!CheckPreConditions(currentAttackConfiguration))
            {
                return;
            }
            _performingAttack = attack;
            _currentComboConfiguration = currentAttackConfiguration;
            _elapsedTimeInCombo = 0.0f;
            ExecuteOnStart(_currentComboConfiguration);
        }

        private void CheckNextComboInputTime()
        {
            if (_gotInputForNextCombo)
            {
                return;
            }
            
            _gotInputForNextCombo = _currentComboConfiguration.CanTriggerNextCombo(_elapsedTimeInCombo);
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
            return _currentComboConfiguration.Actions.Where(a => a.PerformTime > startTime && a.PerformTime <= endTime);
        }

        private void TriggerActions(float previousElapsedtime)
        {
            if (_elapsedTimeInCombo == 0)
            {
                var zeroTimeActions = _currentComboConfiguration.Actions.Where(a => a.PerformTime == 0);
                PerformActions(zeroTimeActions);
            }
            var actions = GetActionsInInterval(previousElapsedtime, _elapsedTimeInCombo);
            PerformActions(actions);
        }

        private bool CheckPreConditions(ComboConfiguration comboConfiguration)
        {
            if (comboConfiguration.PreCondition == null)
            {
                return true;
            }
            return comboConfiguration.PreCondition();
        }

        private void ExecuteOnStart(ComboConfiguration comboConfiguration)
        {
            if (comboConfiguration.OnStart == null)
            {
                return ;
            }
            comboConfiguration.OnStart();
        }

        private void ExecuteOnEnd(ComboConfiguration comboConfiguration)
        {
            if (comboConfiguration.OnEnd == null)
            {
                return;
            }
            comboConfiguration.OnEnd();
        }

        private void ExecuteOnCancelled(ComboConfiguration comboConfiguration)
        {
            if (comboConfiguration.OnCancelled == null)
            {
                return;
            }
            comboConfiguration.OnCancelled();
        }
    }
}
