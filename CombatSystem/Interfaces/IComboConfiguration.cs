using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatSystem.Interfaces
{
    public delegate bool ComboPreCondition();
    public delegate void OnStart();
    public delegate void OnEnd();
    public delegate void OnCancelled();

    public interface IComboConfiguration
    {
        IComboConfiguration If(ComboPreCondition preCondition);
        IComboConfiguration CancelIfAttacked(bool cancel);
        IComboConfiguration WithDuration(int milliseconds);
        IAttackActionConfiguration At(int milliseconds);
        IAttackActionConfiguration At(float percentage);
        IComboConfiguration PlayAnimation(string animationName);
        IComboConfiguration PlaySound(string soundName);
        IComboConfiguration NextCombo(float triggeredBefore = 0.5f);
        IComboConfiguration OnStart(OnStart callback);
        IComboConfiguration OnEnd(OnEnd callback);
        IComboConfiguration OnCancelled(OnCancelled callback);
        ICombatSystem EndCombo();
    }
}
