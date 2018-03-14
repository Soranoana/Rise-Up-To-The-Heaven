using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;// ←new!

public class BossKerube : MonoBehaviour {

	public static int HP;
	private Animator A;
	private GameObject Pl;
	private float targetDir;
	private Vector3 PP;
	private Slider BHPBar;
//	private ParticleSystem Part;
	public GameObject enemy;

	private float dt;

	public GameObject warp1;
	public GameObject warp2;

	private Rigidbody rb;
	// Use this for initialization

	void Start () {
		HP = 200;
		A = GetComponent<Animator> ();
		Pl = GameObject.Find ("Player");
		BHPBar=GameObject.Find("BHPBar").GetComponent<Slider>();
		rb = GetComponent<Rigidbody> ();
		dt = 0f;
	}

	// Update is called once per frame
	void Update () {
		PP = new Vector3(Pl.transform.position.x,this.transform.position.y,Pl.transform.position.z);
		targetDir = Vector3.Distance(PP,this.transform.position);

		if (HP > 0) {

			if (targetDir > 30) {
				A.Play ("Armature|walk");
				rb.AddForce (transform.forward * 1f, ForceMode.VelocityChange);
				transform.LookAt (PP);
				dt = 0;
			} else if(targetDir>10 && targetDir<=30){
				A.Play ("Armature|hoeru");
			} else if(targetDir <= 10){
				A.Play ("Armature|beam");
			}

		}else if (HP <= 0) {
			A.Play ("Armature|down");
			dt += Time.deltaTime;
			if (dt >= 1) {
				Instantiate (enemy, new Vector3 (-54, -4, 116), enemy.transform.rotation);
				Destroy (this.gameObject);
			}	
		}
	}
		
	void OnTriggerEnter (Collider other){
		if (other.gameObject.tag == "Attack") {
			HP -= 10;
		} else if (other.gameObject.tag == "Attack" && CallMain.D) {
			HP -= 200;
		}
	}
}