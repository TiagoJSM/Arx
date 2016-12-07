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
        GameObject CharacterGameObject { get; }

        float Attacked(GameObject attacker, int damage, Vector3? hitPoint);
        bool StartGrappled(GameObject grapple);
        void EndGrappled();
        void Kill();
    }
}
