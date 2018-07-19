using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[Header("Movement Config")]
	public float walkSpeed = 10f;
	public float fallSpeed = 14f;
	public float jumpPower = 10f;
	public float jumpLength = .2f;

	[Header("State")]
	[Tooltip("These won't change if you click; they're just public so you can track what's happening")]
	public bool isGrounded = false;
	public bool isJumping = false;

	BoxCollider2D col;
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

	/* Internal Methods */

	private void Awake() {

		if (col == null) {
			col = GetComponent<BoxCollider2D>();
		}

		UpdateBoundingBox();

		vRaySpacing = b.size.x / vRays;
		hRaySpacing = b.size.y / hRays;

		Debug.Log("H " + hRaySpacing + " _ V " + vRaySpacing);


		layerMask = 1 << LayerMask.NameToLayer("World");
	}

	private void FixedUpdate() {

		
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

		UpdateBoundingBox();
	}

	private void CheckHorizontal(ref Vector2 mv) {

		int direction = (mv.x < 0) ? -1 : 1;
		float origin = (direction == 1) ? b.xMax : b.xMin;
		float length = Mathf.Abs(mv.x) + skinWidth;

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
		float length = Mathf.Abs(mv.y) + skinWidth;

		RaycastHit2D check;

		for (int i = 0; i <= vRays; i++) {

			Vector2 currentRay = new Vector2(b.xMin + skinWidth + (i * vRaySpacing), origin);
			check = Physics2D.Raycast(currentRay, Vector2.up * direction, length, layerMask);
			
			Debug.DrawRay(currentRay, Vector2.up * direction * length, Color.red, mv.y + skinWidth);			
			
			if (check && direction == -1) {
				transform.position = new Vector2(transform.position.x, check.point.y - direction * (b.size.y / 2 - skinWidth));
				isGrounded = true;
				return;
			} else if (check) {
				isJumping = false; //bonk your head, start dropping immediately.
				return;
			}



		}
		//check = Physics2D.BoxCast(transform.position, new Vector2(b.size.x - skinWidth, b.size.y - skinWidth), 0, new Vector2(0, direction), dist, layerMask);

		
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
			col.bounds.min.x,
			col.bounds.min.y,
			col.size.x,
			col.size.y);
	}

	private bool CheckGrounded() {
		return Physics2D.BoxCast(transform.position, new Vector2(b.size.x - skinWidth, b.size.y - skinWidth), 0, new Vector2(0, -skinWidth), skinWidth, layerMask);
	}


}
