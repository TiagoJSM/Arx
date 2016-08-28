using UnityEngine;
using System.Collections;
using CommonInterfaces.Weapons;
using GenericComponents.Enums;
using ArxGame.Components;
using CommonInterfaces.Controllers;
using System;
using ArxGame.Components.Combat;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MainPlatformerController))]
[RequireComponent(typeof(CombatModule))]
public class ArxAnimationController : MonoBehaviour {
    private readonly int _HorizontalVelocity = Animator.StringToHash("Horizontal Velocity");
    private readonly int _Grounded = Animator.StringToHash("Grounded");
    private readonly int _Ducking = Animator.StringToHash("Ducking");
    private readonly int _LedgeGrabbing = Animator.StringToHash("Ledge Grabbing");
    private readonly int _VerticalVelocity = Animator.StringToHash("Vertical Velocity");
    private readonly int _Attacking = Animator.StringToHash("Attacking");
    private readonly int _ComboCount = Animator.StringToHash("Combo Count");
    private readonly int _WeaponType = Animator.StringToHash("Weapon Type");
    private readonly int _AttackType = Animator.StringToHash("Attack Type");
    private readonly int _RollingState = Animator.StringToHash("Base Layer.Ducking locomotion.Roll");

    private Animator _animator;
    private MainPlatformerController _platformerController;
    private CombatModule _combatModule;

    public float HorizontalVelocity
    {
        set
        {
            _animator.SetFloat(_HorizontalVelocity, Mathf.Abs(value));
        }
    }
    private bool Grounded
    {
        set
        {
            _animator.SetBool(_Grounded, value);
        }
    }
    private bool Ducking
    {
        set
        {
            _animator.SetBool(_Ducking, value);
        }
    }
    private bool LedgeGrabbing
    {
        set
        {
            _animator.SetBool(_LedgeGrabbing, value);
        }
    }
    private float VerticalVelocity
    {
        set
        {
            _animator.SetFloat(_VerticalVelocity, value);
        }
    }
    private bool Attacking
    {
        set
        {
            _animator.SetBool(_Attacking, value);
        }
    }
    private int ComboCount
    {
        set
        {
            _animator.SetInteger(_ComboCount, value);
        }
    }
    private WeaponType? WeaponType
    {
        set
        {
            var val = value != null ? (int)value : -1;
            _animator.SetInteger(_WeaponType, val);
        }
    }
    private AttackType AttackType
    {
        set
        {
            //Attacking = value != AttackType.None;
            _animator.SetInteger(_AttackType, (int)value);
        }
    }

    // Use this for initialization
    void Awake () {
        _animator = GetComponent<Animator>();
        _platformerController = GetComponent<MainPlatformerController>();
        _combatModule = GetComponent<CombatModule>();
    }

    void Start()
    {
        //_animator.GetCurrentAnimatorClipInfo(_RollingState)[0].clip. = _platformerController.RollingDuration;
    }
	
    void Update()
    {
        HorizontalVelocity = _platformerController.HorizontalSpeed;
        VerticalVelocity = _platformerController.VerticalSpeed;
        Grounded = _platformerController.IsGrounded;
        LedgeGrabbing = _platformerController.GrabbingLedge;
        ComboCount = _combatModule.ComboNumber;
        WeaponType = _combatModule.WeaponType;
        AttackType = _combatModule.ComboType;
        Attacking = _platformerController.Attacking;
        Ducking = _platformerController.Ducking;

        var currentState = _animator.GetCurrentAnimatorStateInfo(0);
        var c = _animator.GetCurrentAnimatorClipInfo(0)[0];
        if (currentState.fullPathHash == _RollingState)
        {
            _animator.speed = c.clip.length / _platformerController.RollingDuration;
        }
        else
        {
            _animator.speed = 1;
        }
    }
}
