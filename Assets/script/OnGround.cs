using UnityEngine;

public class OnGround : MonoBehaviour {

	//定数
	//rayPositionObjsの数
	private const int rayPositionObjsNum = 5;

	//Inspectorからセットする変数
	//レイを飛ばす位置
	[SerializeField]
	private GameObject[] rayPositionObjs = new GameObject[rayPositionObjsNum];
	// レイの距離
	[SerializeField]
	private readonly float rayRange = 1f;
	//地面接地判定を適用する
	[SerializeField]
	public bool UseGrounding { get; private set; }

	//外部参照可能な変数
	//反射ベクトル
	public Vector3 RayObjectHorizontalVector { get; private set; }
	//接地判定結果
	public bool IsGround { get; private set; } = true;

	void Start() {
		//rayPositionObjsがすべてセットされているか確認する
		for (int i = 0; i < rayPositionObjsNum; i++) {
			if (rayPositionObjs[i] == null) {
				Debug.LogError("rayPositionObjsがnullです。: rayPositionObjs[" + i + "]");
				//TODO warningにして、自動で初期化する。
			}
		}
	}

	void Update() {
		//ステータス更新
		UpdateGroundStatus();

		Debug.Log("IsGround : " + IsGround);
	}

	//接地しているか否かの状態を更新する
	private void UpdateGroundStatus() {
		//処理を行うか確認
		if (!UseGrounding) {
			return;
		}

		//レイを作成する（発射位置と方向をあらわす情報）
		Ray[] ray = new Ray[8];

		//レイを飛ばして接地判定を実施。一つでもレイが当たれば接地していることにする
		for (int i = 0; i < rayPositionObjsNum; i++) {
			//レイオブジェクトの設定と、発射位置の初期化。発射位置はレイのポジションの - (0,rayRange,0)
			ray[i] = new Ray(rayPositionObjs[i].transform.position, rayPositionObjs[i].transform.position - transform.up * rayRange);

			if (Physics.Linecast(ray[i].origin, ray[i].direction, out RaycastHit hit)) {
				//接地面のベクトル方向を取得
				LocalGround(ray[i], hit);
				//接地している
				IsGround = true;

				return;
			}
		}
		//接地していない
		IsGround = false;
		return;
	}

	//坂道用　移動方向のベクトル平面の取得
	private void LocalGround(Ray ray, RaycastHit hit) {
		//レイがあたった当たり判定オブジェクトの面の法線
		Vector3 normal = hit.normal;
		//レイの方向ベクトル
		Vector3 direction = ray.direction;

		//反射ベクトル（反射方向を示すベクトル）
		RayObjectHorizontalVector = -1 * normal * Vector3.Dot(normal, -direction) + direction * 2;

		return;
	}
}
