using Assets.Standard_Assets.Weapons;
using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using GenericComponents.Enums;
using GenericComponents.Helpers;
using MathHelper;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[RequireComponent(typeof(CloseCombatBehaviour))]
[RequireComponent(typeof(ChainThrowCombatBehaviour))]
[RequireComponent(typeof(ShooterCombatBehaviour))]
public class CombatModule : MonoBehaviour//, ICombatComponent
{
    private CloseCombatBehaviour _closeCombat;
    private ChainThrowCombatBehaviour _chainThrowCombat;
    private ShooterCombatBehaviour _shooterCombat;

    private bool _over = false;

    [SerializeField]
    private GameObject _aimingArm;
    [SerializeField]
    private GameObject _head;
    [SerializeField]
    [Range(0, 90)]
    private float _aimArmLimit = 90;
    [SerializeField]
    [Range(0, 90)]
    private float _headLookLimit = 90;

    public event Action OnEnterCombatState;
    public event Action<AttackType, AttackStyle, int> OnAttackStart;
    public event Action OnCombatFinish;

    public ICloseCombatWeapon CloseCombatWeapon
    {
        get
        {
            return _closeCombat.Weapon;
        }
        set
        {
            _closeCombat.Weapon = value;
        }
    }

    public IShooterWeapon ShooterWeapon
    {
        get
        {
            return _shooterCombat.Weapon;
        }
        set
        {
            _shooterCombat.Weapon = value;
        }
    }

    public ChainThrow ChainThrowWeapon
    {
        get
        {
            return _chainThrowCombat.Weapon;
        }
        set
        {
            _chainThrowCombat.Weapon = value;
        }
    }

    public int ComboNumber
    {
        get
        {
            return _closeCombat.ComboNumber;
        }
        set
        {
            _closeCombat.ComboNumber = value;
        }
    }

    public WeaponType? WeaponType
    {
        get
        {
            if (_closeCombat.Weapon == null)
            {
                return null;
            }
            return _closeCombat.Weapon.WeaponType;
        }
    }

    public AttackType AttackType
    {
        get
        {
            return _closeCombat.AttackType;
        }
    }

    public AttackStyle AttackStyle
    {
        get
        {
            return _closeCombat.AttackStyle;
        }
    }

    public bool IsCurrentAnimationOver
    {
        get
        {
            return _over;
        }
    }

    public float AimAngle { get; set; }
    public bool Aiming { get; set; }
    public GrappleRope GrappleRope { get { return _chainThrowCombat.GrappleRope; } }

    public bool PrimaryAttack()
    {
        return _closeCombat.PrimaryAttack();
    }

    public bool SecundaryAttack()
    {
        return _closeCombat.SecundaryAttack();
    }

    /*public bool PrimaryAirAttack()
    {
        return _closeCombat.PrimaryAirAttack();
    }

    public bool SecundaryAirAttack()
    {
        return _closeCombat.SecundaryAirAttack();
    }*/

    public bool ChargeAttack()
    {
        //return _closeCombat.ChargeAttack();
        return false;
    }

    public bool ReleaseChargeAttack()
    {
        //if (_activeCombatBehaviour != null)
        //{
        //    return _activeCombatBehaviour.ReleaseChargeAttack();
        //}
        return false;
    }

    public void StartDiveAttack()
    {
        _closeCombat.StartDiveAttack();
    }

    public void EndDiveAttack()
    {
        _closeCombat.EndDiveAttack();
    }

    public bool Shoot()
    {
        AimAtTarget(_aimingArm, _aimArmLimit);
        return _shooterCombat.Shoot(GetWeaponAimAngle());
    }

    public void Throw()
    {
        AimAtTarget(_aimingArm, _aimArmLimit);
        _chainThrowCombat.ThrowChain(GetWeaponAimAngle());
    }

    public void ClimbGrapple(float vertical, float speed)
    {
        _chainThrowCombat.ClimbGrapple(vertical, speed);
    }

    public void ReleaseGrapple()
    {
        _chainThrowCombat.ReleaseGrapple();
    }

    public void NotifyOnEnterCombatState()
    {
        if (OnEnterCombatState != null)
        {
            OnEnterCombatState.Invoke();
        }
    }

    public void NotifyOnAttackStart(AttackType attackType, AttackStyle attackStyle, int combo)
    {
        if (OnAttackStart != null)
        {
            OnAttackStart.Invoke(attackType, attackStyle, combo);
        }
        _over = false;
    }

    public void NotifyOnCombatFinish()
    {
        if(OnCombatFinish != null)
        {
            OnCombatFinish.Invoke();
        }
        _over = true;
    }

    private void Awake()
    {
        _closeCombat = GetComponent<CloseCombatBehaviour>();
        _shooterCombat = GetComponent<ShooterCombatBehaviour>();
        _chainThrowCombat = GetComponent<ChainThrowCombatBehaviour>();

        _closeCombat.OnEnterCombatState += NotifyOnEnterCombatState;
        _closeCombat.OnAttackStart += NotifyOnAttackStart;
        _closeCombat.OnCombatFinish += NotifyOnCombatFinish;
        _chainThrowCombat.OnAttackFinish += NotifyOnCombatFinish;
    }

    private void Update()
    {
        _over = false;
        _shooterCombat.AimAngle = AimAngle;
        _chainThrowCombat.AimAngle = AimAngle;
    }

    private void LateUpdate()
    {
        if (Aiming)
        {
            AimAtTarget(_head, _headLookLimit);
            AimAtTarget(_aimingArm, _aimArmLimit);
        }
    }

    private void AimAtTarget(GameObject obj, float limit)
    {
        var rotation = AimAtTargetRotation(limit);
        obj.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private float AimAtTargetRotation(float limit)
    {
        var inverted = (_aimingArm.transform.lossyScale.x < 0) || (_aimingArm.transform.lossyScale.y < 0);

        var rotation = default(float);
        if (!inverted)
        {
            rotation = AimAngle;
        }
        else
        {
            rotation = (-AimAngle) + 180;
        }
        if (rotation > 180)
        {
            rotation -= 360;
        }
        rotation = Mathf.Clamp(rotation, -limit, limit);
        return rotation;
    }

    private float GetWeaponAimAngle()
    {
        var degrees = AimAngle;
        var radians = degrees * Mathf.Deg2Rad;
        var quadrant = radians.GetQuadrant();

        var inverted = (_aimingArm.transform.lossyScale.x < 0) || (_aimingArm.transform.lossyScale.y < 0);

        if (!inverted)
        {
            degrees = quadrant == 3 ? degrees - FloatUtils.FullDegreeTurn : degrees;
            degrees = Mathf.Clamp(degrees, -_aimArmLimit, _aimArmLimit);
        }
        else
        {
            degrees = Mathf.Clamp(degrees, FloatUtils.HalfDegreeTurn - _aimArmLimit, FloatUtils.HalfDegreeTurn + _aimArmLimit);
        }
        return degrees;
    }
}
