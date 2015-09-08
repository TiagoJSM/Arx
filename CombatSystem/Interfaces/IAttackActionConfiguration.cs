using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatSystem.Interfaces
{
    public delegate bool AttackAction();

    public interface IAttackActionConfiguration
    {
        IAttackConfiguration Perform(AttackAction action);
    }
}
