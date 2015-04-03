using UnityEngine;
using System.Collections;

public class MyRobotController : MonoBehaviour {

	private bool _grounded = false;
	private float _groundRadius = 0.2f;

	private Animator _animator;
	private Rigidbody2D _ridgidBody;

	public float maxSpeed = 6.0f;
	public Transform groundCheck;
	public LayerMask whatIsGround;
    public float jumpForce = 700.0f;

	// Use this for initialization
	void Start ()
    {
		_animator = GetComponent<Animator>();
		_ridgidBody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate ()
    {
		_grounded = Physics2D.OverlapCircle(groundCheck.position, _groundRadius, whatIsGround);
        _animator.SetBool("Grounded", _grounded);
        _animator.SetFloat("VerticalSpeed", _ridgidBody.velocity.y);

        if (!_grounded)
        {
            return;
        }

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
