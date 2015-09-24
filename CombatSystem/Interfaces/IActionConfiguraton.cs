using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatSystem.Interfaces
{
    public interface IActionConfiguraton
    {
        float PerformTime { get; }
        AttackAction Action { get; set; }
    }
}
