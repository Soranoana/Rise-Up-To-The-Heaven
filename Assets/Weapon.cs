using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	CharacterController controller;
	Animator animator;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Attack () {
		var Attack = GetComponent<ParticleSystem> ();
		Attack.Play ();
	}
}
