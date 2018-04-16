using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Scripts
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
        GameObject CharacterGameObject { get; }
        bool InPain { get; }
        int LifePoints { get; }
        int MaxLifePoints { get; }

        int Attacked(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType = AttackTypeDetail.Generic, int comboNumber = 1, bool showDamaged = false);
        void EndGrappled();
        void Kill();
        bool StartGrappled(GameObject grapple);
    }
}
