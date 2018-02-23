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

public class ChainThrowCombatBehaviour : BaseGenericCombatBehaviour<ChainThrow>
{
    private RaycastHit2D[] _results = new RaycastHit2D[10];

    private ChainThrow _weapon;
    private CommonInterfaces.Controllers.ICharacter _grappledCharacter;
    private bool _attached;

    [SerializeField]
    private float _minGrappleDistance = 5;
    [SerializeField]
    private float _ropePartLength = 6;
    [SerializeField]
    private LayerMask _wallLayer;
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private GameObject _raycastStart;
    [SerializeField]
    private GameObject _raycastEnd;

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

    public bool Attached
    {
        get
        {
            return _attached;
        }
    }
    public CommonInterfaces.Controllers.ICharacter GrappledCharacter { get { return _grappledCharacter; } }

    public event Action OnAttackFinish;
    public event Action OnWallHit;

    public void ThrowChain(float degrees)
    {
        Weapon.Throw(degrees);
    }

    public void ChainPull()
    {
        Weapon.Return();
    }

    private void OnAttackFinishHandler()
    {
        if(OnAttackFinish != null)
        {
            OnAttackFinish();
        }

        if (_grappledCharacter == null)
        {
            return;
        }

        _grappledCharacter.EndGrappled();
        _attached = false;

        var hitCount = Physics2D
            .RaycastNonAlloc(
                _raycastStart.transform.position, 
                Vector2.down, 
                _results,
                _raycastStart.transform.position.y - _raycastEnd.transform.position.y);
        for(var idx = 0; idx < hitCount; idx++)
        {
            if (!_results[idx].collider.isTrigger)
            {
                _grappledCharacter.CharacterGameObject.transform.position = _results[idx].point;
                return;
            }
        }
        _grappledCharacter.CharacterGameObject.transform.position = 
            new Vector3(_raycastStart.transform.position.x, _raycastEnd.transform.position.y);

        _grappledCharacter = null;
    }

    private void OnTriggerEnterHandler(Collider2D other, ChainedProjectile projectile, Vector3 position)
    {
        if (_attached)
        {
            return;
        }
        if (_wallLayer.IsInAnyLayer(other.gameObject) && CanAttachGrapple(other, position))
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
        var character = other.gameObject.GetComponent<CommonInterfaces.Controllers.ICharacter>();
        if (character == null)
        {
            return;
        }

        GrappleCharacter(character, projectile);
    }

    private bool CanAttachGrapple(Collider2D other, Vector3 position)
    {
        var distance = Vector3.Distance(position, this.transform.position);
        if(_minGrappleDistance > distance)
        {
            return false;
        }

        var hit = Physics2D.Linecast(_weapon.RightHandSocket.transform.position, position, _wallLayer);

        return hit && hit.normal.y <= 0;
    }

    private void GrappleCharacter(CommonInterfaces.Controllers.ICharacter character, ChainedProjectile projectile)
    {
        _grappledCharacter = character;
        _grappledCharacter.StartGrappled(projectile.gameObject);
        Weapon.StopProjectile();
        //projectile.Return();
    }
}
