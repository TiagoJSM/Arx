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
        private ComboConfiguration _comboConfig;
        private float _percentage;

        public float PerformTime
        {
            get { return (_comboConfig.DurationMilliseconds * (_percentage / 100.0f)); }
        }

        public AttackAction Action { get; set; }

        public PercentageBasedActionConfiguraton(
            ComboConfiguration comboConfig,
            float percentage)
        {
            _comboConfig = comboConfig;
            _percentage = percentage;
        }
    }
}
