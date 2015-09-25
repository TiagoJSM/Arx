using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CombatSystem.DataStructures;
using CombatSystem.Interfaces;

namespace CombatSystem
{
    public class CombatConfiguration : ICombatSystem, IAttackConfiguration, IAttackActionConfiguration
    {
        private Dictionary<string, AttackConfiguration> _attackConfigurations;
        private AttackConfiguration _currentAttackConfiguration;

        public Dictionary<string, AttackConfiguration> CombatAttacksConfiguration { get { return _attackConfigurations; } }

        public static ICombatSystem Configure()
        {
            return new CombatConfiguration();
        }

        private CombatConfiguration()
        {
            _attackConfigurations = new Dictionary<string, AttackConfiguration>();
        }

        public IAttackConfiguration StartCombo(string comboName)
        {
            _currentAttackConfiguration = new AttackConfiguration();
            _attackConfigurations.Add(comboName, _currentAttackConfiguration);
            return this;
        }

        public IAttackConfiguration CancelIfAttacked(bool cancel)
        {
            _currentAttackConfiguration.CancelIfAttacked = cancel;
            return this;
        }

        public IAttackConfiguration If(AttackPreCondition preCondition)
        {
            _currentAttackConfiguration.PreCondition = preCondition;
            return this;
        }

        public IAttackConfiguration WithDuration(int milliseconds)
        {
            _currentAttackConfiguration.DurationMilliseconds = milliseconds;
            return this;
        }

        public IAttackActionConfiguration At(int milliseconds)
        {
            _currentAttackConfiguration.Actions.Add(new TimeBasedActionConfiguraton(milliseconds));
            return this;
        }

        public IAttackActionConfiguration At(float percentage)
        {
            _currentAttackConfiguration.Actions.Add(new PercentageBasedActionConfiguraton(_currentAttackConfiguration, percentage));
            return this;
        }

        public IAttackConfiguration PlayAnimation(string animationName)
        {
            _currentAttackConfiguration.AnimationName = animationName;
            return this;
        }

        public IAttackConfiguration PlaySound(string soundName)
        {
            _currentAttackConfiguration.SoundName = soundName;
            return this;
        }

        public IAttackConfiguration NextAttack(float triggeredBefore = 0.5f)
        {
            _currentAttackConfiguration.NextComboTriggeredBefore = triggeredBefore;
            _currentAttackConfiguration.NextAttack = new AttackConfiguration();
            _currentAttackConfiguration = _currentAttackConfiguration.NextAttack;
            return this;
        }

        public IAttackConfiguration OnStart(OnStart callback)
        {
            _currentAttackConfiguration.OnStart = callback;
            return this;
        }

        public IAttackConfiguration OnEnd(OnEnd callback)
        {
            _currentAttackConfiguration.OnEnd = callback;
            return this;
        }

        public IAttackConfiguration OnCancelled(OnCancelled callback)
        {
            _currentAttackConfiguration.OnCancelled = callback;
            return this;
        }

        public ICombatSystem EndCombo()
        {
            return this;
        }

        public IAttackConfiguration Perform(AttackAction action)
        {
            _currentAttackConfiguration.Actions.Last().Action = action;
            return this;
        }
    }
}
