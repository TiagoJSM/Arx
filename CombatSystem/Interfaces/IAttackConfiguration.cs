using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatSystem.Interfaces
{
    public delegate bool AttackPreCondition();
    public delegate void OnStart();
    public delegate void OnEnd();
    public delegate void OnCancelled();

    public interface IAttackConfiguration
    {
        IAttackConfiguration If(AttackPreCondition preCondition);
        IAttackConfiguration CancelIfAttacked(bool cancel);
        IAttackConfiguration WithDuration(int milliseconds);
        IAttackActionConfiguration At(int milliseconds);
        IAttackActionConfiguration At(float percentage);
        IAttackConfiguration Play(string animationName);
        IAttackConfiguration NextAttack();
        IAttackConfiguration OnStart(OnStart callback);
        IAttackConfiguration OnEnd(OnEnd callback);
        IAttackConfiguration OnCancelled(OnCancelled callback);
        ICombatSystem EndCombo();
    }
}
