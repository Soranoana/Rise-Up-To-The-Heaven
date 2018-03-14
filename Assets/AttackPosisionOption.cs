using UnityEngine;
using System.Collections;

public class AttackPosisionOption : MonoBehaviour {

	public GameObject PP;

	// Use this for initialization
	void Start () {
		PP = GameObject.Find("Player").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (PP.transform.position);
	}
}
