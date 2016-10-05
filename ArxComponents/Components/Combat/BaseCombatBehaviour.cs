using GenericComponents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Combat
{
    public abstract class BaseCombatBehaviour : MonoBehaviour
    {
        public AttackType ComboType { get; protected set; }
        public int ComboNumber { get; set; }

        public abstract bool PrimaryAttack();
        public abstract bool SecundaryAttack();
    }

    public abstract class BaseGenericCombatBehaviour<TWeapon> : BaseCombatBehaviour
    {
        public virtual TWeapon Weapon { get; set; }
        public abstract event Action OnAttackFinish;
    }
}
