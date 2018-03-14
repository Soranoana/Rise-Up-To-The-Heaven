using UnityEngine;
using System.Collections;

public class light : MonoBehaviour {
	private GameObject player;
	private float distance;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		transform.FindChild ("Light").gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		distance = Vector3.Distance (player.transform.position, transform.position);
		if (distance <= 20) {
			transform.FindChild ("Light").gameObject.SetActive (true);
		} else {
			transform.FindChild ("Light").gameObject.SetActive (false);
		}
	}
}
