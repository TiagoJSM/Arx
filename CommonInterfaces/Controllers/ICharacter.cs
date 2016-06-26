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

        bool Attacked(GameObject attacker, int damage, Vector3? hitPoint);
    }
}
