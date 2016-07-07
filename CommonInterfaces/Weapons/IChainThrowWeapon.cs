using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Weapons
{
    public interface IChainThrowWeapon : IWeapon
    {
        event Action OnAttackFinish;
        bool ReadyToThrow { get; }
        void Spin();
        void FocusThrow();
        void Throw();
    }
}
