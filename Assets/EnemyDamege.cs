using UnityEngine;
using System.Collections;

public class EnemyDamege : MonoBehaviour {
	private int eHP;
	void Start () {
		//eHP = 10;
		if (this.transform.name == "Zako") {
			eHP = 10;
		}
		if (this.transform.name == "Zako2") {
			eHP = 15;
		}
		if (this.transform.name == "ika") {
			eHP = 150;
		}
		if (this.transform.name == "dog") {
			eHP = 100;
		}
		if (this.transform.name == "dexyurahan") {
			eHP = 200;
		}
		if (this.transform.name == "Syaka") {
			eHP = 250;
		}
		if (this.transform.name == "doragon") {
			eHP = 500;
		}
	//以下に量産できる
		//	if (this.transform.name == "example") {
	//		eHP = null;
	//	}
	}

	void Update () {
		if (eHP <= 0) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "plAttack1") {
			eHP -= 1;
		}
		//以下に量産できる
		//if (col.gameObject.tag == "plAttackNull") {
		//	eHP -= null;
		//}
	}
}
