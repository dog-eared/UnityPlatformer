using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimator : MonoBehaviour {

	public string currentState;
	public Animator _anim;

	/*
	HUMAN ANIMATOR. Animation class intended for humanoid characters. 
	The reason this is split off is to keep the PlayerController and EnemyController classes fairly focused,
	but also because at some point I may want to have the player's arms/head move seperately from the body
	to look around, aim weapons, etc. 

	TODO: Might investigate having a generic EntityAnimator as an interface to the built-in animator to call
	generic .SetIntegers for common types of movement, handle error exception. Then, have specific types of
	character inherit. Come back to this one.

	*/



	void Awake() {
		if (_anim == null) {
			_anim = GetComponent<Animator>();
		}	
	}

	public void SetGrounded(bool state) {
		if (state == true) {
			_anim.SetBool("In Air", true);
		} else {
			_anim.SetBool("In Air", false);
		}
	}

	public void SetAnimation(string newState = "Idle") {

		if (currentState == newState) {
			//No need to do anything!
			return;
		} else if (newState == "Walking") {
			_anim.SetInteger("CurrentAnimation", 1);
		} else if (newState == "Running") {
			_anim.SetInteger("CurrentAnimation", 2);
		} else {
			_anim.SetInteger("CurrentAnimation", 0);
		}

	}

}