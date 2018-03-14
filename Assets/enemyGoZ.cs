using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using UnityEngine.SceneManagement;// ←new!
public class enemyGoZ : MonoBehaviour {

	//public static int HP;
	private int HP;
//	public Animator A;
	private GameObject Pl;
	private float targetDir;
	private Vector3 PP;
	private Vector3 LookPP;

	private float dt;

	private Rigidbody rb;
	// Use this for initialization

	void Start () {
		HP = 10;
//		A = GetComponent<Animator> ();
		Pl = GameObject.Find ("Player");
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		
		PP = new Vector3 (Pl.transform.position.x, transform.position.y ,Pl.transform.position.z);
		targetDir = Vector3.Distance (PP, this.transform.position);
		LookPP = new Vector3 (PP.x, PP.y + 100, PP.z);

		if (HP > 0) {
			if (targetDir <= 30) {
				rb.AddForce (transform.up * -0.15f, ForceMode.VelocityChange);
				transform.LookAt (LookPP);
			} else {
				transform.Rotate (0, 0, 0f);
			}
		}else {
			Destroy (gameObject);
		}
	}


	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Attack") {
			HP -= 10;
		}
	}
}