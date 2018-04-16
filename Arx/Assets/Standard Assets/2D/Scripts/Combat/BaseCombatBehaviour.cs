using Assets.Standard_Assets.Scripts;
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

    protected List<ICharacter> DetectNewEnemies(List<ICharacter> attackedCharacters, Vector3 attackAreaP1, Vector3 attackAreaP2, LayerMask enemyLayer)
    {
        var enemiesInRange = GetCharactersInRange(attackAreaP1, attackAreaP2, enemyLayer);
        var newCharacters = new List<ICharacter>();

        for (var idx = 0; idx < enemiesInRange.Length; idx++)
        {
            var enemy = enemiesInRange[idx];
            if (!attackedCharacters.Contains(enemy))
            {
                newCharacters.Add(enemy);
            }
        }
        attackedCharacters.AddRange(newCharacters);
        return newCharacters;
    }
}

public abstract class BaseGenericCombatBehaviour<TWeapon> : BaseCombatBehaviour
{
    public virtual TWeapon Weapon { get; set; }
}
