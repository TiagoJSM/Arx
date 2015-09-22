using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatSystem.Interfaces
{
    public delegate bool AttackPreCondition();

    public interface IAttackConfiguration
    {
        IAttackConfiguration If(AttackPreCondition preCondition);
        IAttackConfiguration CancelIfAttacked(bool cancel);
        IAttackConfiguration WithDuration(int milliseconds);
        IAttackActionConfiguration At(int milliseconds);
        IAttackActionConfiguration At(float percentage);
        IAttackConfiguration Play(string animationName);
        IAttackConfiguration NextAttack();
        ICombatSystem EndCombo();
    }
}
