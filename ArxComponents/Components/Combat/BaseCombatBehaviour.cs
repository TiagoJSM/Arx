using CommonInterfaces.Controllers;
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
        public AttackStyle AttackStyle { get; protected set; }

        public virtual bool PrimaryGroundAttack() { return false; }
        public virtual bool SecundaryGroundAttack() { return false; }
        public virtual bool PrimaryAirAttack() { return false; }
        public virtual bool SecundaryAirAttack() { return false; }
        public virtual bool ChargeAttack() { return false; }
        public virtual bool ReleaseChargeAttack() { return false; }
        public virtual void StartDiveAttack() { }
        public virtual void EndDiveAttack() { }

        protected ICharacter[] GetCharactersInRange(Vector3 attackAreaP1, Vector3 attackAreaP2, LayerMask enemyLayer)
        {
            return 
                Physics2D
                    .OverlapAreaAll(attackAreaP1, attackAreaP2, enemyLayer)
                    .Select(c => c.GetComponent<ICharacter>())
                    .Where(c => c != null)
                    .Distinct()
                    .ToArray();
        }
    }

    public abstract class BaseGenericCombatBehaviour<TWeapon> : BaseCombatBehaviour
    {
        public virtual TWeapon Weapon { get; set; }
        public abstract event Action OnAttackFinish;
    }
}
