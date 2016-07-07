using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Weapons
{
    public interface IShooterWeapon : IWeapon
    {
        event Action OnCooldownFinish;
        bool InCooldown { get; }
        float RemainingCooldownTime { get; }

        bool Shoot(float angleInDegrees);
    }
}
