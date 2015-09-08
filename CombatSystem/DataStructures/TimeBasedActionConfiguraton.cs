using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CombatSystem.Interfaces;

namespace CombatSystem.DataStructures
{
    public class TimeBasedActionConfiguraton : IActionConfiguraton
    {
        public int PerformTime { get; private set; }

        public AttackAction Action { get; set; }

        public TimeBasedActionConfiguraton(int performTime)
        {
            PerformTime = performTime;
        }
    }
}
