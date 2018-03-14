using UnityEngine;
using System.Collections;
public class enemyGoBoss : MonoBehaviour {
//	private CharacterController eCon;		//CharacterControllerをeConと呼ぶ
	private Vector3 destination;	//目的地
	private Vector3 Sdestination;
	private float speed;	//スピード値
//	private Vector3 velocity;        //速度×方向
	private Vector3 direction;       //移動方向
	private Vector3 was;			//現在位置の一つ手前
	private Vector3 Tpoint;			//実ベクトル
	private bool arrived;		//目的地到着フラグ
	private float waitTime;		//待ち移行時間
	private float currentTime;	//現在時間
	private int state;		//モード
	private int i;			//便利関数
	public GameObject Player;		//ゲームオブジェクトPlayer取得
	RaycastHit hit;
	private int flameCount;
	public GameObject Ground;
	public GameObject AttackObject;		//テスト用攻撃オブジェクト
	/*enum StateB {		//モードの種類
	wait,			//待機		1
	chase,			//Player追いかけ＆攻撃	2
	};*/
/*------------------------------スタート-----------------------------------*/
	void Start () {
		Player = GameObject.Find("Player");		//ゲームオブジェクトPlayerを見つけてPlayerに格納
//		eCon = GetComponent<CharacterController>();		//CharacterControllerをeConと呼ぶ
		speed = 10.0f;					//移動スピード設定 パーセント表記
		arrived = false;				//arrived初期化　目的地についていない
//		velocity = Vector3.zero;		//移動ベクトルを初期化
		waitTime = 200.0f;				//待ち移行時間初期化
		currentTime = 0.0f;				//現在時間初期化
		setState(/*"chase"*/2);			//モードをwalk、目的地の設定
		Ground = GameObject.FindGameObjectWithTag("Ground");
		flameCount = 0;
		was = Vector3.zero;
	}
/*------------------------------アップデート-----------------------------------*/
	void Update () {
		flameCount++;
		if(Vector3.Distance(destination, transform.position) >=2) {
			arrived=false;
		} else if(Vector3.Distance(destination, transform.position) < 2) {			//目的地までの距離が2以下か
			arrived = true;						//目的地到着
		}
		if(!arrived){						//目的地に到着していない
			if (state == /*State.chase*/2) {			//モードがchaseか
				walk (Player.transform.position);
			}
		} else if(arrived){	//目的地に到着している
			if (state == /*State.chase*/2) {	//stateがchaseの場合
//				GameObject attack =GameObject.Instantiate(AttackObject)as GameObject;
//				attack.transform.position = this.transform.position;
				setState (/*"wait"*/1);
			}
		}
		if(state == /*State.wait*/1) {			//モードがwaitか
			wait();				//待ち関数呼び出し
		}
	}
/*------------------------------モードチェンジ-----------------------------------*/
	void setState(int nextState) {
		if(nextState == /*"chase"*/2) {			//chaseで呼び出されたか
			state = /*State.chase*/2;				//モードをchaseに設定
			arrived = false;				//　待機状態から追いかける場合もあるのでOff		目的地についていない
			destination =setDestination(Player.transform.position);						//　追いかける対象をセット
		} else if(nextState == /*"wait"*/1) {			//waitで呼び出されたか
			state = /*State.wait*/1;				//モードをwaitに設定
			arrived = true;					//目的地に到着
			wait();
		}
	}
/*------------------------------歩き回る-----------------------------------*/
	void walk(Vector3 position) {				//歩行関数
		destination =setDestination(position);		//目的地の生成
		if(CheckGrounded()) {				//エネミーが接地しているか
			Vector3 target = new Vector3(destination.x, transform.position.y+10000, destination.z);
			transform.LookAt(target);			//移動方向を向く
			if(flameCount%50!=0){
				direction = (destination - transform.position).normalized;		//移動方向の割出
				Tpoint = (direction - (transform.position - was));//*Vector3.Distance(destination, transform.position);
				this.GetComponent<Rigidbody> ().AddForce (Tpoint/100*speed, ForceMode.VelocityChange);//Force,Accelerationはダメ

//				direction = (destination - transform.position).normalized;		//移動方向の割出
//				Tpoint = (direction/* - (transform.position - was).normalized*/);//*Vector3.Distance(destination, transform.position);
//				this.GetComponent<Rigidbody>().AddForce((transform.position-was).normalized/100*speed/2,ForceMode.Impulse);
//				this.GetComponent<Rigidbody> ().AddForce (Tpoint/100*speed, ForceMode.Impulse);//Force,Accelerationはダメ
			}else{
				this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
			//Debug.Log ("walk");
		}else if (!CheckGrounded()){
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			this.GetComponent<Rigidbody> ().AddForce (Vector3.down*10, ForceMode.Acceleration);//Impulse,VelocityChangeはダメ
			//Debug.Log ("noWalk noGround");
		}
		was = transform.position;
	}
/*------------------------------止まる-----------------------------------*/
	void wait (){					//待ち関数
		state=/*State.wait*/1;						//モードをwaitに
		currentTime+=1; 							//時間経過
		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
//		this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		//Debug.Log ("wait");
		if(waitTime < currentTime) {			//waitTimeを過ぎたか
			currentTime = 0.0f;				//currentTime初期化
			setState(/*"walk"*/2);			//モードをwalkに移行
			//Debug.Log("toWalk");
		}
	}
/*------------------------------接地判定-----------------------------------*/
	bool CheckGrounded(){			//地面に接地しているか調べる
		if (transform.position.y - Ground.transform.position.y <= 1.4) {
			//Debug.Log ("isGround");
			return true;
		} else {
			//Debug.Log ("noGround");
			return false;
		}
	}

/*------------------------------入力値を目的地にする-----------------------------------*/
	public Vector3 setDestination(Vector3 position) {
		Sdestination = position;//入力値を目的地に設定
		Sdestination.y = transform.position.y;//入力値を目的地に設定
		return Sdestination;//目的地を設定
	}
}