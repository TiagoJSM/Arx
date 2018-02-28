using CommonInterfaces.Controllers;
using GenericComponents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class BaseCombatBehaviour : MonoBehaviour
{
    public AttackType AttackType { get; protected set; }
    public int ComboNumber { get; set; }
    public AttackStyle AttackStyle { get; protected set; }

    protected CommonInterfaces.Controllers.ICharacter[] GetCharactersInRange(Vector3 attackAreaP1, Vector3 attackAreaP2, LayerMask enemyLayer)
    {
        return 
            Physics2D
                .OverlapAreaAll(attackAreaP1, attackAreaP2, enemyLayer)
                .Select(c => c.GetComponent<CommonInterfaces.Controllers.ICharacter>())
                .Where(c => c != null)
                .Distinct()
                .ToArray();
    }
}

public abstract class BaseGenericCombatBehaviour<TWeapon> : BaseCombatBehaviour
{
    public virtual TWeapon Weapon { get; set; }
}
