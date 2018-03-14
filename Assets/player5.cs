using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;// ←new!
using UnityStandardAssets.CrossPlatformInput;

public class player5 : MonoBehaviour {

	public int HP;					//HP
	public int MP;					//MP
	public GameObject AttackWave;	//攻撃判定
	public GameObject SAttackWave;	//Ｓ攻撃判定
	public Animator ani;			//animator
	public Vector3 PP;				//Playerの場所
	public Quaternion PQ;			//Playerの角度
	public float ATime;				//攻撃の間隔
	public float DTime;				//死亡の間隔
	public bool Battou;				//抜刀しているか
	public float speed;				//移動速度
	public GameObject AttackPosition;//攻撃発生場所
	public Vector3 AP;//攻撃発生場所の情報
	public Slider HPBar;
	public Slider MPBar;
	public bool D;
	public float AB;
	public Rigidbody rb;
	public GameObject P;
	public bool kaidan;
	public Text an;

	public bool warp;
	public float kt;
	public float wt;
	public GameObject Shaka; 

	public GameObject[] enemyObjects;
	public float EnemyCount;
	public GameObject[] bossObjects;
	public float BossCount;

	public bool bosskey;

	public float ct;
	/*追加*/
	Rigidbody rigid;
	private float addspeed;
	private bool isGround;
	private float defoAngle;

	// Use this for initialization
	void Start () {
		HP=100;					
		MP=0;					
		ani = GetComponent<Animator>();
		ATime=0;
		DTime=0;
		Battou = true;
		speed = 0.2f;
		AttackPosition = transform.FindChild("AttackPosition").gameObject;
		HPBar=GameObject.Find("HPBar").GetComponent<Slider>();
		MPBar=GameObject.Find("MPBar").GetComponent<Slider>();
		D = false;
		AB = 0;
		P = GameObject.Find ("Player");
		rb = P.GetComponent<Rigidbody> ();
		kt = 0;
		wt = 0;
		warp = false;
		//		Shaka = GameObject.Find ("Shaka").GetComponent<GameObject>();
		kaidan = false;
		bosskey=false;
		ct = 0;
		rigid = this.GetComponent<Rigidbody>();///////////////////////////////////
		addspeed = 10f;////////////////////////////////
		//カーソル非表示
		//		Cursor.visible = false;
		defoAngle = gameObject.transform.localEulerAngles.x;
	}

	// Update is called once per frame
	void Update ()
	{
		///////////////////////////////////////////////////
		//if (!CheckGrounded()){
		int mask =1 << LayerMask.NameToLayer("Ground");//Groundというレイヤーのオブジェクトのみ対象
		RaycastHit hit;
		//hitしたらtrue
		isGround = Physics.SphereCast(transform.position+Vector3.up*-1/*発射点*/,0.1f/*飛ばす球体の半径*/,-Vector3.up/*飛ばす方向*/,out hit/*hit.collider.gameObjectで取得可能*/,0.67f/*射程*/,mask/*対象のレイヤーマスク*/);
		if(!isGround){//レイヤー的になにもなし
			rigid.AddForce (Vector3.down*10, ForceMode.Acceleration);//Impulse,VelocityChangeはダメ
		}
			
		PP = this.transform.position;
		PQ = this.transform.rotation;

		enemyObjects=Spawn.EnemyObjects;
		EnemyCount=enemyObjects.Length;
		bossObjects=Spawn.BossObjects;
		BossCount=bossObjects.Length;

		if (HPBar.value != HP) {
			HPBar.value = HP;
		}
		if (MPBar.value != MP) {
			MPBar.value = MP;
		}
			
		if (HP <= 0) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene ("GameOver");
		}

		if (D == true) {
			DTime += Time.deltaTime;
			if (DTime >= 3.0f) {
				D = false;
			}
		}	


