using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public struct ItemAndWeaponSet {
    public int ItemNumber;     //アイテムの番号（検索用など）
    public string ItemName;    //アイテムの名前
    public int AttackPoint;    //攻撃力
    public int UseLimit;       //残り使用回数（999で∞扱い）
    public string ItemTips;    //アイテムの効果説明（実装するかは未定）
    public string ItemTypes;      //アイテムの種類
                                  /* アイテム番号一覧
                                   * 0 未所持
                                   * 1 死神の鎌
                                   * 2 ちわベロスの槍
                                   * 3 釈迦の後光
                                   * 4 デュラハンの聖剣
                                   * 5 クラーケンの触手
                                   * 6 ドラゴンのバーナー
                                   *
                                   * 以下隠し武器（使うたびに威力減少）
                                   * 7 死神の化身
                                   * 8 ケルベロスのつがい
                                   * 9 釈迦の彫像
                                   * 10 デュラハンの甲冑
                                   * 11 クラーケンの子供
                                   * 12 ドラゴンの雛
                                   *
                                   * 以下アイテム
                                   * 13 薬品        ポーション（少量回復、使用でなくなる）
                                   * 14 強力な薬品  エクスポーション（大）
                                   * 15 彷徨う魂の結晶    復活（回復半分）
                                   * 16 那由多の断末魔   無限完全回復+復活+難易度上昇（即上昇、初期装備）
                                   * 17 夢結晶    一時無敵化スキルポイント（クリア時所持でポイント付与）
                                   * 18 デバッグ用アイテム
                                   */
                                  /* アイテムの種類
                                   * 隠し武器
                                   * 武器
                                   * 回復
                                   */
};

public class playerControll : MonoBehaviour {

    private float walkSpeed = 10;
    private Vector3 walkVector = Vector3.zero;
    private Vector2 walkVector2 = Vector2.zero;
    private bool inputed = false;
    private Rigidbody rigid;
    public float GravityForce;
    public float runSpeed;
    //IsGround用
    //　レイを飛ばす位置
    [SerializeField]
    private GameObject[] rayPosition = new GameObject[8];
    //　レイの距離
    [SerializeField]
    private float rayRange = 1f;
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
    public ItemAndWeaponSet[] InventorySet;
    public string filePath;
    private StreamReader sr;
    private int itemKindNum;
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
    //地面接地判定を適用する
    public bool useGrounding;
    //上へ移動を可能する
    public bool canMoveToUp;


    private void Awake() {
        /* プラットホーム依存コンパイル */
#if UNITY_EDITOR
        filePath = Directory.GetCurrentDirectory() + "/Assets/Resources/itemlist.txt";
#endif
#if UNITY_STANDALONE_WIN
        filePath = Directory.GetCurrentDirectory() + "/Assets/Resources/itemlist.txt";
#endif
#if UNITY_ANDROID
        filePath = Directory.GetCurrentDirectory() + "/Assets/Resources/itemlist.txt";
#endif
        /* プラットホーム依存コンパイル ここまで*/
        itemKindNum = 18;
        itemSetOnStart();

        inventoryCapacity = itemKindNum;
        inventoryList = new int[inventoryCapacity];

        rightHand = transform.Find("handRight").gameObject;
        rightHandScript = rightHand.GetComponent<controllWeaponOnHands>();
    }

