using Assets.Standard_Assets._2D.Cameras.Scripts;
using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using GenericComponents.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CloseCombatBehaviour : BaseGenericCombatBehaviour<ICloseCombatWeapon>
{
    private AttackType _executedAttackType;
    private const int COMBO_START = 1;

    private List<ICharacter> _charactersAttackedOnDive;
    private Coroutine _diveAttackDetector;

    [SerializeField]
    private int maxCombos = 3;
    [SerializeField]
    private Transform _attackAreaP1;
    [SerializeField]
    private Transform _attackAreaP2;
    [SerializeField]
    private Transform _diveAttackAreaP1;
    [SerializeField]
    private Transform _diveAttackAreaP2;
    [SerializeField]
    private LayerMask _enemyLayer;

    public event Action OnEnterCombatState;
    public event Action<AttackType, AttackStyle, int> OnAttackStart;
    public event Action OnCombatFinish;
    public event Action<AttackType> OnHit;

    public CloseCombatBehaviour()
    {
        _charactersAttackedOnDive = new List<ICharacter>();
    }

    void Awake()
    {
        this.enabled = false;
    }

    void OnEnable()
    {
        AttackType = AttackType.None;
        ComboNumber = 0;
    }

    public void DoDamage()
    {
        var enemiesInRange = GetCharactersInRange(_attackAreaP1.position, _attackAreaP2.position, _enemyLayer);

        if(enemiesInRange.Length == 0)
        {
            return;
        }

        if (_executedAttackType == AttackType.Primary)
        {
            Weapon.LightAttack(ComboNumber, enemiesInRange, this.gameObject);
            RaiseOnHit();
        }
        else if (_executedAttackType == AttackType.Secundary)
        {
            Weapon.StrongAttack(ComboNumber, enemiesInRange, this.gameObject);
            RaiseOnHit();
        }
    }

    public void StartSlashAttack()
    {
        _executedAttackType = AttackType.Secundary;
        AttackType = AttackType.None;
    }

    public void FinishSlashAttack()
    {
        //ToDo
    }

    public void StartDiveAttack()
    {
        _diveAttackDetector = StartCoroutine(DiveAttackDetector());
    }

    public void EndDiveAttack()
    {
        if(_diveAttackDetector != null)
        {
            StopCoroutine(_diveAttackDetector);
            _diveAttackDetector = null;
        }
    }

    public void NotifyEnterCombatState()
    {
        if (OnEnterCombatState != null)
        {
            OnEnterCombatState.Invoke();
        }
    }

    public void NotifyAttackStart(AttackType attackType, AttackStyle attackStyle, int combo)
    {
        _executedAttackType = attackType;
        AttackType = AttackType.None;
        ComboNumber = combo;
        AttackStyle = attackStyle;
        if (OnAttackStart != null)
        {
            OnAttackStart.Invoke(attackType, attackStyle, combo);
        }
    }

    public void NotifyCombatFinish()
    {
        if (OnCombatFinish != null)
        {
            OnCombatFinish.Invoke();
        }
    }

    public bool PrimaryAttack()
    {
        AttackType = AttackType.Primary;
        return true;
    }

    public bool SecundaryAttack()
    {
        AttackType = AttackType.Secundary;
        return true;
    }

    private IEnumerator DiveAttackDetector()
    {
        while (true)
        {
            var enemiesInRange = GetCharactersInRange(_diveAttackAreaP1.position, _diveAttackAreaP2.position, _enemyLayer);
            Weapon.DiveAttack(enemiesInRange, this.gameObject);
            yield return null;
        }
    }

    private void RaiseOnHit()
    {
        if(OnHit != null)
        {
            OnHit(_executedAttackType);
        }
    }
}
