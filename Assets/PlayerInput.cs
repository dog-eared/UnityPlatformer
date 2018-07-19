using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	PlayerController _pc;

	Vector2 input;

	float horizontal, vertical;

	/* Private Methods */

	private void Awake() {
		if (_pc == null) {
			_pc = GetComponent<PlayerController>();
		}
	}

	private void Update() {

		if (Input.GetAxisRaw("Horizontal") != 0) {
			horizontal = Input.GetAxisRaw("Horizontal");
		} else {
			horizontal = 0;
		}

		if (Input.GetAxisRaw("Vertical") != 0) {
			vertical = Input.GetAxisRaw("Vertical");
		} else {
			vertical = 0;
		}

		if (Input.GetButton("Fire1")) {
			_pc.Jump();
		}

	}

	private void FixedUpdate() {

		input = new Vector2(horizontal, vertical);

		_pc.Move(input * Time.deltaTime);
	}


}
