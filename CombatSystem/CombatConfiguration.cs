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

        public IAttackConfiguration If(AttackPreCondition preCondition)
        {
            _currentAttackConfiguration.PreCondition = preCondition;
            return this;
        }

        public IAttackConfiguration WithDuration(int milliseconds)
        {
            _currentAttackConfiguration.Duration = milliseconds;
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

        public IAttackConfiguration Play(string animationName)
        {
            _currentAttackConfiguration.AnimationName = animationName;
            return this;
        }

        public IAttackConfiguration NextAttack()
        {
            _currentAttackConfiguration.NextAttack = new AttackConfiguration();
            _currentAttackConfiguration = _currentAttackConfiguration.NextAttack;
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
