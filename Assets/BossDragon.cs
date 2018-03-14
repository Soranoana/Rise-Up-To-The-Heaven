using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;// ←new!
public class BossDragon : MonoBehaviour {


	public static int HP;
	public Animator A;
	private GameObject Pl;
	public float targetDir;
	public Vector3 PP;
	public Slider BHPBar;
	public GameObject Beam;
	public GameObject BeamP;
	public ParticleSystem Part;

	public float dt;

	private Rigidbody rb;
	// Use this for initialization

	void Start () {
		HP = 200;
		A = GetComponent<Animator> ();
		Pl = GameObject.Find ("Player");
		BHPBar=GameObject.Find("BHPBar").GetComponent<Slider>();
		Beam = GameObject.Find ("Beam");
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		rb.constraints =RigidbodyConstraints.FreezeRotationX; 
		rb.constraints =RigidbodyConstraints.FreezeRotationZ;
		PP = new Vector3(Pl.transform.position.x,4,Pl.transform.position.z);
		targetDir = Vector3.Distance(PP,this.transform.position);

		if (HP > 0) {

			if (targetDir > 50) {
				A.Play ("Armature|walk");
				rb.AddForce (transform.forward * 1f, ForceMode.VelocityChange);
				transform.LookAt (PP);
				dt = 0;
			} else if(targetDir>20 && targetDir<=50){
				A.Play ("Armature|");
				dt+=Time.deltaTime;
				if(dt>=3){
					Instantiate(Beam,BeamP.transform.position,this.transform.rotation);
				}else if(dt>=6){
					dt=0;
				}
			} else if(targetDir <= 20){
				A.Play ("Armature|");
			}

		}else if (HP <= 0) {
			A.Play ("Armature|death");
			dt += Time.deltaTime;
			if (dt >= 4) {
				Spawn.Stage6Clear = true;
				Destroy (this.gameObject);
			}	
		}
	}


	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Attack") {
			HP -= 10;
		} else if (other.gameObject.tag == "Attack" && CallMain.D) {
			HP -= 200;
		}
	}
}