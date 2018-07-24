using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DamageOnTouch : MonoBehaviour {

	public int damageDealt = 2;


	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log(col.gameObject.name);
		if (col.gameObject.tag == "Player") {
			PlayerVitals pv;
			try {
				pv = col.gameObject.GetComponent<PlayerVitals>();
				Damage(pv);
			} catch {
				Debug.Log("Player Vitals not found on " + col.gameObject.name);
			}

		} 
	}


	void Damage(PlayerVitals pv) {
		pv.Damage(damageDealt);
	}

}
