using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Attack : MonoBehaviour {

	public Collider Col;
	public Quaternion qua;
	public float time1 = 0;
	public float time2 = 0;
	public ParticleSystem part;
//	private Rigidbody rigid;
//	private GameObject AP;
//	private float targetDir;
//	private GameObject Pl;
//	private Vector3 PP;
	private Rigidbody rb;

	void Start () {
		Col = GetComponent<Collider>();
		time1 = 0;
		time2 = 0;
//		rigid=GetComponent<Rigidbody> ();
//		AP = GameObject.Find ("AttackPosition");
//		targetDir = 0;
//		Pl = GameObject.Find ("AttackPosition").gameObject;
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		time1 += Time.deltaTime;
		time2 += Time.deltaTime;
//		PP = Pl.transform.position;

		if (time1 != 0) {
			rb.AddForce (transform.forward * 1f, ForceMode.VelocityChange);
			time1 = 0;
		}

		if (time2 >= 2f) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Enemy") {
			Destroy (other.gameObject);
			Destroy(this.gameObject);

		}
	}
}
