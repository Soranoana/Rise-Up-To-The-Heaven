using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*playerControllから武器情報をintで取得
 * VRか否か確認をplayerControllerにしてもらう
 * 以下nonVR
 * 武器情報から攻撃アニメーションを取得
 * パーティクル、弾、当たり判定などを用意
 * playerControllからの攻撃命令により攻撃開始
 * 弾の発生、射出、当たり判定、パーティクルを発生
 * 攻撃アニメーションなどを再生
 * 
 * 以下VR
 * 近接武器の攻撃の当たり判定は出しっぱなし
 * 一部武器は攻撃命令よりアニメーション実行
 * 弾の射出、など
 * 
 *
 */
public class controllWeaponOnHands : MonoBehaviour {

	public int ItemKindNum;
	private int weaponNum = 1;
	private int weaponNumWas = 0;
	private bool changeWeapon = true;
	private GameObject[] weaponObject;
	private GameObject nowWeapon;
	private bool isVR = false;
	//アニメーション
	public AnimationClip[] rightHandAnim1;
	public AnimationClip[] rightHandAnim2;
	private new Animation animation;
	//当たり判定
	private Collider[] colliderArray;

	private void Awake() {
		weaponObject = new GameObject[ItemKindNum];
		colliderArray = new Collider[ItemKindNum];
	}

	void Start() {
		//アニメーションの格納
		animation = transform.parent.GetComponent<Animation>();
		for (int i = 0; i < rightHandAnim1.Length; i++) {
			if (rightHandAnim1[i] != null)
				animation.AddClip(rightHandAnim1[i], rightHandAnim1[i].name);
		}
		for (int i = 0; i < rightHandAnim2.Length; i++) {
			if (rightHandAnim1[i] != null)
				animation.AddClip(rightHandAnim2[i], rightHandAnim2[i].name);
		}
		//コライダーの格納 オブジェクトの非表示化
		setWeaponObject();
	}

	void Update() {
		//逆再生
		/*if (animation.IsPlaying(rightHandAnim2[weaponNum].name)) {
            animation[rightHandAnim2[weaponNum].name].time = animation[rightHandAnim2[weaponNum].name].time-Time.deltaTime*2;
            if (animation[rightHandAnim2[weaponNum].name].time <= 0) {
                animation.Stop(rightHandAnim2[weaponNum].name);
            }
        }*/
		if (changeWeapon) {
			//古い武器の処理
			colliderArray[weaponNumWas].enabled = false;
			nowWeapon.SetActive(false);

			//新しい武器の処理
			nowWeapon = weaponObject[weaponNum];
			nowWeapon.SetActive(true);
			setWeaponTransform(nowWeapon, weaponNum);
			//controllCollider();
			changeWeapon = false;
		}
		if (!animation.isPlaying && colliderArray[weaponNum].enabled) {
			//当たり判定を消す
			colliderArray[weaponNum].enabled = false;
		}
	}

	public void weaponNumUpdate(int Num) {
		weaponNumWas = weaponNum;
		weaponNum = Num;
		changeWeapon = true;
	}

	public void useWeapon(int attackKind) {
		if (!animation.isPlaying) {
			if (attackKind == 1) {
				animation.Play(rightHandAnim1[weaponNum].name);
			} else if (attackKind == 2) {
				animation.Play(rightHandAnim2[weaponNum].name);
				//逆さ再生準備
				/*animation[rightHandAnim2[weaponNum].name].time = animation[rightHandAnim2[weaponNum].name].clip.length;
                float tmp = Time.deltaTime;*/
			} else {
				Debug.Log("empty attack number");
				return;
			}
			//当たり判定を表示
			colliderArray[weaponNum].enabled = true;
		}
	}

	public void useWeaponNagaoshi(int attackKind) {
		//長押し攻撃処理
	}

	public void IsVR(bool vr) {
		isVR = vr;
	}

	private void setWeaponTransform(GameObject weapon, int weaponNum) {
		if (weaponNum == 1) {
			weapon.transform.localPosition = new Vector3(0.01125f, -0.00067f, 0.00044f);
			weapon.transform.localEulerAngles = new Vector3(-40, 0, 176);
			weapon.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			transform.localPosition = new Vector3(0, 0, 0);
			transform.localEulerAngles = new Vector3(-90, 0, -63);
		}
	}

	//weaponObjectの初期化
	private void setWeaponObject() {
		weaponObject[0] = this.transform.Find("punch").gameObject;
		weaponObject[1] = this.transform.Find("Sickle").gameObject;

		//コライダーも取得
		setColliderArray();

		//表示を消す
		for (int i = 0; i < weaponObject.Length; i++) {
			if (weaponObject[i] != null)
				weaponObject[i].SetActive(false);
		}
		//今の武器だけ表示する
		nowWeapon = weaponObject[weaponNum];
		nowWeapon.SetActive(true);

		return;
	}
	//colliderArrayの初期化
	private void setColliderArray() {
		colliderArray[0] = weaponObject[0].transform.Find("Collider").GetComponent<BoxCollider>();
		colliderArray[1] = weaponObject[1].transform.Find("blade").transform.Find("Collider").GetComponent<BoxCollider>();

		//コライダーの表示を消す
		for (int i = 0; i < colliderArray.Length; i++) {
			if (colliderArray[i] != null)
				colliderArray[i].enabled = false;
		}

		return;
	}


	/* 以下、それぞれの武器についての個別挙動 */
}
