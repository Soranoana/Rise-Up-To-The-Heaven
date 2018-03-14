using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;// ←new!
public class BossDexra : MonoBehaviour {

	public static int HP;
	private Animator A;
	private GameObject Pl;
	private float targetDir;
	private Vector3 PP;
	private Slider BHPBar;
	private GameObject Beam;
//	public GameObject BeamP;
	private ParticleSystem Part;

	private float dt;

	private Rigidbody rb;
	// Use this for initialization

	void Start () {
		HP = 200;
		A = GetComponent<Animator> ();
		Pl = GameObject.Find ("Player");
		BHPBar=GameObject.Find("BHPBar").GetComponent<Slider>();
		Beam = GameObject.Find ("Beam");
		rb = GetComponent<Rigidbody> ();
		dt = 0f;
	}

	// Update is called once per frame
	void Update () {
		PP = new Vector3 (Pl.transform.position.x, -15, Pl.transform.position.z);
		targetDir = Vector3.Distance (PP, this.transform.position);

		if (HP > 0) {

			if (targetDir > 20) {
				A.Play ("Armature|walk");
				rb.AddForce (transform.forward * 1.5f, ForceMode.VelocityChange);
				transform.LookAt (PP);
			}else{
				A.Play ("Armature|attack");
			} 

			if (HP <= 0) {
				A.Play ("Armature|down");
				dt += Time.deltaTime;
				if (dt >= 4) {
					Destroy (this.gameObject);
				}
			}
		}
	}	

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Attack") {
			HP -= 10;
		}else if (other.gameObject.tag == "Attack" && CallMain.D) {
			HP -= 200;
			}
		}
	}
