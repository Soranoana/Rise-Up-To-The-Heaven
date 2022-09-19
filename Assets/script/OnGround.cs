// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class OnGround : MonoBehaviour {

	// rayPositionObjsの数
	private const int rayPositionObjsNum = 5;

	// レイを飛ばす位置
	[SerializeField]
	private GameObject[] rayPositionObjs = new GameObject[rayPositionObjsNum];
	// レイの距離
	[SerializeField]
	private float rayRange = 1f;
	private Vector3 rayObjectHorizontalVector;
	// int layerMask = 1 << 12 | 1 << 16;

	//地面接地判定を適用する
	public bool useGrounding;

	private bool isGround;

	void Start() {
		//初期化
		isGround = true;

		//rayPositionObjsがすべてセットされているか確認する
		for (int i = 0; i < rayPositionObjsNum; i++) {
			if (rayPositionObjs[i] == null) {
				Debug.LogError("rayPositionObjsがnullです。: rayPositionObjs[" + i + "]");
				//TODO warningにして、自動で初期化する。
			}
		}
		// rayPositionObjs = GameObject.FindGameObjectsWithTag("PlayerFootRayPosition");
	}

	void Update() {
		//ステータス更新
		UpdateGroundStatus();

		Debug.Log("IsGround : " + IsGround());
	}

	//接地しているか否かを更新する
	private void UpdateGroundStatus() {
		//処理を行うか確認
		if (!useGrounding) {
			return;
		}

		//RayCast用
		//あたったものの情報が入るRaycastHitを準備
		// RaycastHit hit;
		//新しいレイを作成する（発射位置と方向をあらわす情報）
		Ray[] ray = new Ray[8];

		for (int i = 0; i < rayPositionObjsNum; i++) {
			//TODO newを最初だけにして、変数の更新だけにしたい。
			//レイオブジェクトの設定と、発射位置の初期化。発射位置はレイのポジションの - (0,rayRange,0)
			ray[i] = new Ray(rayPositionObjs[i].transform.position, rayPositionObjs[i].transform.position - transform.up * rayRange);

			if (Physics.Linecast(ray[i].origin, ray[i].direction, out RaycastHit hit/*, layerMask*/)) {
				//接地面のベクトル方向を取得
				LocalGround(ray[i], hit);
				//接地している
				isGround = true;

				return;
			}
		}
		isGround = false;
		return;
	}

	//坂道用　移動方向のベクトル平面の取得
	private void LocalGround(Ray ray, RaycastHit hit) {
		//  normal; //レイがあたった当たり判定オブジェクトの面の法線
		//  reflectDirection;  //反射ベクトル（反射方向を示すベクトル）
		//  direction;  //レイの方向ベクトル

		//レイがあたった当たり判定オブジェクトの面の法線
		Vector3 normal = hit.normal;
		// normal = new Vector3(Mathf.Abs(hit.normal.x), hit.normal.y, Mathf.Abs(hit.normal.z));

		//レイの方向ベクトル
		Vector3 direction = ray.direction;

		//反射ベクトル（反射方向を示すベクトル）
		// Vector3 reflectDirection = 2 * normal * Vector3.Dot(normal, -direction) + direction;
		Vector3 reflectDirection = -1 * normal * Vector3.Dot(normal, -direction) + direction * 2;

		// rayObjectHorizontalVector = -( direction + reflectDirection ) / 2f;
		rayObjectHorizontalVector = reflectDirection;
		return;
	}

	public Vector3 RayObjectHorizontalVector() {
		return rayObjectHorizontalVector;
	}

	public bool IsGround() {
		return isGround;
	}
}
