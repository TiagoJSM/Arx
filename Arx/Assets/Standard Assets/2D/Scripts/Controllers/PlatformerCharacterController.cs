using GenericComponents.Controllers.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(LedgeChecker))]
[RequireComponent(typeof(RoofChecker))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerCharacterController : BasePlatformerController
{
    private Collider2D _activePlatformCollider;
    private bool _grabbingLedge = false;
    private Vector3 _velocity;
    private float _normalizedHorizontalSpeed = 0;
    private float _defaultGravity;

    private LedgeChecker _ledgeChecker;
    private RoofChecker _roofChecker;
    private Rigidbody2D _rigidBody;
    private CharacterController2D _characterController2D;
    private float _gravityScale;
    private Collider2D _detectedLedge;
    private Collider2D _lastGrabbedLedge;
    private bool _ledgeDetected;
    private bool _ducking;

    public BoxCollider2D standingCollider;
    public BoxCollider2D duckingCollider;
    public float maxRollSpeed = 12.0f;

    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    private bool DetectingPreviousGrabbedLedge
    {
        get
        {
            if (_lastGrabbedLedge == null)
            {
                return false;
            }
            return _lastGrabbedLedge == _detectedLedge;
        }
    }

    public bool CanGrabLedge
    {
        get
        {
            return _ledgeDetected && !IsGrounded && !DetectingPreviousGrabbedLedge && !_grabbingLedge;
        }
    }

    public bool IsGrounded { get; private set; }

    public bool CanStand { get; private set; }

    public float VerticalSpeed
    {
        get
        {
            return _velocity.y;
        }
    }

    public float HorizontalSpeed
    {
        get
        {
            return _velocity.x;
        }
    }

    public bool SlidingDown
    {
        get
        {
            return _characterController2D.SlidingDown;
        }
    }

    protected Rigidbody2D Body
    {
        get
        {
            return _rigidBody;
        }
    }

    public bool Ducking
    {
        get
        {
            return _ducking;
        }
    }

    public bool GrabbingLedge
    {
        get
        {
            return _grabbingLedge;
        }
    }

    public CharacterController2D CharacterController2D { get { return _characterController2D; } }

    public void LedgeDetected(bool detected, Collider2D ledgeCollider)
    {
        _ledgeDetected = detected;
        if (!detected)
        {
            if (_grabbingLedge)
            {
                DropLedge();
            }
            _lastGrabbedLedge = null;
            return;
        }
        _detectedLedge = ledgeCollider;
    }

    public void DoMove(float move)
    {
        var direction = DirectionOfMovement(move, Direction);
        _normalizedHorizontalSpeed = DirectionValue(direction);
        if (Math.Abs(move) < 0.5)
        {
            _normalizedHorizontalSpeed = 0;
        }
        Flip(direction);
    }

    public void DoGrabLedge()
    {
        if (_detectedLedge == null)
        {
            return;
        }
        _lastGrabbedLedge = _detectedLedge;
        transform.parent = _lastGrabbedLedge.gameObject.transform;
        _velocity = Vector2.zero;
        gravity = 0;
        _grabbingLedge = true;
    }

    public void JumpUp()
    {
        _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
    }

    public void DropLedge()
    {
        _grabbingLedge = false;
        transform.parent = null;
        gravity = _defaultGravity;
    }

    public virtual void Duck()
    {
        _ducking = true;
        duckingCollider.enabled = true;
        standingCollider.enabled = false;
        _characterController2D.BoxCollider2D = duckingCollider;
    }

    public virtual void Stand()
    {
        _ducking = false;
        duckingCollider.enabled = false;
        standingCollider.enabled = true;
        _characterController2D.BoxCollider2D = standingCollider;
    }

    public void StayStill()
    {
        _normalizedHorizontalSpeed = 0;
        _velocity = Vector2.zero;
    }

    public void Roll(float move)
    {
        var direction = DirectionOfMovement(move, Direction);
        Flip(direction);
        var directionValue = DirectionValue(direction);
        _velocity.x = directionValue * maxRollSpeed;
    }

    protected override void Awake()
    {
        base.Awake();
        _rigidBody = GetComponent<Rigidbody2D>();
        _gravityScale = _rigidBody.gravityScale;
        _ledgeChecker = GetComponent<LedgeChecker>();
        _roofChecker = GetComponent<RoofChecker>();
        _characterController2D = GetComponent<CharacterController2D>();
        _defaultGravity = gravity;
        _characterController2D.OnFrameAllControllerCollidedEvent += OnAllControllerCollidedEventHandler;
    }

    protected virtual void Start()
    {
        _characterController2D.BoxCollider2D = standingCollider;
    }

    protected virtual void Update()
    {
        ApplyMovement();
        Collider2D collider;
        var ledgeDetected = _ledgeChecker.IsLedgeDetected(out collider);
        LedgeDetected(ledgeDetected, collider);

        //IsGrounded = CheckGrounded();
        IsGrounded = _characterController2D.isGrounded && CheckGrounded();
        CanStand = !_roofChecker.IsTouchingRoof;
        transform.rotation = Quaternion.identity;
    }

    protected virtual void FixedUpdate()
    {   
    }

    protected virtual void OnDestroy()
    {
        _characterController2D.OnFrameAllControllerCollidedEvent -= OnAllControllerCollidedEventHandler;
    }

    void OnDrawGizmosSelected()
    {
        base.DrawGizmos();
    }

    private void ApplyMovement()
    {
        var smoothedMovementFactor = _characterController2D.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        _velocity.x = Mathf.Lerp(_velocity.x, _normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
        _velocity.y += gravity * Time.deltaTime;

        _characterController2D.move(_velocity * Time.deltaTime);
        // grab our current _velocity to use as a base for all calculations
        _velocity = _characterController2D.velocity;
    }

    private void OnAllControllerCollidedEventHandler(IEnumerable<RaycastHit2D> hits)
    {
        if (_activePlatformCollider != null)
        {
            var _stillCollidingWithActivePlatform = hits.Any(h => h.collider == _activePlatformCollider);
            if (_stillCollidingWithActivePlatform)
            {
                return;
            }
            transform.parent = null;
            _activePlatformCollider = null;
        }

        foreach (var hit in hits)
        {
            if (hit.normal.y > GroundContactMaxNormal)
            {
                if (transform.parent == hit.collider.transform)
                {
                    return;
                }
                transform.SetParent(hit.collider.transform);
                transform.rotation = Quaternion.identity;
                _activePlatformCollider = hit.collider;
                return;
            }
        }
    }
}
