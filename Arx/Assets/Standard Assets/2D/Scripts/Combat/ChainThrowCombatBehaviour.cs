using CommonInterfaces.Enums;
using MathHelper;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GenericComponents.Enums;
using GenericComponents.Behaviours;
using Extensions;
using Assets.Standard_Assets.Weapons;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using System.Collections;

[RequireComponent(typeof(CharacterController2D))]
public class ChainThrowCombatBehaviour : BaseGenericCombatBehaviour<ChainThrow>
{
    private RaycastHit2D[] _results = new RaycastHit2D[10];
    private CharacterController2D _characterController;

    private ChainThrow _weapon;
    private GrappledCharacter _grappledCharacter;

    [SerializeField]
    private LayerMask _wallLayer;
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private float _chainThrustSpeed = 60f;
    [SerializeField]
    private float _thrustTreshold = 0.5f;

    public override ChainThrow Weapon
    {
        get
        {
            return _weapon;
        }
        set
        {
            if(_weapon == value)
            {
                return;
            }
            if(_weapon != null)
            {
                _weapon.OnAttackFinish -= OnAttackFinishHandler;
                _weapon.OnTriggerEnter -= OnTriggerEnterHandler;
            }
            _weapon = value;
            if (_weapon != null)
            {
                _weapon.OnAttackFinish += OnAttackFinishHandler;
                _weapon.OnTriggerEnter += OnTriggerEnterHandler;
            }
        }
    }

    public GrappledCharacter GrappledCharacter { get { return _grappledCharacter; } }
    public bool ChainPulling { get; private set; }
    public bool ChainThrusting { get; private set; }

    public event Action OnAttackFinish;
    public event Action<GrappledCharacter> ChainThrustComplete;

    public void ThrowChain(float degrees)
    {
        Weapon.Throw(degrees);
    }

    public void ChainPull()
    {
        ChainPulling = true;
        Weapon.Return();
        GrappledCharacter.Pull();
    }

    public void ChainThrust()
    {
        StartCoroutine(ChainThrustRoutine());
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController2D>();
    }

    private void OnAttackFinishHandler()
    {
        if(OnAttackFinish != null)
        {
            OnAttackFinish();
        }

        ChainPulling = false;
        Weapon.ShowProjectile(false);

        if (_grappledCharacter == null)
        {
            return;
        }

        _grappledCharacter.EndGrapple();
        _grappledCharacter = null;
    }

    private void OnTriggerEnterHandler(Collider2D other, ChainedProjectile projectile, Vector3 position)
    {
        if (_grappledCharacter != null)
        {
            return;
        }
        if (_wallLayer.IsInAnyLayer(other.gameObject))
        {
            projectile.Return();
            return;
        }
        if (!_enemyLayer.IsInAnyLayer(other.gameObject))
        {
            return;
        }
        if (_grappledCharacter != null)
        {
            return;
        }
        var character = other.gameObject.GetComponent<GrappledCharacter>();
        if (character == null)
        {
            return;
        }

        GrappleCharacter(character, projectile);
    }

    private void GrappleCharacter(GrappledCharacter character, ChainedProjectile projectile)
    {
        _grappledCharacter = character;
        _grappledCharacter.StartGrapple(projectile.gameObject, gameObject);
        Weapon.StopProjectile();
    }

    private IEnumerator ChainThrustRoutine()
    {
        ChainThrusting = true;
        var grappleOffset = _weapon.InstantiatedHeldProjectile.Origin.transform.position - transform.position;
        while (true)
        {
            var target = _weapon.InstantiatedHeldProjectile.transform.position - grappleOffset;
            var direction = (target - this.transform.localPosition).normalized;
            _characterController.move(direction * _chainThrustSpeed * Time.deltaTime);
            if (Vector3.Distance(this.transform.localPosition, target) < _thrustTreshold)
            {
                break;
            }
            yield return null;
        }

        ChainThrusting = false;
        Weapon.InstantiatedHeldProjectile.ResetProjectile();

        if (ChainThrustComplete != null)
        {
            ChainThrustComplete(_grappledCharacter);
        }
        OnAttackFinishHandler();
    }
}
