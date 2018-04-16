using Assets.Standard_Assets._2D.Scripts.Characters.Enemies;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets.Extensions;
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
    Run,
    Sprint
}

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(RoofChecker))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterSpread))]
public class PlatformerCharacterController : BasePlatformerController
{
    private Vector3 _supportingPlatformPosition;
    private Transform _supportingPlatform;
    private int EnemyLayer;
    private int PlayerLayer;

    public Transform SupportingPlatform
    {
        get
        {
            return _supportingPlatform;
        }
        set
        {
            _supportingPlatform = value;
            if(_supportingPlatform != null)
            {
                _supportingPlatformPosition = _supportingPlatform.position;
            }
        }
    }

    private Collider2D _activePlatformCollider;
    private Vector3 _desiredMovementVelocity;
    private bool _detectPlatform = true;
    private bool _applyMovementAndGravity;
    private bool _isGrounded;

    private Vector2 _pushImpact;
    private Vector2 _impactMovement;
    private float _pushStartTime;

    private RoofChecker _roofChecker;
    private Rigidbody2D _rigidBody;
    private CharacterController2D _characterController2D;
    private bool _ducking;

    [SerializeField]
    private bool _constantVelocity = false;
    [SerializeField]
    private AudioSource _jumpSound;
    [SerializeField]
    private ParticleSystem _runningParticles;

    public BoxCollider2D standingCollider;
    public BoxCollider2D duckingCollider;
    public float maxRollSpeed = 12.0f;

    public float gravity = -25f;
    public float sprintSpeed = 10f;
    public float runSpeed = 8f;
    public float walkSpeed = 5f;
    public float airSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 5f;

    public event Action OnGrounded;
    public event Action OnJump;

    public bool IsGrounded
    {
        get
        {
            return _isGrounded && _supportingPlatform;
        }
        private set
        {
            if(_isGrounded != value)
            {
                _isGrounded = value;
                if (_isGrounded && OnGrounded != null)
                {
                    OnGrounded();
                }
            }
        }
    }

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