		rb.velocity = Vector3.zero;
//		if (isGround) {
			if (Input.GetKey (KeyCode.W)) {
				/*変更*/
				keyGo (transform.up * -1);
			}
			if (Input.GetKey (KeyCode.S)) {
				/*変更*/
				keyGo (transform.up);			
			}
			if (Input.GetKey (KeyCode.A)) {
				keyGo (transform.right * -1);
			}
			if (Input.GetKey (KeyCode.D)) {
				keyGo (transform.right * 1);
			}
//		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			transform.Rotate (0, 0, -45f);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			transform.Rotate (0, 0, 45f);
		}

		if (Input.GetKeyDown (KeyCode.X) && Battou == true) {
			speed = 0.02f;
			ani.Play ("Player@Attack");
			Instantiate (AttackWave, PP, new Quaternion(0,PQ.y+90f,0,0));
		}

		if (Input.GetKey (KeyCode.X) && Battou == true) {
			MP += 1;
		}

		if (Input.GetKeyUp (KeyCode.X) && MP >= 100) {
			ani.Play ("Player@Run");
			Battou = false;
			speed = 0.1f;
		}
		if (Input.GetKeyUp (KeyCode.X) && MP != 100) {
			MP = 0;
		}

		if (Input.GetKeyDown (KeyCode.X) && Battou == false) {
			ani.Play ("PlayerS");
			Battou = true;
			speed = 0.02f;
			Instantiate (SAttackWave, PP, new Quaternion (0f, 180f, 0f, 1f));
		}
		if (EnemyCount == 0) {
			bosskey = true;
		}

		if (kaidan == true && bosskey==true) {
			an.text = ("Cキー長押しで登る");		
			if (Input.GetKey (KeyCode.C)) {
				kt += Time.deltaTime;
				if (kt >= 0.5) {
					transform.Translate (0f, -1.25f, 1.25f);
					kt = 0;
				}
			}
		}

		if(warp == true) {
			an.text = ("ボスが出現します");
			wt += Time.deltaTime;

			if (wt >= 3.0f) {
				transform.position = new Vector3(0f,0.66f,0);
				wt = 0;
				Spawn.Boss1 = true;
				bosskey = false;
				Instantiate (Shaka, new Vector3 (0, 0, 50), new Quaternion (0, 180, 0, 1));
			}
		}

		if (kaidan==false && warp == false) {
//			an.text = ("");
		}

		if (BossCount != 0) {
//			an.text = ("");
		}
	
		if (ct >= 5) {
			SceneManager.LoadScene("GameClear");
		}
		//カメラコントロール
		if (gameObject.transform.localEulerAngles.x >= defoAngle + 280 || gameObject.transform.localEulerAngles.x <= defoAngle + 80) {			
		//object上方向80度以下(240度以上)の向きまたは下方向80度の向き
			gameObject.transform.Rotate (-1f * CrossPlatformInputManager.GetAxis ("Mouse Y") * 3f, 0, 0);		//マウス入力y座標でローカルx座標回転

		}else if(gameObject.transform.localEulerAngles.x < defoAngle + 180){
			if (CrossPlatformInputManager.GetAxis ("Mouse Y") > 0) {		//マウス入力上
				gameObject.transform.Rotate (-1f * CrossPlatformInputManager.GetAxis ("Mouse Y") * 3f, 0, 0);		//マウス入力y座標でローカルx座標回転

			}

		} else if (gameObject.transform.localEulerAngles.x > defoAngle + 180){
			if(CrossPlatformInputManager.GetAxis ("Mouse Y") < 0) {		//マウス入力下
				gameObject.transform.Rotate (-1f * CrossPlatformInputManager.GetAxis ("Mouse Y") * 3f, 0, 0);		//マウス入力y座標でローカルx座標回転

			}

		}
		gameObject.transform.Rotate ( 0,CrossPlatformInputManager.GetAxis("Mouse X")*3f,0,Space.World);	//マウス入力x座標でワールドy座標回転
	}



	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "EAttack") {
			HP -= 10;
			D = true;
		}

	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "kaidan") {
			kaidan = true;
		}
		if (other.gameObject.tag == "Warp") {
			warp = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "kaidan") {
			kaidan = false;
		}
		if (other.gameObject.tag == "Warp") {
			warp = false;
		}
		if (other.gameObject.tag == "Damage") {
			HP -= 20;
			D = true;
		}
	}
	/*追加*/
	void keyGo(Vector3 trans){
		rigid.AddForce (trans * addspeed, ForceMode.VelocityChange);
	}
}