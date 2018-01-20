using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonInterfaces.Controllers
{
    public enum DamageType
    {
        Sword,
        Fist,
        Shoot,
        ChainedProjectile,
        BodyAttack,
        Environment
    }

    public enum AttackTypeDetail
    {
        Generic,
        GroundLight,
        GroundStrong,
        AirStrong
    }

    public interface ICharacter
    {
        bool CanBeAttacked { get; }
        bool IsEnemy { get; }
        int MaxLifePoints { get; }
        int LifePoints { get; }
        GameObject CharacterGameObject { get; }

        int Attacked(
            GameObject attacker, 
            int damage, 
            Vector3? hitPoint, 
            DamageType damageType,
            AttackTypeDetail attackType = AttackTypeDetail.Generic,
            int comboNumber = 1,
            bool showDamaged = false);
        bool StartGrappled(GameObject grapple);
        void EndGrappled();
        void Kill();
    }
}
