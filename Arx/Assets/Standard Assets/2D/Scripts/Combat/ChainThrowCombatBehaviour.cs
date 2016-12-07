using CommonInterfaces.Enums;
using CommonInterfaces.Weapons;
using MathHelper;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GenericComponents.Enums;
using ArxGame.Components.Weapons;
using ArxGame.Components.Environment;
using GenericComponents.Behaviours;
using Extensions;

public class ChainThrowCombatBehaviour : BaseGenericCombatBehaviour<ChainThrow>
{
    private RaycastHit2D[] _results = new RaycastHit2D[10];
    private GameObject _fixedJointGO;

    private ChainThrow _weapon;
    private CommonInterfaces.Controllers.ICharacter _grappledCharacter;
    private GrappleRope _rope;
    private bool _attached;

    [SerializeField]
    private float _maxGrappleRopeLength = 20;
    [SerializeField]
    private float _minGrappleDistance = 5;
    [SerializeField]
    private HingeJoint2D _hingePrefab;
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
    [Range(0, 90)]
    public float aimLimit = 90;
    public GameObject aimingArm;

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
    public GrappleRope GrappleRope { get { return _rope; } }

    public event Action OnAttackFinish;
    public event Action OnWallHit;

    public void ThrowChain(float degrees)
    {
        Weapon.Throw(degrees);
    }

    public void ClimbGrapple(float vertical, float speed)
    {
        if (Math.Abs(vertical) > 0.01f)
        {
            if(Math.Sign(vertical) > 0)
            {
                ClimUp(speed);
            }
            else
            {
                ClimbDown(speed);
            }
        }
    }

    public void ReleaseGrapple()
    {
        this.transform.parent = null;
        _weapon.Reset();
        Destroy(_rope.gameObject);
        _rope = null;
        _attached = false;
    }

    private void ClimUp(float speed)
    {
        var yMove = speed * Time.deltaTime;
        var joints = _rope.Joints;
        var tail = joints[0];
        var connected = joints[1];
        var ropeSize = _rope.RopeSize;
        var ropeSizeAfterClimb = _rope.RopeSize - yMove;

        if(ropeSize <= _ropePartLength)
        {
            return;
        }
        
        if(ropeSizeAfterClimb <= _ropePartLength)
        {
            yMove = _ropePartLength - ropeSizeAfterClimb;
        }

        if ((-tail.connectedAnchor.y) > yMove)
        {
            tail.connectedAnchor = new Vector2(0, tail.connectedAnchor.y + yMove);
            return;
        }

        if (joints.Length == 2)
        {
            tail.connectedAnchor = Vector2.zero;
        }
        else
        {
            var newConnection = joints[2];
            var excess = yMove + tail.connectedAnchor.y;
            var excessVerticalMove = new Vector2(0, excess);
            tail.connectedAnchor = new Vector2(0, -_ropePartLength);
            tail.connectedAnchor += excessVerticalMove;
            tail.connectedBody = newConnection.GetComponent<Rigidbody2D>();
            Destroy(connected.gameObject);
        }
    }

    private void ClimbDown(float speed)
    {
        var yMove = speed * Time.deltaTime;
        var joints = _rope.Joints;
        var tail = joints[0];
        var connected = joints[1];

        var totalRopeSize = _rope.RopeSize;
        if (totalRopeSize >= _maxGrappleRopeLength)
        {
            return;
        }

        totalRopeSize += yMove;
        if (totalRopeSize >= _maxGrappleRopeLength)
        {
            var excess = totalRopeSize - _maxGrappleRopeLength;
            yMove -= excess;
        }

        var anchor = tail.connectedAnchor.y - yMove;
        var distance = Vector2.Distance(tail.transform.position, connected.transform.position);
        var heading = connected.transform.position - tail.transform.position;
        var direction = heading / distance;

        if (Math.Abs(anchor) > _ropePartLength) // requires split
        {
            var hinge = Instantiate(_hingePrefab);
            hinge.transform.SetParent(_rope.transform, false);
            hinge.connectedBody = connected.GetComponent<Rigidbody2D>();
            hinge.transform.localPosition = connected.transform.localPosition - (direction * _ropePartLength);

            tail.transform.localPosition = hinge.transform.localPosition;
            tail.connectedBody = hinge.GetComponent<Rigidbody2D>();
            tail.connectedAnchor = new Vector2(0, -(Math.Abs(anchor) - _ropePartLength));
        }
        else
        {
            tail.connectedAnchor = new Vector2(0, anchor);
        }
    }

    private void CreateRope(Vector3 position, ChainedProjectile projectile)
    {
        var point = new GameObject("grapple");
        point.transform.position = position;
        _attached = true;
        projectile.Stop();
        CreateRopeAt(point, projectile);
        if (OnAttackFinish != null)
        {
            OnAttackFinish.Invoke();
        }
    }

    private void CreateRopeAt(GameObject point, ChainedProjectile projectile)
    {
        _fixedJointGO = new GameObject("fixed");
        var ropeRenderer = point.AddComponent<RopeRenderer>();
        _rope = point.AddComponent<GrappleRope>();

        _fixedJointGO.transform.SetParent(point.transform, false);
        var fixedJoint = _fixedJointGO.AddComponent<FixedJoint2D>();
        fixedJoint.autoConfigureConnectedAnchor = false;

        var distance = Vector2.Distance(point.transform.position, projectile.Origin.transform.position);
        var heading = projectile.Origin.transform.position - point.transform.position;
        var direction = heading / distance;

        var partsCount = (int)(distance / _ropePartLength);
        var connectedBody = _fixedJointGO.GetComponent<Rigidbody2D>();
        var firstHingeGo = new GameObject();
        var firstHinge = firstHingeGo.AddComponent<HingeJoint2D>();
        firstHinge.connectedBody = connectedBody;
        firstHinge.transform.SetParent(point.transform, false);
        firstHinge.autoConfigureConnectedAnchor = false;
        connectedBody = firstHinge.GetComponent<Rigidbody2D>();
        connectedBody.angularDrag = 10;

        if (partsCount < 2)
        {
            partsCount = 2;
        }

        var hinge = default(HingeJoint2D);
        for (var idx = 0; idx < partsCount; idx++)
        {
            hinge = Instantiate(_hingePrefab);
            if (idx == 0)
            {
                hinge.connectedAnchor = Vector2.zero;
            }
            hinge.transform.SetParent(point.transform, false);
            hinge.connectedBody = connectedBody;
            hinge.transform.localPosition = direction * idx * _ropePartLength;
            connectedBody = hinge.GetComponent<Rigidbody2D>();
        }

        _rope.RopeEnd = hinge;
        ropeRenderer.Rope = _rope;
        this.transform.localPosition = Vector3.zero;
        this.gameObject.transform.parent = hinge.gameObject.transform;
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
            CreateRope(position, projectile);
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
        projectile.Return();
    }
}
