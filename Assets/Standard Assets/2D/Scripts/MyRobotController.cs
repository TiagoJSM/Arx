using UnityEngine;
using System.Collections;
using System.Linq;

public class MyRobotController : MonoBehaviour, ILedgeGrabber {

	private bool _grounded = false;
    private bool _grabbingLedge = false;
	private float _groundRadius = 0.2f;
    private bool _facingRight;

	private Animator _animator;
	private Rigidbody2D _rigidBody;
    private float _gravityScale;
    private Collider2D _lastLedge;

    private Rigidbody2D activePlatformBody;
    private Collider2D activePlatformCollider; 
    private Transform activePlatform; 
    private Vector2 activePlatformPosition;
    //private Vector3 activeLocalPlatformPoint; 
    //private Vector3 activeGlobalPlatformPoint; 
    //private Vector3 lastPlatformVelocity;

	public float maxSpeed = 6.0f;
    public float airMaxSpeed = 2.0f;
	public Transform groundCheck;
    public GameObject ledgeWallCheckTrigger;
    public GameObject emptyGrabSpace;
	public LayerMask whatIsGround;
    public float jumpForce = 700.0f;
    public float ledgeTopThreshold = 0.5f;

	// Use this for initialization
	void Start ()
    {
		_animator = GetComponent<Animator>();
		_rigidBody = GetComponent<Rigidbody2D>();
        _gravityScale = _rigidBody.gravityScale;
	}

	void FixedUpdate ()
    {
		_grounded = Physics2D.OverlapCircle(groundCheck.position, _groundRadius, whatIsGround);
        _animator.SetBool("Grounded", _grounded);
        _animator.SetFloat("VerticalSpeed", _rigidBody.velocity.y);

        if (_grabbingLedge)
        {
            ProcessMovementWhenGrabbingLedge();
            return;
        }

		var move = Input.GetAxis("Horizontal");

        _facingRight = DirectionOfMovement(move) ?? _facingRight;

		_animator.SetFloat("Speed", Mathf.Abs(move));

        _rigidBody.velocity = new Vector2(move * maxSpeed, _rigidBody.velocity.y);

		if (move > 0) {
			Flip(true);
		}
		else if(move < 0){
			Flip(false);
		}

        PlatformMovement();
	}

    void Update()
    {
        if (_grounded && Input.GetButtonDown("Jump"))
        {
            _animator.SetBool("Grounded", false);
            _rigidBody.AddForce(new Vector2(0, jumpForce));
        }
        else if (_grabbingLedge && Input.GetButtonDown("Jump"))
        {
            var move = Input.GetAxis("Horizontal");
            _animator.SetBool("Grounded", false);
            _rigidBody.AddForce(new Vector2(0, jumpForce));
        }
    }

    public void CanGrabLedge(bool canGrab, Collider2D ledgeCollider)
    {
        if (!canGrab)
        {
            DropLedge();
            _lastLedge = null;
            return;
        }
        if (_lastLedge == ledgeCollider)
        {
            return ;
        }
        _grabbingLedge = true;
        _lastLedge = ledgeCollider;
        _rigidBody.gravityScale = 0;
        _rigidBody.velocity = Vector2.zero;
    }

    private void ProcessMovementWhenGrabbingLedge()
    {
        var drop = Input.GetAxis("Vertical") < 0;
        if (drop)
        {
            DropLedge();
            return;
        }
    }

    private void DropLedge()
    {
        _grabbingLedge = false;
        _rigidBody.gravityScale = _gravityScale;
    }

    private void WallJump()
    {
        var horizontalJumpForce = _facingRight ? -10 : 10;
        _rigidBody.AddForce(new Vector2(horizontalJumpForce, jumpForce/3));
    }

	private void Flip(bool right)
    {
		var scale = transform.localScale;
		if ((right && scale.x > 0) || (!right && scale.x < 0)) 
        {
			return;
		}
		scale.x *= -1;
		transform.localScale = scale;
	}
   
    private bool? DirectionOfMovement(float horizontal)
    {
        if (horizontal > 0)
        {
            return true;
        } 
        else if (horizontal < 0)
        {
            return false;
        }
        return null;
    }


    private void PlatformMovement()
    {
        // Moving platform support
        if (activePlatformBody != null) {
            //var newPlatformPosition = activePlatform.transform.position.ToVector2();
            var newPlatformPosition = activePlatformBody.position;
            var moveDistance = (newPlatformPosition - activePlatformPosition);
            if(moveDistance != Vector2.zero)
            {
                //ToDo: bug is here
                _rigidBody.position = moveDistance + _rigidBody.position;
                var position = moveDistance + transform.position.ToVector2();
                transform.position = new Vector3(position.x, position.y);
            }
            activePlatformPosition = newPlatformPosition;
        }
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (activePlatformBody != null)
        {
            return;
        }
        foreach (var contact in other.contacts)
        {
            if (/*contact.moveDirection.y < -0.9 &&*/ contact.normal.y > 0.5 || _grabbingLedge) 
            { 
                activePlatformBody = contact.collider.gameObject.GetComponent<Rigidbody2D>();
                activePlatformCollider = contact.collider;
                activePlatform = activePlatformCollider.transform; 
                activePlatformPosition = activePlatform.transform.position;
                break;
            } 
        }
    }
    
    void OnCollisionExit2D(Collision2D other) 
    {
        if (other.collider == activePlatformCollider /*&& !_grabbingLedge*/)
        {
            activePlatformBody = null;
            activePlatformCollider = null;
        }
    }

}
