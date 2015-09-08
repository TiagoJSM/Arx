using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CombatSystem.Interfaces;

namespace CombatSystem.DataStructures
{
    public class PercentageBasedActionConfiguraton : IActionConfiguraton
    {
        private AttackConfiguration _attackConfig;
        private float _percentage;

        public int PerformTime
        {
            get { return (int)(_attackConfig.Duration * (_percentage / 100.0f)); }
        }

        public AttackAction Action { get; set; }

        public PercentageBasedActionConfiguraton(
            AttackConfiguration attackConfig,
            float percentage)
        {
            _attackConfig = attackConfig;
            _percentage = percentage;
        }
    }
}
