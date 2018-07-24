using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitals : MonoBehaviour {

	public int maxHP = 10;
	public int currentHP = 10;

	

	public void Damage(int damage) {
		currentHP -= damage;
	}



}
