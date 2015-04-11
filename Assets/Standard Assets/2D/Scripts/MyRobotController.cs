using UnityEngine;
using System.Collections;
using System.Linq;

public class MyRobotController : MonoBehaviour, ILedgeGrabber {

	private bool _grounded = false;
    private bool _grabbingLedge = false;
	private float _groundRadius = 0.2f;
    private bool _facingRight;

	private Animator _animator;
	private Rigidbody2D _ridgidBody;
    private float _gravityScale;
    private Collider2D _lastLedge;

    private Collider2D activePlatformCollider; 
    private Transform activePlatform; 
    private Vector3 activePlatformPosition;
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
		_ridgidBody = GetComponent<Rigidbody2D>();
        _gravityScale = _ridgidBody.gravityScale;
	}

	void FixedUpdate ()
    {
		_grounded = Physics2D.OverlapCircle(groundCheck.position, _groundRadius, whatIsGround);
        _animator.SetBool("Grounded", _grounded);
        _animator.SetFloat("VerticalSpeed", _ridgidBody.velocity.y);

        if (_grabbingLedge)
        {
            ProcessMovementWhenGrabbingLedge();
            return;
        }

		var move = Input.GetAxis("Horizontal");

        _facingRight = DirectionOfMovement(move) ?? _facingRight;

		_animator.SetFloat("Speed", Mathf.Abs(move));

        _ridgidBody.velocity = new Vector2(move * maxSpeed, _ridgidBody.velocity.y);

		if (move > 0) {
			Flip(true);
		}
		else if(move < 0){
			Flip(false);
		}
	}

    void Update()
    {
        if (_grounded && Input.GetButtonDown("Jump"))
        {
            _animator.SetBool("Grounded", false);
            _ridgidBody.AddForce(new Vector2(0, jumpForce));
        }
        else if (_grabbingLedge && Input.GetButtonDown("Jump"))
        {
            var move = Input.GetAxis("Horizontal");
            _animator.SetBool("Grounded", false);
            _ridgidBody.AddForce(new Vector2(0, jumpForce));
        }

        // Moving platform support
        if (activePlatform != null) {
            var newPlatformPosition = activePlatform.transform.position;
            var moveDistance = (newPlatformPosition - activePlatformPosition);

            /*Debug.Log(newPlatformPosition.ToString("F4"));
            Debug.Log(moveDistance.ToString("F4"));
            Debug.Log(_ridgidBody.position.ToString("F4"));
            Debug.Log((moveDistance.ToVector2() + transform.position.ToVector2()).ToString("F4"));
            Debug.Log("");*/
            /*if (moveDistance != Vector3.zero)
            {*/
            _ridgidBody.position = moveDistance.ToVector2() + _ridgidBody.position;
            //}
            activePlatformPosition = newPlatformPosition;
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
        _ridgidBody.gravityScale = 0;
        _ridgidBody.velocity = Vector2.zero;
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
        _ridgidBody.gravityScale = _gravityScale;
    }

    private void WallJump()
    {
        var horizontalJumpForce = _facingRight ? -10 : 10;
        _ridgidBody.AddForce(new Vector2(horizontalJumpForce, jumpForce/3));
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

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (activePlatformCollider != null)
        {
            return;
        }
        foreach (var contact in other.contacts)
        {
            if (/*contact.moveDirection.y < -0.9 &&*/ contact.normal.y > 0.5 || _grabbingLedge) 
            { 
                activePlatformCollider = contact.collider;
                activePlatform = activePlatformCollider.transform; 
                //activeGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
                activePlatformPosition = activePlatform.transform.position;
            } 
        }
    }
    
    void OnCollisionExit2D(Collision2D other) 
    {
        if (other.collider == activePlatformCollider)
        {
            activePlatformCollider = null;
        }
    }

}
