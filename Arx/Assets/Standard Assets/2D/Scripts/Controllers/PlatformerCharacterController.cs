using GenericComponents.Controllers.Characters;
using MathHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum MovementType
{
    Walk,
    Run
}

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(LedgeChecker))]
[RequireComponent(typeof(RoofChecker))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerCharacterController : BasePlatformerController
{
    private Collider2D _activePlatformCollider;
    private bool _grabbingLedge = false;
    private Vector3 _desiredMovementVelocity;
    private float _defaultGravity;
    private bool _detectPlatform = true;
    private bool _applyMovementAndGravity;

    private Vector2 _pushImpact;
    private Vector2 _impactMovement;
    private float _pushStartTime;

    private LedgeChecker _ledgeChecker;
    private RoofChecker _roofChecker;
    private Rigidbody2D _rigidBody;
    private CharacterController2D _characterController2D;
    private Collider2D _detectedLedge;
    private Collider2D _lastGrabbedLedge;
    private bool _ledgeDetected;
    private bool _ducking;

    [SerializeField]
    private bool _constantVelocity = false;
    [SerializeField]
    private GameObject _grabHand;
    [SerializeField]
    private AudioSource _jumpSound;
    [SerializeField]
    private AudioSource _grabLedgeSound;

    public BoxCollider2D standingCollider;
    public BoxCollider2D duckingCollider;
    public float maxRollSpeed = 12.0f;

    public float gravity = -25f;
    public float minYVelocity = -40f;
    public float runSpeed = 8f;
    public float walkSpeed = 5f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float minJumpHeight = 7f;
    public float maxJumpHeight = 13f;

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
            return Velocity.y;
        }
    }

    public float HorizontalSpeed
    {
        get
        {
            return Velocity.x;
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

    public bool ApplyMovementAndGravity
    {
        get
        {
            return _applyMovementAndGravity;
        }
        set
        {
            _applyMovementAndGravity = value;
            if (!_applyMovementAndGravity)
            {
                DesiredMovementVelocity = Vector2.zero;
            }
        }
    }
    public bool SteadyRotation { get; protected set; }
    public bool DetectPlatform
    {
        get
        {
            return _detectPlatform;
        }
        protected set
        {
            _detectPlatform = value;
            if (!_detectPlatform)
            {
                _activePlatformCollider = null;
            }
        }
    }

    public Vector2 Velocity
    {
        get
        {
            return _characterController2D.velocity;
        }
    }

    protected Vector2 DesiredMovementVelocity
    {
        get { return _desiredMovementVelocity; }
        set { _desiredMovementVelocity = value; }
    }

    public Vector2 VelocityMultiplier { get; protected set; }
    public IEnumerable<RaycastHit2D> FrameHits { get; private set; }

    public CharacterController2D CharacterController2D { get { return _characterController2D; } }
    public MovementType MovementType { get; set; }

    public PlatformerCharacterController()
    {
        ApplyMovementAndGravity = true;
        SteadyRotation = true;
        MovementType = MovementType.Run;
    }

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
        DoMove(move, true);
    }

    public void DoGrabLedge()
    {
        if (_detectedLedge == null)
        {
            return;
        }
        //_grabHand.SetActive(true);
        _lastGrabbedLedge = _detectedLedge;
        transform.parent = _lastGrabbedLedge.gameObject.transform;
        DesiredMovementVelocity = Vector2.zero;
        gravity = 0;
        _grabbingLedge = true;
        _grabLedgeSound.Play();
    }

    public void JumpUp(float jumpRatio)
    {
        jumpRatio = Mathf.Clamp01(jumpRatio);
        var jumpHeight = Mathf.Lerp(minJumpHeight, maxJumpHeight, jumpRatio);
        _desiredMovementVelocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        _jumpSound.Play();
    }

    public void DropLedge()
    {
        //_grabHand.SetActive(false);
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
        //_normalizedHorizontalSpeed = 0;
        _desiredMovementVelocity = new Vector2(0, _desiredMovementVelocity.y);
    }

    public void Roll(float move)
    {
        var direction = DirectionOfMovement(move, Direction);
        _desiredMovementVelocity.x = DirectionValue(direction) * runSpeed * VelocityMultiplier.x;
        Flip(direction);
    }

    public void Push(Vector2 force)
    {
        var dir = force.normalized;
        _pushImpact = dir.normalized * force.magnitude;
        _pushStartTime = Time.time;
    }

    protected void DoMove(float move, bool setDirectionToMovement)
    {
        var direction = DirectionOfMovement(move, Direction);
        var speed = MovementType == MovementType.Run ? runSpeed : walkSpeed;
        _desiredMovementVelocity.x = DirectionValue(direction) * speed * VelocityMultiplier.x;
        if (Math.Abs(move) < 0.2)
        {
            _desiredMovementVelocity.x = 0;
        }
        else
        {
            if (setDirectionToMovement)
            {
                Flip(direction);
            }
        }        
    }

    protected IEnumerator MoveInParabola(
        Vector2 start, 
        Vector2 end, 
        Vector2 other, 
        float timeInSeconds,
        Action onFinish)
    {
        var parabola = MathfUtils.GetParabola(start, end, other);
        var startOtherXDistance = Math.Abs(start.x - other.x);
        var startEndXDistance = Math.Abs(start.x - end.x);
        var otherEndXDistance = Math.Abs(other.x - end.x);

        if (parabola.HasInvalidValue || startOtherXDistance < 1 || startEndXDistance < 1 || otherEndXDistance < 1)
        {
            yield return MoveLinear(start, end, timeInSeconds, onFinish);
        }
        else
        {
            yield return MoveInParabolaAux(start, end, parabola, timeInSeconds, onFinish);
        }
    }

    protected IEnumerator MoveLinear(
        Vector2 start,
        Vector2 end,
        float timeInSeconds,
        Action onFinish)
    {
        var elapsed = 0f;
        while (elapsed < timeInSeconds)
        {
            var position = Vector2.Lerp(start, end, elapsed / timeInSeconds);
            this.transform.position = new Vector3(position.x, position.y, this.transform.position.z);
            yield return null;
            elapsed += Time.deltaTime;
        }
        onFinish();
    }

    protected override void Awake()
    {
        base.Awake();
        if(_grabHand != null)
        {
            _grabHand.SetActive(false);
        }
        _rigidBody = GetComponent<Rigidbody2D>();
        _ledgeChecker = GetComponent<LedgeChecker>();
        _roofChecker = GetComponent<RoofChecker>();
        _characterController2D = GetComponent<CharacterController2D>();
        _defaultGravity = gravity;
        _characterController2D.OnFrameAllControllerCollidedEvent += OnAllControllerCollidedEventHandler;
        VelocityMultiplier = Vector2.one;
    }

    protected virtual void Start()
    {
        _characterController2D.BoxCollider2D = standingCollider;
    }

    protected virtual void Update()
    {
        _impactMovement =
            new Vector2(
                Mathf.Lerp(_pushImpact.x, 0, Mathf.Clamp01((Time.time - _pushStartTime) / 1)),
                _pushImpact.y) *
            Time.deltaTime;
        _pushImpact = new Vector2(_pushImpact.x, 0);

        if (ApplyMovementAndGravity)
        {
            ApplyMovement();
        }
        Collider2D collider;
        var ledgeDetected = _ledgeChecker.IsLedgeDetected(out collider);
        LedgeDetected(ledgeDetected, collider);

        IsGrounded = _characterController2D.isGrounded && CheckGrounded();
        CanStand = !_roofChecker.IsTouchingRoof;
        if (SteadyRotation)
        {
            transform.rotation = Quaternion.identity;
        }
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
        //var smoothedMovementFactor = _characterController2D.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?

        _desiredMovementVelocity.y += gravity * Time.deltaTime * VelocityMultiplier.y;

        if(_desiredMovementVelocity.y < minYVelocity)
        {
            _desiredMovementVelocity.y = minYVelocity;
        }

        _characterController2D.move(
            _desiredMovementVelocity * Time.deltaTime + 
            new Vector3(_impactMovement.x, _impactMovement.y, 0));
        //_desiredMovementVelocity = _characterController2D.velocity;
    }

    private void OnAllControllerCollidedEventHandler(IEnumerable<RaycastHit2D> hits)
    {
        if (!DetectPlatform)
        {
            return;
        }
        FrameHits = hits;
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

    private IEnumerator MoveInParabolaAux(
        Vector2 start,
        Vector2 end,
        Parabola parabola,
        float timeInSeconds,
        Action onFinish)
    {
        var elapsed = 0f;
        while (elapsed < timeInSeconds)
        {
            var position = MathfUtils.QuadraticInterpolation(start.x, end.x, parabola, elapsed / timeInSeconds);
            this.transform.position = new Vector3(position.x, position.y, this.transform.position.z);
            yield return null;
            elapsed += Time.deltaTime;
        }
        onFinish();
    }
}
