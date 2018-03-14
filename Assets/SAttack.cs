using UnityEngine;
using System.Collections;

public class SAttack : MonoBehaviour {

	public Collider Col;
	// Use this for initialization

	public float time = 0;

	void Start () {
		Col = GetComponent<Collider>();
	}

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time >= 0.5f) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.tag=="Enemy") {
			Destroy (other.gameObject);
		}
	}
}