    void Start() {
        rigid = this.gameObject.GetComponent<Rigidbody>();
        GravityForce = 98f;
        rayPosition = GameObject.FindGameObjectsWithTag("PlayerFootRayPosition");
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
        if (IsGround()) {
            rigid.velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) {
                walkVector = transform.TransformVector(new Vector3(0, 0, 1) * walkSpeed * runSpeed);
                walkVector2 = new Vector2(walkVector.x, walkVector.z);
                //不自然に上に行かないようにする
                if (!canMoveToUp) {
                    walkVector = new Vector3(walkVector.x, rayObjectHorizontalVector.y * walkVector2.magnitude / walkVector2.normalized.magnitude, walkVector.z);
                }
                rigid.velocity += walkVector;
                inputed = true;
            }
            if (Input.GetKey(KeyCode.A)) {
                walkVector = transform.TransformVector(new Vector3(-1, 0, 0) * walkSpeed * runSpeed);
                walkVector2 = new Vector2(walkVector.x, walkVector.z);
                //不自然に上に行かないようにする
                if (!canMoveToUp) {
                    walkVector = new Vector3(walkVector.x, rayObjectHorizontalVector.y * walkVector2.magnitude / walkVector2.normalized.magnitude, walkVector.z);
                }
                rigid.velocity += walkVector;
                inputed = true;
            }
            if (Input.GetKey(KeyCode.S)) {
                walkVector = transform.TransformVector(new Vector3(0, 0, -1) * walkSpeed * runSpeed);
                walkVector2 = new Vector2(walkVector.x, walkVector.z);
                //不自然に上に行かないようにする
                if (!canMoveToUp) {
                    walkVector = new Vector3(walkVector.x, rayObjectHorizontalVector.y * walkVector2.magnitude / walkVector2.normalized.magnitude, walkVector.z);
                }
                rigid.velocity += walkVector;
                inputed = true;
            }
            if (Input.GetKey(KeyCode.D)) {
                walkVector = transform.TransformVector(new Vector3(1, 0, 0) * walkSpeed * runSpeed);
                walkVector2 = new Vector2(walkVector.x, walkVector.z);
                //不自然に上に行かないようにする
                if (!canMoveToUp) {
                    walkVector = new Vector3(walkVector.x, rayObjectHorizontalVector.y * walkVector2.magnitude / walkVector2.normalized.magnitude, walkVector.z);
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

    //接地しているか否か
    private bool IsGround() {
        if (!useGrounding)
            return true;

        for (int i = 0; rayPosition.Length > i; i++) {
            ray[i] = new Ray(rayPosition[i].transform.position, rayPosition[i].transform.position - transform.up * rayRange);
            if (Physics.Linecast(rayPosition[i].transform.position, rayPosition[i].transform.position - transform.up * rayRange, out hit, layerMask)) {
                rayObjectHorizontalVector = localGround(ray[i]);
                return true;
            }
        }
        return false;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "DropItem") {
            ItemGet(other, other.gameObject.GetComponent<dropItem>().getItemNum());
        } else if (other.gameObject.tag == "DamegeArea") {
            HPDamegeManeger(-10);
            Debug.Log("dm");
        }
    }

    private void ItemGet(Collider other, int ItemNumber) {
        int tmp = getEmptyinventoryNum();
        tmp = getEmptyinventoryNum();
        if (tmp < 0)
            return;//持てません処理未実装
        else
            other.gameObject.GetComponent<dropItem>().settingDestroySelf(0);
        inventoryList[tmp] = ItemNumber;
    }

    private void SetHPAndStaminaAndRedEyeOnFirst() {
        HPbar.GetComponent<Slider>().maxValue = HPMax;
        StaminaBar.GetComponent<Slider>().maxValue = StaminaMax;
        SetHPAndStaminaAndRedEyeOnUpdate();
    }
    private void SetHPAndStaminaAndRedEyeOnUpdate() {
        HPbar.GetComponent<Slider>().value = HPNow;
        StaminaBar.GetComponent<Slider>().value = StaminaNow;
        RedEye.GetComponent<RawImage>().color = new Color(RedEye.GetComponent<RawImage>().color.r,
                                                        RedEye.GetComponent<RawImage>().color.g,
                                                        RedEye.GetComponent<RawImage>().color.b,
                                                        (float)( HPMax - HPNow ) / HPMax - 0.4f);
    }

    //坂道用　移動方向のベクトル平面の取得
    private Vector3 localGround(Ray rayOnce) {
        //レイがあたった当たり判定オブジェクトの面の法線
        normal = hit.normal;
        normal = new Vector3(Mathf.Abs(normal.x), normal.y, Mathf.Abs(normal.z));

        //レイの方向ベクトル
        direction = rayOnce.direction;

        //反射ベクトル（反射方向を示すベクトル）
        reflect_direction = 2 * normal * Vector3.Dot(normal, -direction) + direction;

        rayObjectHorizontalVector = -( direction + reflect_direction ) / 2f;
        return rayObjectHorizontalVector;
    }

    public void HPDamegeManeger(int damege) {
        HPNow += damege;
        if (HPNow < 0)
            HPNow = 0;
    }

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

    /* アイテムの一覧をtxtファイルから作成 */
    private void itemSetOnStart() {
        sr = new StreamReader(filePath);
        InventorySet = new ItemAndWeaponSet[itemKindNum];
        string[] text = sr.ReadLine().Split('\t');
        //Debug.Log(text[0]);
        for (int i = 0; i < itemKindNum; i++) {
            text = sr.ReadLine().Split('\t');
            //Debug.Log(text[0]);
            InventorySet[i] = new ItemAndWeaponSet() {
                ItemNumber = int.Parse(text[0]),
                ItemName = text[1],
                AttackPoint = int.Parse(text[2]),
                UseLimit = int.Parse(text[3]),
                ItemTips = text[4],
                ItemTypes = text[5]
            };
        }
    }

    /* アイテムリストの空き番号を調べる */
    private int getEmptyinventoryNum() {
        //中身が0(未所持)ならそれを返す
        for (int i = 0; i < inventoryCapacity; i++) {
            if (inventoryList[i] == 0)
                return i;
        }
        //リストに空きがないなら-1を返す
        return -1;
    }

    /* 左上のアイテムの欄の表示 */
    private void weaponTextureChange() {
        int invKey;
        invKey = inventoryList[NowWeapon];
        weapon1Texture.GetComponent<RawImage>().texture = texture[invKey];
        if (NowWeapon + 1 < inventoryCapacity) {
            if (inventoryList[NowWeapon + 1] != 0)
                invKey = inventoryList[NowWeapon + 1];
            else
                invKey = NowWeapon + 1 - getEmptyinventoryNum() + 1;
        } else {
            invKey = inventoryList[NowWeapon + 1 - inventoryCapacity];
        }
        weapon2Texture.GetComponent<RawImage>().texture = texture[invKey];

        if (NowWeapon + 2 < inventoryCapacity) {
            if (inventoryList[NowWeapon + 2] != 0)
                invKey = inventoryList[NowWeapon + 2];
            else
                invKey = NowWeapon + 2 - getEmptyinventoryNum() + 1;
        } else {
            invKey = inventoryList[NowWeapon + 2 - inventoryCapacity];
        }
        weapon3Texture.GetComponent<RawImage>().texture = texture[invKey];
    }

    /* インベントリの中身の途中の空きを削除 */
    private void inventorySort() {
        for (int i = 0; i < inventoryCapacity; i++) {
            if (inventoryList[i] == 0) {
                bool isListEnd = true;
                for (int j = i + 1; j < inventoryCapacity; j++) {
                    if (inventoryList[j] != 0) {
                        inventoryList[i] = inventoryList[j];
                        inventoryList[j] = 0;
                        j = inventoryCapacity;
                        isListEnd = false;
                    }
                }
                if (isListEnd) {
                    return;
                }
            }
        }
        return;
    }
}
/*
 * 非VRなら左手消す


 
     
     */
