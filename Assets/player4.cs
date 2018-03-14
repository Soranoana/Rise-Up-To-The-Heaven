using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class player4 : MonoBehaviour {

	private int HP;					//HP
	private int MP;					//MP
	public GameObject AttackWave;	//攻撃判定
	public GameObject SAttackWave;	//Ｓ攻撃判定
	private Animator ani;			//animator
	private bool Battou;				//抜刀しているか
	private float speed;				//移動速度関数
	private float RunSpeed;			//Run時の移動速度
	private float WalkSpeed;			//Walk時の移動速度
	private Slider HPBar;
	private Slider MPBar;
	private ParticleSystem part;
	private GameObject[] enemyObjects;
//	private float EnemyCount;
	private	Rigidbody rigid;
	private float addspeed;
	private bool isGround;

//	private GameObject[] warp;

	void Start () {
		HP = 100;					
		MP = 0;					
		ani = this.GetComponent<Animator> ();
		Battou = true;
		RunSpeed = 0.1f;
		WalkSpeed = 0.02f;
		HPBar = GameObject.Find ("HPBar").GetComponent<Slider> ();
		MPBar = GameObject.Find ("MPBar").GetComponent<Slider> ();
		part = transform.FindChild ("AttackSystem").gameObject.GetComponent<ParticleSystem> ();
		rigid = this.GetComponent<Rigidbody> ();
		addspeed = 10f;
		//カーソル非表示
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
//		warp = GameObject.FindGameObjectsWithTag ("warp");
/*		int i = 0;
		for (i = 0; i < 20; i++) {
			warp[i].SetActive (false);
		}
	
*/
	}
	void Update ()
	{

		if(!isGround){//レイヤー的になにもなし
			rigid.AddForce (Vector3.down*500, ForceMode.Acceleration);//Impulse,VelocityChangeはダメ
		}

		enemyObjects=Spawn.EnemyObjects;
//		EnemyCount=enemyObjects.Length;

		if (HPBar.value != HP) {
			HPBar.value = HP;
		}
		if (MPBar.value != MP) {
			MPBar.value = MP;
		}
		//ゲームオーバー処理
		if (HP <= 0) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene ("GameOver");
		}

		this.GetComponent<Rigidbody> ().velocity = Vector3.zero;

		if (Input.GetKey (KeyCode.W)) {
			keyGo (transform.forward * 1);
		}
		if (Input.GetKey (KeyCode.S)) {
			keyGo (transform.forward* -1);			
		}
		if (Input.GetKey (KeyCode.A)) {
			keyGo (transform.right * -1);
		}
		if (Input.GetKey (KeyCode.D)) {
			keyGo (transform.right * 1);
		}
		//カーソル表示
		if (Input.GetKey (KeyCode.Escape)) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		Vector3 PP = this.transform.position;
		Quaternion PQ = this.transform.rotation;

		if (Input.GetMouseButtonDown(0) && Battou) {
			speed = WalkSpeed;
			ani.Play ("Player@Attack");
			part.Play ();
			Instantiate (AttackWave, PP/*this.transform.position*/, new Quaternion(PQ.x+45,PQ.y,PQ.z,0)/*this.transform.rotation*/);
		}

		if (Input.GetMouseButton(0) && Battou) {
//			Debug.Log ("Charge");
			if (MP < 100) {
				MP += 1;
			}
		}

		if (Input.GetMouseButtonUp(0) && MP >= 100) {
//			Debug.Log ("Run");
			ani.Play ("Player@Run");
			Battou = false;
			speed = RunSpeed;
		}

		if (Input.GetMouseButtonUp(0) && MP < 100) {
			MP = 0;
		}

		if (Input.GetMouseButtonDown(0) && !Battou) {
			ani.Play ("PlayerS");
			Battou = true;
			speed = WalkSpeed;
			Instantiate (SAttackWave, this.transform.position, new Quaternion (0f, 180f, 0f, 1f));
			MP = 0;
		}

		//カメラコントロール
		if (gameObject.transform.localEulerAngles.x >= 280 || gameObject.transform.localEulerAngles.x <= 80) {
			gameObject.transform.Rotate (-1f * CrossPlatformInputManager.GetAxis ("Mouse Y") * 3f, 0, 0);
		} else if (gameObject.transform.localEulerAngles.x < 180) {
			if (CrossPlatformInputManager.GetAxis ("Mouse Y") > 0) {		//マウス入力上
				gameObject.transform.Rotate (-1f * CrossPlatformInputManager.GetAxis ("Mouse Y") * 3f, 0, 0);		//マウス入力y座標でローカルx座標回転
			}
		} else if (gameObject.transform.localEulerAngles.x > 180) {
			if (CrossPlatformInputManager.GetAxis ("Mouse Y") < 0) {		//マウス入力下
				gameObject.transform.Rotate (-1f * CrossPlatformInputManager.GetAxis ("Mouse Y") * 3f, 0, 0);		//マウス入力y座標でローカルx座標回転
			}
		}
		gameObject.transform.Rotate ( 0,CrossPlatformInputManager.GetAxis("Mouse X")*3f,0,Space.World);	//マウス入力x座標でワールドy座標回転

	}
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Enemy" && CallMain.D) {
			HP -= 10;
		}
		if (other.gameObject.tag == "Boss" && CallMain.D) {
			HP -= 15;
		}
	}

	void OnCollisionStay(Collision col){
		if (col.gameObject.tag == "Ground") {
			isGround = true;
		}
	}

	void OnCollisionExit(Collision col){
		if (col.gameObject.tag == "Ground") {
			isGround = false;
		}
	}

	void keyGo(Vector3 trans){
		rigid.AddForce (trans * addspeed, ForceMode.VelocityChange);
	}
}