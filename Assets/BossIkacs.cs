using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;// ←new!
public class BossIkacs : MonoBehaviour {


	public static int HP;
	public Animator A;
	private Vector3 C;
	public float targetDir;
	public Slider BHPBar;
	public ParticleSystem Part;

	public float dt;

	private GameObject center;


	private Rigidbody rb;
	// Use this for initialization

	void Start () {
		HP = 200;
		A = GetComponent<Animator> ();
		BHPBar = GameObject.Find ("BHPBar").GetComponent<Slider> ();
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		C = GameObject.Find ("center").transform.position;
		targetDir = Vector3.Distance(C,this.transform.position);
		targetDir = 30;

		if (HP > 0) {

			if (targetDir == 30) {
				A.Play ("Armature|walk");
				rb.AddForce (transform.forward * 1f, ForceMode.VelocityChange);
				transform.LookAt (C);
				dt = 0;
			

			} else if(targetDir <= 20){
				A.Play ("Armature|");
			}

		}else if (HP <= 0) {
			A.Play ("Armature|death");
			dt += Time.deltaTime;
			if (dt >= 4) {
				Destroy (this.gameObject);
				}	
		}
	}


	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Attack") {
			HP -= 10;
		}		else if (other.gameObject.tag == "Attack" && CallMain.D) {
				HP -= 200;
			}
		}
	}