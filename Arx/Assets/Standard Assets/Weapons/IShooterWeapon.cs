using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
{
    public interface IShooterWeapon : IWeapon
    {
        event Action OnCooldownFinish;
        bool InCooldown { get; }
        float RemainingCooldownTime { get; }

        bool Shoot(float angleInDegrees, LayerMask enemyLayer, GameObject attacker, Vector3 position);
    }
}
