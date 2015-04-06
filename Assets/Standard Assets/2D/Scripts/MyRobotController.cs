using UnityEngine;
using System.Collections;
using System.Linq;

public class MyRobotController : MonoBehaviour, ILedgeGrabber {

	private bool _grounded = false;
    private bool _grabbingLedge = false;
	private float _groundRadius = 0.2f;
    private bool _movingRight;

	private Animator _animator;
	private Rigidbody2D _ridgidBody;
    private float _gravityScale;
    private Collider2D _lastLedge;

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

        /*if (!_grounded)
        {
            return;
        }*/

		var move = Input.GetAxis("Horizontal");

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
    }

    public void CanGrabLedge(bool canGrab, Collider2D ledgeCollider)
    {
        if (!canGrab)
        {
            _grabbingLedge = false;
            _lastLedge = null;
            _ridgidBody.gravityScale = _gravityScale;
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

	private void Flip(bool right)
    {
		var scale = transform.localScale;
		if ((right && scale.x > 0) || (!right && scale.x < 0)) {
			return;
		}
		scale.x *= -1;
		transform.localScale = scale;
	}
   
}
