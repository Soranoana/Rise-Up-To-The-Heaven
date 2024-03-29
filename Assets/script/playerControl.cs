﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class playerControl : MonoBehaviour {

	private float walkSpeed = 10;
	private Vector3 walkVector = Vector3.zero;
	private Vector2 walkVector2 = Vector2.zero;
	private bool inputed = false;
	private Rigidbody rigid;
	public float GravityForce;
	public float runSpeed;
	//CanvasUI用
	//[SerializeField]
	private GameObject CanvasUI;
	private GameObject HPbar;
	private GameObject StaminaBar;
	private GameObject weapon1Texture;
	private GameObject weapon2Texture;
	private GameObject weapon3Texture;
	private GameObject RedEye;
	public Texture[] texture;
	//ステータス
	private int HPMax;                  //最大HP
	private int HPNow;                  //現在HP
	private int StaminaMax;             //最大スタミナ
	private int StaminaNow;             //現在スタミナ
	private int NowWeapon;              //現在装備しているもの
	private int NowInventoryNum = 0;    //現在装備しているものを管理するための添え字
	private int inventoryCapacity;       //インベントリのキャパシティ
	private int[] inventoryList;
	//アイテム用
	// public ItemAndWeaponSet[] InventorySet;
	public string filePath;
	private StreamReader sr;
	//スタミナ回復量
	private int staminaRegain = 1;
	//スタミナを消費している
	private bool isStaminaUse = false;
	//スタミナが切れた
	private bool isStaminaLost = false;

	//ハンドオブジェクト　及び攻撃
	private GameObject rightHand;
	private controllWeaponOnHands rightHandScript;

	//重力を適用する
	public bool useGravity;
	//上へ移動を可能する
	public bool canMoveToUp;

	[SerializeField]
	private OnGround onGround;

	private void Awake() {
		rightHand = transform.Find("handRight").gameObject;
		rightHandScript = rightHand.GetComponent<controllWeaponOnHands>();
	}

	void Start() {
		rigid = this.gameObject.GetComponent<Rigidbody>();
		GravityForce = 98f;
		CanvasUI = GameObject.Find("CanvasUI");
		//カーソル非表示
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		//CanvasUI
		HPbar = CanvasUI.transform.Find("HP").gameObject;
		StaminaBar = CanvasUI.transform.Find("Stamina").gameObject;
		weapon1Texture = CanvasUI.transform.Find("weapon1").gameObject;
		weapon2Texture = CanvasUI.transform.Find("weapon2").gameObject;
		weapon3Texture = CanvasUI.transform.Find("weapon3").gameObject;
		RedEye = transform.Find("Camera").transform.Find("CanvasColor").transform.Find("RawImage").gameObject;
		//アイテム表の初期化　必要
		//ステータス
		HPMax = 150;
		HPNow = 120;
		StaminaMax = 120;
		StaminaNow = 100;
		NowWeapon = 0;
		NowInventoryNum = 0;
		SetHPAndStaminaAndRedEyeOnFirst();      //スタミナとHP初期化
												//インベントリ初期化　必要
		for (int i = 0; i < inventoryCapacity; i++) {
			if (PlayerPrefs.HasKey("item" + i.ToString()) && false) {
				inventoryList[i] = PlayerPrefs.GetInt("item" + i.ToString());
			} else {
				inventoryList[i] = 0;
			}
		}
		inventoryList[0] = 1;
		inventoryList[1] = 2;
		inventoryList[2] = 3;
		inventoryList[3] = 4;
		inventoryList[4] = 5;
		inventoryList[5] = 6;
		inventoryList[6] = 17;
	}

	void Update() {
		CameraControll();
		SetHPAndStaminaAndRedEyeOnUpdate();
		//武器変更
		if (Input.GetKeyUp(KeyCode.E)) {
			itemChange();
		}
		weaponTextureChange();
		//inventorySort();
		//スタミナ消費中でなければ回復
		if (!isStaminaUse) {
			StaminaDamegeManeger(staminaRegain);
		}

		if (Input.GetMouseButtonUp(0)) {
			//Debug.Log("attack");
			rightHandScript.useWeapon(1);
		} else if (Input.GetMouseButtonUp(1)) {
			//Debug.Log("attack");
			rightHandScript.useWeapon(2);
		}
	}

	private void FixedUpdate() {
		//重力
		if (useGravity)
			rigid.AddForce(Vector3.down * GravityForce, ForceMode.Acceleration);

		PcArrowMove();
	}

	/* 矢印キー入力による移動 */
	private void PcArrowMove() {
		inputed = false;
		if (( Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ) && StaminaNow > 0) {
			isStaminaUse = true;
			StaminaDamegeManeger(0);
			runSpeed = 2.5f;
		} else {
			isStaminaUse = false;
			runSpeed = 1;
		}
		if (onGround.IsOnGround()) {
			rigid.velocity = Vector3.zero;
			if (Input.GetKey(KeyCode.W)) {
				walkVector = transform.TransformVector(new Vector3(0, 0, 1) * walkSpeed * runSpeed);
				walkVector2 = new Vector2(walkVector.x, walkVector.z);
				//不自然に上に行かないようにする
				if (!canMoveToUp) {
					walkVector = new Vector3(walkVector.x, onGround.RayObjectHorizontalVector.y * walkVector2.magnitude / walkVector2.normalized.magnitude, walkVector.z);
				}
				rigid.velocity += walkVector;
				inputed = true;
			}
			if (Input.GetKey(KeyCode.A)) {
				walkVector = transform.TransformVector(new Vector3(-1, 0, 0) * walkSpeed * runSpeed);
				walkVector2 = new Vector2(walkVector.x, walkVector.z);
				//不自然に上に行かないようにする
				if (!canMoveToUp) {
					walkVector = new Vector3(walkVector.x, onGround.RayObjectHorizontalVector.y * walkVector2.magnitude / walkVector2.normalized.magnitude, walkVector.z);
				}
				rigid.velocity += walkVector;
				inputed = true;
			}
			if (Input.GetKey(KeyCode.S)) {
				walkVector = transform.TransformVector(new Vector3(0, 0, -1) * walkSpeed * runSpeed);
				walkVector2 = new Vector2(walkVector.x, walkVector.z);
				//不自然に上に行かないようにする
				if (!canMoveToUp) {
					walkVector = new Vector3(walkVector.x, onGround.RayObjectHorizontalVector.y * walkVector2.magnitude / walkVector2.normalized.magnitude, walkVector.z);
				}
				rigid.velocity += walkVector;
				inputed = true;
			}
			if (Input.GetKey(KeyCode.D)) {
				walkVector = transform.TransformVector(new Vector3(1, 0, 0) * walkSpeed * runSpeed);
				walkVector2 = new Vector2(walkVector.x, walkVector.z);
				//不自然に上に行かないようにする
				if (!canMoveToUp) {
					walkVector = new Vector3(walkVector.x, onGround.RayObjectHorizontalVector.y * walkVector2.magnitude / walkVector2.normalized.magnitude, walkVector.z);
				}
				rigid.velocity += walkVector;
				inputed = true;
			}
		}

		//入力がなければ速度を徐々に減衰
		if (!inputed) {
			rigid.velocity = new Vector3(rigid.velocity.x * 0.95f, rigid.velocity.y, rigid.velocity.z * 0.95f);
			if (rigid.velocity.x <= 0.01f && rigid.velocity.z <= 0.01f) {
				rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			}

		}
	}

	/* カメラ操作 */
	private void CameraControll() {
		//カーソル表示
		if (Input.GetKey(KeyCode.Escape)) {
			Cursor.visible = !Cursor.visible;
			if (Cursor.visible)
				Cursor.lockState = CursorLockMode.None;
			else
				Cursor.lockState = CursorLockMode.Locked;
		}

		if (gameObject.transform.localEulerAngles.x >= 280 || gameObject.transform.localEulerAngles.x <= 80) {
			gameObject.transform.Rotate(-1f * Input.GetAxis("Mouse Y") * 3f, 0, 0);
		} else if (gameObject.transform.localEulerAngles.x < 180) {
			if (Input.GetAxis("Mouse Y") > 0) {     //マウス入力上
				gameObject.transform.Rotate(-1f * Input.GetAxis("Mouse Y") * 3f, 0, 0);     //マウス入力ｙ座標でローカルｘ座標回転
			}
		} else if (gameObject.transform.localEulerAngles.x > 180) {
			if (Input.GetAxis("Mouse Y") < 0) {     //マウス入力下
				gameObject.transform.Rotate(-1f * Input.GetAxis("Mouse Y") * 3f, 0, 0);     //マウス入力ｙ座標でローカルｘ座標回転
			}
		}
		gameObject.transform.Rotate(0, Input.GetAxis("Mouse X") * 3f, 0, Space.World);     //マウス入力x座標でワールドy座標回転
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "DropItem") {
			//落ちてるアイテムに接触したら拾う
			// ItemGet(other, other.gameObject.GetComponent<dropItem>().getItemNum());
		} else if (other.gameObject.tag == "DamegeArea") {
			//ダメージエリアに入ったらダメージを受ける
			HPDamegeManeger(-10);
			Debug.Log("dm");
		}
	}

	/* HP・スタミナの最大値を各バーに適用。アップデート関数の呼び出し */
	private void SetHPAndStaminaAndRedEyeOnFirst() {
		HPbar.GetComponent<Slider>().maxValue = HPMax;
		StaminaBar.GetComponent<Slider>().maxValue = StaminaMax;
		SetHPAndStaminaAndRedEyeOnUpdate();
	}

	/* HP・スタミナの現在値を各バーに適用。アップデート関数の呼び出し */
	private void SetHPAndStaminaAndRedEyeOnUpdate() {
		HPbar.GetComponent<Slider>().value = HPNow;
		StaminaBar.GetComponent<Slider>().value = StaminaNow;
		RedEye.GetComponent<RawImage>().color = new Color(
			RedEye.GetComponent<RawImage>().color.r,
			RedEye.GetComponent<RawImage>().color.g,
			RedEye.GetComponent<RawImage>().color.b,
			(float)( HPMax - HPNow ) / HPMax - 0.4f
		);
	}

	/* HPへのダメージを取りまとめる */
	public void HPDamegeManeger(int damege) {
		HPNow += damege;
		if (HPNow < 0)
			HPNow = 0;
	}

	/* スタミナへのダメージを取りまとめる */
	public void StaminaDamegeManeger(int damege) {
		StaminaNow += damege;
		if (StaminaNow < 0)
			StaminaNow = 0;
	}

	/* NowWeaponをぐるぐるさせる */
	private void itemChange() {
		//持ち物容量限界以下　または　持ち物の次が空じゃない
		if (NowWeapon + 1 < inventoryCapacity && inventoryList[NowWeapon + 1] != 0) {
			NowWeapon++;
		} else {
			NowWeapon = 0;
		}
	}

	/* 左上のアイテムの欄の表示 */
	private void weaponTextureChange() {
		int invKey;
		invKey = inventoryList[NowWeapon];
		weapon1Texture.GetComponent<RawImage>().texture = texture[invKey];
		if (NowWeapon + 1 < inventoryCapacity) {
			if (inventoryList[NowWeapon + 1] != 0)
				invKey = inventoryList[NowWeapon + 1];
			// else
				// invKey = NowWeapon + 1 - getEmptyinventoryNum() + 1;
		} else {
			invKey = inventoryList[NowWeapon + 1 - inventoryCapacity];
		}
		weapon2Texture.GetComponent<RawImage>().texture = texture[invKey];

		if (NowWeapon + 2 < inventoryCapacity) {
			if (inventoryList[NowWeapon + 2] != 0)
				invKey = inventoryList[NowWeapon + 2];
			// else
				// invKey = NowWeapon + 2 - getEmptyinventoryNum() + 1;
		} else {
			invKey = inventoryList[NowWeapon + 2 - inventoryCapacity];
		}
		weapon3Texture.GetComponent<RawImage>().texture = texture[invKey];
	}
}
