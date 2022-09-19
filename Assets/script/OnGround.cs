using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : MonoBehaviour{

    // rayPositionObjsの数
    private readonly int rayPositionObjsNum = 5;

    // レイを飛ばす位置
    [SerializeField]
    private GameObject[] rayPositionObjs = new GameObject[rayPositionObjsNum];
    // レイの距離
    [SerializeField]
    private float rayRange = 1f;
    //RayCast用
    //あたったものの情報が入るRaycastHitを準備
    RaycastHit hit;
    //新しいレイを作成する（発射位置と方向をあらわす情報）
    Ray[] ray = new Ray[8];
    Vector3 rayObjectHorizontalVector;
    int layerMask = 1 << 12 | 1 << 16;
    //反射ベクトル（反射方向を示すベクトル）
    Vector3 reflect_direction;
    //レイがあたった当たり判定オブジェクトの面の法線
    Vector3 normal;
    //レイの方向ベクトル
    Vector3 direction;

    //地面接地判定を適用する
    public bool useGrounding;

    private bool isGround;

    void Start(){
        for(int i = 0;i<rayPositionObjsNum;i++){
            if(rayPositionObjs[i]==null){
                Debug.LogError("rayPositionObjsの数が不正です。");
            }
        }
        isGround=true;
        // rayPositionObjs = GameObject.FindGameObjectsWithTag("PlayerFootRayPosition");
    }

    void Update(){
        IsGround();
    }

    //接地しているか否かを更新する
    private void UpdateGroundStatus() {
        if (!useGrounding){
            return;
        }

        for (int i = 0; i < rayPositionObjsNum; i++) {
            ray[i] = new Ray(rayPositionObjs[i].transform.position, rayPositionObjs[i].transform.position - transform.up * rayRange);
            if (Physics.Linecast(rayPositionObjs[i].transform.position, rayPositionObjs[i].transform.position - transform.up * rayRange, out hit, layerMask)) {
                rayObjectHorizontalVector = localGround(ray[i]);
                isGround = true;
                return;
            }
        }
        isGround=false;
        return;
    }

    public bool IsGround(){
        return isGround;
    }
}
