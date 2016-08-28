using CommonInterfaces.Enums;
using CommonInterfaces.Weapons;
using Extensions;
using GenericComponents.Containers;
using GenericComponents.Enums;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using GenericComponents.StateMachine;
using GenericComponents.StateMachine.States.PlatformerCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.Director;

namespace GenericComponents.Controllers.Characters
{
    //[RequireComponent(typeof(LedgeChecker))]
    //[RequireComponent(typeof(RoofChecker))]
    //[RequireComponent(typeof(Rigidbody2D))]
    //public class PlatformerCharacterController : BasePlatformerController
    //{
    //    private bool _grabbingLedge = false;

    //    private LedgeChecker _ledgeChecker;
    //    private RoofChecker _roofChecker;
    //    private Rigidbody2D _rigidBody;
    //    private float _gravityScale;
    //    private Collider2D _detectedLedge;
    //    private Collider2D _lastGrabbedLedge;
    //    private bool _ledgeDetected;
    //    private bool _ducking;

    //    public float groundMovementForce = 2f;
    //    public float airMovementForce = 1f;
    //    public float maxRunSpeed = 6.0f;
    //    public float airMaxSpeed = 2.0f;
    //    public float jumpForce = 700.0f;
    //    public Collider2D[] standingColliders;
    //    public Collider2D[] duckingColliders;
    //    public float maxRollSpeed = 12.0f;

    //    private bool DetectingPreviousGrabbedLedge
    //    {
    //        get
    //        {
    //            if(_lastGrabbedLedge == null)
    //            {
    //                return false;
    //            }
    //            return _lastGrabbedLedge == _detectedLedge;
    //        }
    //    }

    //    public bool CanGrabLedge
    //    {
    //        get
    //        {
    //            return _ledgeDetected && !IsGrounded && !DetectingPreviousGrabbedLedge && !_grabbingLedge;
    //        }
    //    }

    //    public bool IsGrounded { get; private set; }

    //    public bool CanStand { get; private set; }

    //    public float VerticalSpeed
    //    {
    //        get
    //        {
    //            return _rigidBody.velocity.y;
    //        }
    //    }

    //    public float HorizontalSpeed
    //    {
    //        get
    //        {
    //            return _rigidBody.velocity.x;
    //        }
    //    }

    //    protected Rigidbody2D Body
    //    {
    //        get
    //        {
    //            return _rigidBody;
    //        }
    //    }

    //    public bool Ducking
    //    {
    //        get
    //        {
    //            return _ducking;
    //        }
    //    }

    //    public bool GrabbingLedge
    //    {
    //        get
    //        {
    //            return _grabbingLedge;
    //        }
    //    }

    //    public void LedgeDetected(bool detected, Collider2D ledgeCollider)
    //    {
    //        _ledgeDetected = detected;
    //        if (!detected)
    //        {
    //            if (_grabbingLedge)
    //            {
    //                DropLedge();
    //            }
    //            _lastGrabbedLedge = null;
    //            return;
    //        }
    //        _detectedLedge = ledgeCollider;
    //    }

    //    public void DoMove(float move)
    //    {
    //        var direction = DirectionOfMovement(move, Direction);
    //        var movementForce = IsGrounded ? groundMovementForce : airMovementForce;

    //        if (Math.Abs(move) < 0.5)
    //        {
    //            _rigidBody.velocity = new Vector2(0, _rigidBody.velocity.y);
    //        }
    //        else
    //        {
    //            var hit = Physics2D.Raycast(this.transform.position, new Vector2(0, -1), 1, whatIsGround);
    //            var y = 0.0f;

    //            if (hit.collider != null)
    //            {
    //                var normal = hit.normal;
    //                y = -(normal.y * Mathf.Sign(normal.x) * DirectionValue(this.Direction));
    //            }
                
    //            _rigidBody.AddForce(new Vector2(movementForce, y) * DirectionValue(direction), ForceMode2D.Impulse);
    //        }
            
    //        Flip(direction);
    //    }

    //    public void DoGrabLedge()
    //    {
    //        if (_detectedLedge == null)
    //        {
    //            return;
    //        }
    //        _lastGrabbedLedge = _detectedLedge;
    //        transform.parent = _lastGrabbedLedge.gameObject.transform;
    //        _rigidBody.gravityScale = 0;
    //        _rigidBody.velocity = Vector2.zero;
    //        _grabbingLedge = true;
    //    }

    //    public void JumpUp()
    //    {
    //        _rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    //    }

    //    public void DropLedge()
    //    {
    //        _grabbingLedge = false;
    //        transform.parent = null;
    //        _rigidBody.gravityScale = _gravityScale;
    //    }

    //    public virtual void Duck()
    //    {
    //        _ducking = true;
    //        foreach(var duckCollider in duckingColliders)
    //        {
    //            duckCollider.enabled = true;
    //        }
    //        foreach (var standingCollider in standingColliders)
    //        {
    //            standingCollider.enabled = false;
    //        }
    //    }

    //    public virtual void Stand()
    //    {
    //        _ducking = false;
    //        foreach (var duckCollider in duckingColliders)
    //        {
    //            duckCollider.enabled = false;
    //        }
    //        foreach (var standingCollider in standingColliders)
    //        {
    //            standingCollider.enabled = true;
    //        }
    //    }

    //    public void StayStill()
    //    {
    //        _rigidBody.velocity = Vector2.zero;
    //    }

    //    public void Roll(float move)
    //    {
    //        var direction = DirectionOfMovement(move, Direction);
    //        Flip(direction);
    //        var directionValue = DirectionValue(direction);
    //        _rigidBody.velocity = new Vector2(directionValue * maxRollSpeed, _rigidBody.velocity.y);
    //    }

    //    protected override void Awake()
    //    {
    //        base.Awake();
    //        _rigidBody = GetComponent<Rigidbody2D>();
    //        _gravityScale = _rigidBody.gravityScale;
    //        _ledgeChecker = GetComponent<LedgeChecker>();
    //        _roofChecker = GetComponent<RoofChecker>();
    //    }

    //    protected virtual void FixedUpdate()
    //    {
    //        Collider2D collider;
    //        var ledgeDetected = _ledgeChecker.IsLedgeDetected(out collider);
    //        LedgeDetected(ledgeDetected, collider);
    //        IsGrounded = CheckGrounded();
    //        CanStand = !_roofChecker.IsTouchingRoof;
    //        //ToDo, this seems wrong
    //        //check https://vonlehecreative.wordpress.com/2010/02/02/unity-resource-velocitylimiter/
    //        var currentMaxSpeed = IsGrounded ? maxRunSpeed : airMaxSpeed;
    //        _rigidBody.velocity = new Vector2(Mathf.Clamp(_rigidBody.velocity.x, -currentMaxSpeed, currentMaxSpeed), _rigidBody.velocity.y);
    //        _rigidBody.rotation = 0;
    //        transform.rotation = Quaternion.identity;
    //    }

    //    void OnDrawGizmosSelected()
    //    {
    //        base.DrawGizmos();
    //    }
    //}
}
