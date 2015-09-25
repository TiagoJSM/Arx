using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CombatSystem.DataStructures;
using CombatSystem.Interfaces;

namespace CombatSystem
{
    public class CombatConfiguration : ICombatSystem, IComboConfiguration, IAttackActionConfiguration
    {
        private Dictionary<string, ComboConfiguration> _attackConfigurations;
        private ComboConfiguration _currentComboConfiguration;

        public Dictionary<string, ComboConfiguration> CombatAttacksConfiguration { get { return _attackConfigurations; } }

        public static ICombatSystem Configure()
        {
            return new CombatConfiguration();
        }

        private CombatConfiguration()
        {
            _attackConfigurations = new Dictionary<string, ComboConfiguration>();
        }

        public IComboConfiguration StartCombo(string comboName)
        {
            _currentComboConfiguration = new ComboConfiguration();
            _attackConfigurations.Add(comboName, _currentComboConfiguration);
            return this;
        }

        public IComboConfiguration CancelIfAttacked(bool cancel)
        {
            _currentComboConfiguration.CancelIfAttacked = cancel;
            return this;
        }

        public IComboConfiguration If(ComboPreCondition preCondition)
        {
            _currentComboConfiguration.PreCondition = preCondition;
            return this;
        }

        public IComboConfiguration WithDuration(int milliseconds)
        {
            _currentComboConfiguration.DurationMilliseconds = milliseconds;
            return this;
        }

        public IAttackActionConfiguration At(int milliseconds)
        {
            _currentComboConfiguration.Actions.Add(new TimeBasedActionConfiguraton(milliseconds));
            return this;
        }

        public IAttackActionConfiguration At(float percentage)
        {
            _currentComboConfiguration.Actions.Add(new PercentageBasedActionConfiguraton(_currentComboConfiguration, percentage));
            return this;
        }

        public IComboConfiguration PlayAnimation(string animationName)
        {
            _currentComboConfiguration.AnimationName = animationName;
            return this;
        }

        public IComboConfiguration PlaySound(string soundName)
        {
            _currentComboConfiguration.SoundName = soundName;
            return this;
        }

        public IComboConfiguration NextCombo(float triggeredBefore = 0.5f)
        {
            _currentComboConfiguration.NextComboTriggeredBefore = triggeredBefore;
            _currentComboConfiguration.NextCombo = new ComboConfiguration();
            _currentComboConfiguration = _currentComboConfiguration.NextCombo;
            return this;
        }

        public IComboConfiguration OnStart(OnStart callback)
        {
            _currentComboConfiguration.OnStart = callback;
            return this;
        }

        public IComboConfiguration OnEnd(OnEnd callback)
        {
            _currentComboConfiguration.OnEnd = callback;
            return this;
        }

        public IComboConfiguration OnCancelled(OnCancelled callback)
        {
            _currentComboConfiguration.OnCancelled = callback;
            return this;
        }

        public ICombatSystem EndCombo()
        {
            return this;
        }

        public IComboConfiguration Perform(AttackAction action)
        {
            _currentComboConfiguration.Actions.Last().Action = action;
            return this;
        }
    }
}
