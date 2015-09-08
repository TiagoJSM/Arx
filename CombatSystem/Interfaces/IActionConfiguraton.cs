using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatSystem.Interfaces
{
    public interface IActionConfiguraton
    {
        int PerformTime { get; }
        AttackAction Action { get; set; }
    }
}
