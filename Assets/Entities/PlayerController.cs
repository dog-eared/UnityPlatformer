using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[Header("Movement Config")]
	public float walkSpeed = 10f;
	public float fallSpeed = 14f;
	public float jumpPower = 10f;
	public float jumpLength = .2f;
	public float runMultiplier = 1.20f;

	[Header("State")]
	[Tooltip("These won't change if you click; they're just public so you can track what's happening")]
	public bool isGrounded = false;
	public bool isJumping = false;
	public bool isRunning = false;

	HumanAnimator _anim;
	SpriteRenderer _rend;
	BoxCollider2D _col;
	int layerMask;

	Vector2 movement = new Vector2(0, 0);
	Rect b; //boundary box

	const float skinWidth = 0.15f; 
	const int hRays = 6;
	const int vRays = 5;

	float hRaySpacing;
	float vRaySpacing;


	/* Public Methods */

	public void Move(Vector2 input) {
		movement = new Vector2(input.x * walkSpeed, movement.y);
	}

	public void Jump() {
		if (isGrounded && !isJumping) {
			isJumping = true;
			isGrounded = false;
			Invoke("JumpFinish", jumpLength);
		}
	}

	public void Run(bool toggle) {
		if (!isRunning && toggle == true) {
			isRunning = true;
		} else if (isRunning && toggle == false) {
			isRunning = false;
		}
	}

	/* Internal Methods */

	private void Awake() {

		if (_col == null) {
			_col = GetComponent<BoxCollider2D>();
		}

		if (_anim == null) {
			_anim = GetComponent<HumanAnimator>();
		}

		if (_rend == null) {
			_rend = GetComponent<SpriteRenderer>();
		}

		UpdateBoundingBox();

		vRaySpacing = b.size.x / vRays;
		hRaySpacing = b.size.y / hRays;

		layerMask = 1 << LayerMask.NameToLayer("World");
	}

	private void FixedUpdate() {

		if (isRunning) {
			ApplyRunning();
		}

		
		if (movement.x != 0) {
			CheckHorizontal(ref movement);
		}

		if (movement.y != 0) {
			CheckVertical(ref movement);
		}


		if (isGrounded && !isJumping) {
			isGrounded = CheckGrounded();
		}

		ApplyGravity();

		transform.Translate(movement);

		ApplyAnimation();

		UpdateBoundingBox();
	}

	private void CheckHorizontal(ref Vector2 mv) {

		int direction = (mv.x < 0) ? -1 : 1;
		float origin = (direction == 1) ? b.xMax : b.xMin;
		float length = Mathf.Abs(mv.x);
		
		RaycastHit2D check;

		for (int i = 0; i <= hRays; i++) {

			Vector2 currentRay = new Vector2(origin, b.yMin + skinWidth + (i * hRaySpacing));
			check = Physics2D.Raycast(currentRay, Vector2.right * direction, length, layerMask);

			Debug.DrawRay(currentRay, Vector2.right * direction * length, Color.yellow, length);	

			if (check) {
				mv.x = (check.distance - skinWidth) * direction;
				return;
			}
		}
	}


	private void CheckVertical(ref Vector2 mv) {

		if (isGrounded) {
			mv.y = 0;
			return;
		}

		int direction = (mv.y < 0) ? -1 : 1;
		float origin = (direction == 1) ? b.yMax : b.yMin;
		float length = Mathf.Abs(mv.y);

		RaycastHit2D check;

		for (int i = 0; i <= vRays; i++) {

			Vector2 currentRay = new Vector2(b.xMin+ (i * vRaySpacing), origin);
			check = Physics2D.Raycast(currentRay, Vector2.up * direction, length, layerMask);
			
			Debug.DrawRay(currentRay, Vector2.up * direction * length, Color.red, mv.y + skinWidth);			
			
			if (check) {
				//TO DO: Right now our check is responding to the leftmost ray which isn't as accurate as I'd like
				//Will need to play w. this to get slopes acting as expected


				transform.position = new Vector2(transform.position.x, check.point.y + -direction * (b.size.y / 2));
				isGrounded = true;

				if (direction == 1) {
					isJumping = false;
				} else {
					isGrounded = true;
				}

				return;
			}



		}
		//check = Physics2D.BoxCast(transform.position, new Vector2(b.size.x - skinWidth, b.size.y - skinWidth), 0, new Vector2(0, direction), dist, layerMask);

		
	}

	private void ApplyRunning() {
		movement.x *= runMultiplier;
	}

	private void ApplyAnimation() {

		if (isGrounded) {
			if (movement.x != 0 && isRunning) { 
				_anim.SetAnimation("Running");
			} else if (movement.x != 0) {
				_anim.SetAnimation("Walking"); 
			} else {
				_anim.SetAnimation();
			}

			if (movement.x > 0) {
				FlipX(false);
			} else if (movement.x < 0) {
				FlipX(true);
			}
		}
	
	}

	private void FlipX(bool toLeft) {
		//Written out in long form so this method doesn't keep setting flipX if it's already set

		if (_rend.flipX == false && toLeft) {
			_rend.flipX = true;
		} else if (_rend.flipX == true && !toLeft) {
			_rend.flipX = false;
		}
	}

	private void ApplyGravity() {

		float y;

		if (isJumping) {
			y = jumpPower * Time.deltaTime;
		} else if (!isGrounded) {
			y = -fallSpeed * Time.deltaTime;
			isGrounded = CheckGrounded();
		} else {
			y = 0;
		}

		movement.y = y;
	}

	private void JumpFinish() { 
		isJumping = false;
	}

	private float CheckSlopeDown(int direction) {
		/*Vector2 origin =  (direction == 1) ? b.xMin : b.xMax;
	
		Vector2 a = Physics2D.Raycast(origin, -Vector2.up, )*/
		return 0f;

	}

	private void UpdateBoundingBox() {

		//Set boundaries to b for shorthand

		b = new Rect(
			_col.bounds.min.x,
			_col.bounds.min.y,
			_col.size.x,
			_col.size.y);
	}

	private bool CheckGrounded() {
		return Physics2D.BoxCast(transform.position, new Vector2(b.size.x - skinWidth, b.size.y - skinWidth), 0, new Vector2(0, -skinWidth), skinWidth, layerMask);
	}


}
