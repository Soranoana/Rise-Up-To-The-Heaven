using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		transform.Rotate (1, 0, 0);
	
	}
	void OnCollisionEnter(Collision collision) {
		
		if (collision.gameObject.tag == "Player") {

			Destroy(this.gameObject);


		}
	}
}