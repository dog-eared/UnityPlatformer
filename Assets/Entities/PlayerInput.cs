using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	public GameObject _playerObj;
	PlayerController _pc;

	Vector2 input;

	float horizontal, vertical;

	/* Private Methods */

	private void Awake() {

		if (_playerObj == null) {
			_playerObj = GameObject.FindWithTag("PlayerCharacter");
		}

		if (_pc == null) {
			_pc = _playerObj.GetComponent<PlayerController>();
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

		if (Input.GetButtonDown("Fire3")) {
			Debug.Log("Shooting");
		}

		if (Input.GetButtonDown("Fire2")) {
			_pc.Run(true);
		} else if (Input.GetButtonUp("Fire2")) {
			_pc.Run(false);
		}

	}

	private void FixedUpdate() {

		input = new Vector2(horizontal, vertical);

		_pc.Move(input * Time.deltaTime);
	}


}
