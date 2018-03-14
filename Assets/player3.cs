using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;// ←new!
using UnityStandardAssets.CrossPlatformInput;

public class player3 : MonoBehaviour {

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
	public Slider HPBar;			//体力バー
	public Slider MPBar;			//魔力バー
	public GameObject Part;
	public ParticleSystem part;		//パーティクル
	public bool D;					//ダメージ判定
	public float AB;				
	public Rigidbody rb;			//rigidbody			
	public GameObject P;

	public bool warp;
	public float kt;
	public float wt;
	public GameObject Shaka; 

	public GameObject[] enemyObjects;
	public float EnemyCount;
	public GameObject[] bossObjects;
	public float BossCount;

	public bool bosskey;

	public float BHP;
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
		Part=transform.FindChild("AttackSystem").gameObject;
		part=Part.GetComponent<ParticleSystem>();
		D = false;
		AB = 0;
		P = GameObject.Find ("Player");
		rb = P.GetComponent<Rigidbody> ();
		kt = 0;
		wt = 0;
		warp = false;
		//		Shaka = GameObject.Find ("Shaka").GetComponent<GameObject>();

		bosskey=false;
		BHP = BossShaka.HP;
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
		int mask = 1<< LayerMask.GetMask (new string[] {"Ground"});//Groundというレイヤーのオブジェクトのみ対象
//		mask=1;
		RaycastHit hit;
		//hitしたらtrue
		isGround = Physics.SphereCast (transform.position/* + Vector3.up * 1*//*発射点*/, 0.1f/*飛ばす球体の半径*/, Vector3.down/*飛ばす方向*/, out hit/*hit.collider.gameObjectで取得可能*/, 1.2f/*射程*/, mask/*対象のレイヤーマスク*/);
	//		Debug.Log (hit.collider.gameObject);
		
			if (!isGround) {//レイヤー的になにもなし
				rigid.AddForce (Vector3.down * 200, ForceMode.Force);//Impulse,VelocityChangeはダメ
		}

		Debug.Log ("isg "+isGround);
			
		PP = this.transform.position;
		PQ = this.transform.rotation;
		BHP = BossShaka.HP;

/*		enemyObjects = Spawn.EnemyObjects;
		EnemyCount = Spawn.EnemyCount;
		bossObjects = Spawn.BossObjects;
		BossCount = Spawn.BossCount;
*/
		if (HPBar.value != HP) {
			HPBar.value = HP;
		}
		if (MPBar.value != MP) {
			MPBar.value = MP;
		}
		if (D == true) {
			DTime += Time.deltaTime;
			if (DTime >= 3.0f) {
				D = false;
			}
		}	

			rb.velocity = Vector3.zero;
		if (HP >= 0) {
			if (Input.GetKey (KeyCode.W)) {
				/*変更*/
					keyGo (transform.forward * 1);
			}

			if (Input.GetKey (KeyCode.S)) {
				/*変更*/
					keyGo (transform.forward*-1);			
			}
			if (Input.GetKey (KeyCode.A)) {
				keyGo (transform.right * -1);
			}
			if (Input.GetKey (KeyCode.D)) {
				keyGo (transform.right * 1);
			}
//		}
			if (Input.GetKeyDown (KeyCode.Space) && Battou == true) {
				speed = 0.02f;
				ani.Play ("Player@Attack");
				part.Play ();
					Instantiate (AttackWave, PP, new Quaternion ((PQ.x+45f),PQ.y,PQ.z, 1));

			}

			if (Input.GetKey (KeyCode.Space) && Battou == true) {
				MP += 1;
			}

			if (Input.GetKeyUp (KeyCode.Space) && MP >= 100) {
				ani.Play ("Player@Run");
				Battou = false;
				speed = 0.1f;
			}
			if (Input.GetKeyUp (KeyCode.Space) && MP != 100) {
				MP = 0;
			}

			if (Input.GetKeyDown (KeyCode.Space) && Battou == false) {
				ani.Play ("PlayerS");
				Battou = true;
				speed = 0.02f;
				Instantiate (SAttackWave, PP, new Quaternion (0f, 180f, 0f, 1f));
			}
		}
		if (HP <= 0) {
//			Application.LoadLevel("GameOver");
			SceneManager.LoadScene ("GameOver");
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
		if (other.gameObject.tag == "Enemy") {
			HP -= 10;
			D = true;
				}

	}
	void OnTriggerStay(Collider other)
	{
		
	}

	void OnTriggerExit(Collider other)
	{
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