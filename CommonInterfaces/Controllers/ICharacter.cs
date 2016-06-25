using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonInterfaces.Controllers
{
    public interface ICharacter
    {
        bool CanBeAttacked { get; }
        bool IsEnemy { get; }
        int MaxLifePoints { get; }
        int LifePoints { get; }

        bool Attack(ICharacter target, Vector3? hitPoint);
        bool Attacked(ICharacter attacker, int damage, Vector3? hitPoint);
    }
}