    public bool Ducking
    {
        get
        {
            return _ducking;
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

    public Vector2 DesiredMovementVelocity
    {
        get { return _desiredMovementVelocity; }
        set { _desiredMovementVelocity = value; }
    }

    public Vector2 VelocityMultiplier { get; protected set; }
    public IEnumerable<RaycastHit2D> FrameHits { get; private set; }

    public CharacterController2D CharacterController2D { get { return _characterController2D; } }
    public MovementType MovementType { get; set; }
    public CharacterSpread CharacterSpread { get; private set; }
    public float DefaultGravity { get; private set; }

    public PlatformerCharacterController()
    {
        ApplyMovementAndGravity = true;
        SteadyRotation = true;
        MovementType = MovementType.Run;
    }
    
    public void DoMove(float move)
    {
        DoMove(move, true);
    }

    public void JumpUp(float jumpRatio)
    {
        jumpRatio = Mathf.Clamp01(jumpRatio);
        _desiredMovementVelocity.y = jumpHeight * jumpRatio;

        if (!_jumpSound.isPlaying)
        {
            _jumpSound.Play();
        }

        if(OnJump != null)
        {
            OnJump();
        }
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

    public void DoMove(float move, float speed, bool setDirectionToMovement)
    {
        var direction = DirectionOfMovement(move, Direction);
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

    public void DoMove(float move, bool setDirectionToMovement)
    {
        var moveSpeed = walkSpeed;
        switch (MovementType)
        {
            case MovementType.Run: moveSpeed = runSpeed; break;
            case MovementType.Sprint: moveSpeed = sprintSpeed; break;
        }

        if (!IsGrounded)
        {
            moveSpeed = airSpeed;
        }
        DoMove(move, moveSpeed, setDirectionToMovement);       
    }

    public void EmitDustParticle()
    {
        _runningParticles.Emit(1);
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
        _rigidBody = GetComponent<Rigidbody2D>();
        _roofChecker = GetComponent<RoofChecker>();
        _characterController2D = GetComponent<CharacterController2D>();
        DefaultGravity = gravity;
        _characterController2D.OnFrameAllControllerCollidedEvent += OnAllControllerCollidedEventHandler;
        CharacterSpread = GetComponent<CharacterSpread>();
        VelocityMultiplier = Vector2.one;
        EnemyLayer = LayerMask.NameToLayer(GameObjectExtensions.EnemyTag);
        PlayerLayer = LayerMask.NameToLayer(GameObjectExtensions.PlayerTag);
}

    protected virtual void Start()
    {
        _characterController2D.BoxCollider2D = standingCollider;
    }

    protected override void Update()
    {
        base.Update();
        RepositionRelativeToParent();

        _impactMovement =
            new Vector2(
                Mathf.Lerp(_pushImpact.x, 0, Mathf.Clamp01(Time.time - _pushStartTime)),
                _pushImpact.y) *
            Time.deltaTime;
        _pushImpact = new Vector2(_pushImpact.x, 0);

        if (ApplyMovementAndGravity)
        {
            ApplyMovement();
        }

        IsGrounded = _characterController2D.isGrounded;
        CanStand = !_roofChecker.IsTouchingRoof;
        if (SteadyRotation)
        {
            transform.rotation = Quaternion.identity;
        }

        //HandleMovementParticles();
    }

    private void RepositionRelativeToParent()
    {
        if (SupportingPlatform)
        {
            var movement = SupportingPlatform.position - _supportingPlatformPosition;
            movement.z = 0;
            _supportingPlatformPosition = SupportingPlatform.position;
            transform.position = transform.position + movement;
        }
    }

    protected virtual void FixedUpdate()
    {

    }
    
    protected virtual void OnDestroy()
    {
        _characterController2D.OnFrameAllControllerCollidedEvent -= OnAllControllerCollidedEventHandler;
    }

    private void ApplyMovement()
    {
        var smoothedMovementFactor = _characterController2D.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        _desiredMovementVelocity.y += gravity * Time.deltaTime * VelocityMultiplier.y;
        var gravityForce = new Vector3(0, gravity * Time.deltaTime, 0);

        var targetMovement = _desiredMovementVelocity * Time.deltaTime;

        if (!_characterController2D.isGrounded)
        {
            targetMovement.x = Mathf.Lerp(Velocity.x * Time.deltaTime, targetMovement.x, Time.deltaTime * smoothedMovementFactor);
        }

        var movement = new Vector3(targetMovement.x * VelocityMultiplier.x, targetMovement.y * VelocityMultiplier.y, 0);

        _characterController2D.move(
            movement + new Vector3(_impactMovement.x, _impactMovement.y, 0));

        if(_characterController2D.velocity.y <= 0 && _desiredMovementVelocity.y > 0)
        {
            _desiredMovementVelocity.y = 0;
        }
        if (IsGrounded && !CharacterController2D.SlidingDown && _desiredMovementVelocity.y < 0)
        {
            _desiredMovementVelocity.y = 0;
        }
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
            SupportingPlatform = null;
            _activePlatformCollider = null;
        }

        foreach (var hit in hits)
        {
            var hitLayer = hit.collider.gameObject.layer;
            if(hitLayer == EnemyLayer || hitLayer == PlayerLayer)
            {
                continue;
            }
            if (hit.normal.y > GroundContactMaxNormal)
            {
                if(SupportingPlatform == hit.collider.transform)
                {
                    return;
                }
                SupportingPlatform = hit.collider.transform;
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

    private void HandleMovementParticles()
    {
        if(_runningParticles != null)
        {
            var isMoving = !Mathf.Approximately(Velocity.x, 0);
            if(isMoving && !_runningParticles.isPlaying && IsGrounded)
            {
                _runningParticles.Play();
            }
            else if (!isMoving && _runningParticles.isPlaying || !IsGrounded)
            {
                _runningParticles.Stop();
            }
        }
    }
}
