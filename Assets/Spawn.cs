using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour {
	public GameObject Enemy0;    //敵オブジェクト
	public GameObject Enemy1;    //敵オブジェクト
	public GameObject Enemy2;    //敵オブジェクト
	public GameObject Enemy3;    //敵オブジェクト
	public GameObject Enemy4;    //敵オブジェクト

	public GameObject Player;   //プレイヤーオブジェクト

	public static GameObject[] EnemyObjects;
	private int EnemyCount;	//敵オブジェクト数

	public static GameObject[] BossObjects;
	private int BossCount; //ボスオブジェクト数

	private bool Zako0;				//雑魚0 煉獄 	出現用bool
	private bool Zako1;				//雑魚1 寺　 	出現用bool
	private bool Zako2;				//雑魚3 廊下	出現用bool
	private bool Zako3;				//雑魚4 海	　	出現用bool
	private bool Zako4;				//雑魚5 塔		出現用bool

	private int r;					//雑魚　ランダム出現用int
	public Vector3 pos1;				//雑魚　出現座標用Vector3
	public Vector3 pos2;
	public Vector3 pos3;				//雑魚　出現座標用Vector3
	public Vector3 pos4;
	public Vector3 pos5;				//雑魚　出現座標用Vector3

	private int i0;
	private int i1;
	private int i2;
	private int i3;
	private int i4;
	private int i5;
	private int i6;

//	private Quaternion I;			//雑魚出現角度
	public Text t;					//アナウンス

	private float BossBirth;			//ボス出現用floot

	public GameObject BossKeru;		//ボス0　ケルベロス
	public GameObject BossSyaka;	//ボス1　釈迦　
	public GameObject Bossdex;		//ボス2　デュラハン　
	public GameObject Bossika;		//ボス3　イカ
	public GameObject Bossdragon;	//ボス4　ドラゴン　

	public static bool Boss0;		//ボス0　ケルベロス　出現用bool
	public static bool Boss1;		//ボス1　釈迦　出現用bool
	public static bool Boss2;		//ボス2　デュラハン　出現用bool
	public static bool Boss3;		//ボス3　烏賊　出現用bool
	public static bool Boss4;		//ボス4　ドラゴン　出現用bool

	public static bool Stage0Clear;		//ステージ0 煉獄　	突破用bool関数B
	public static bool Stage1Clear;		//ステージ1 寺　	突破用bool関数B
	public static bool Stage2Clear;		//ステージ2 城	　	突破用bool関数
	public static bool Stage3Clear;		//ステージ3 廊下　	突破用bool関数
	public static bool Stage4Clear;		//ステージ4 闘技場　突破用bool関数B
	public static bool Stage5Clear;		//ステージ5 海　	突破用bool関数B
	public static bool Stage6Clear;		//ステージ6 塔　	突破用bool関数B

	private Slider BHPBar;			//ボス　体力用Slider

	private GameObject[] F0;			//ステージ0 煉獄
	private GameObject[] F1;			//ステージ1 寺　
	private GameObject[] F2;			//ステージ3 廊下　
	private GameObject[] F3;			//ステージ5 海
	private GameObject[] F4;            //ステージ6 塔


	/*各ステージ用スポーン地点数カウント*/
	private int F0C;
	private int F1C;
	private int F2C;
	private int F3C;
	private int F4C;

	/*スポーン済か否かの確認配列*/
	private int[] F0b = new int[100];
	private int[] F1b= new int[100];
	private int[] F2b= new int[100];
	private int[] F3b= new int[100];
	private int[] F4b= new int[100];

	public GameObject[] WarpPoint;

	void Start () {
		/*スポーン確認配列初期化*/
		for (int i = 0; i < 100; i++) {
			F0b [i] = 0;
			F1b [i] = 0;
			F2b [i] = 0;
			F3b [i] = 0;
			F4b [i] = 0;
		}

		i0 = 0;
		i1 = 0;
		i2 = 0;
		i3 = 0;
		i4 = 0;

		EnemyObjects = GameObject.FindGameObjectsWithTag ("Enemy");
		EnemyCount = EnemyObjects.Length;
		BossObjects = GameObject.FindGameObjectsWithTag ("Boss");
		BossCount = BossObjects.Length;

		Zako0 = false;
		Zako1 = false;
		Zako2 = false;
		Zako3 = false;
		Zako4 = false;

//		I = new Quaternion(-45f,0,0,1);

		Boss0 = false;
		Boss1 = false;
		Boss2 = false;
		Boss3 = false;
		Boss4 = false;

		Stage0Clear = false;
		Stage1Clear = false;
		Stage2Clear = false;
		Stage3Clear = false;
		Stage4Clear = false;
		Stage5Clear = false;
		Stage6Clear = false;

		BHPBar=GameObject.Find("BHPBar").GetComponent<Slider>();

		F0 = GameObject.FindGameObjectsWithTag ("Floor0");
		F0C = F0.Length;
		F1 = GameObject.FindGameObjectsWithTag ("Floor1");
		F1C = F1.Length;
		F2 = GameObject.FindGameObjectsWithTag ("Floor2");
		F2C = F2.Length;
		F3 = GameObject.FindGameObjectsWithTag ("Floor3");
		F3C = F3.Length;
		F4 = GameObject.FindGameObjectsWithTag ("Floor4");
		F4C = F4.Length;

		for (int i = 0; i < 100; i++) {
			F0b [i] = 0;
			F1b [i] = 0;
			F2b [i] = 0;
			F3b [i] = 0;
			F4b [i] = 0;
		}
	}

	void Update ()
	{
		//------------------------------初期設定-------------------------------//
		EnemyObjects = GameObject.FindGameObjectsWithTag ("Enemy");
		EnemyCount = EnemyObjects.Length;
		BossObjects = GameObject.FindGameObjectsWithTag ("Boss");
		BossCount = BossObjects.Length;

		if (EnemyCount != 0) {
			t.text = ("あと  " + EnemyCount.ToString () + "体でBOSS出現!!");
		} else if (BossCount != 0) {
			t.text = "";
		} else {
			t.text ="Error : BossCount = 0 && EnemyCount = 0";		//デバッグ用
		}

		//------------------------------ボス撃破判定---a-------------------//
	

		//--------------------------エネミー出現場所の指定-------------------------------//
		if (!Zako0) {
			while (i0 < F0C) {
				r = Random.Range (0, F0C);
				if (F0b [r] == 0)
					break;
			}
			if (i0 < F0C) {
				i0++;
			}
			F0b [r] = 1;
		
		} else if (Stage0Clear && !Zako1) {
			while (i1 < F1C) {
				r = Random.Range (0, F1C);
				if (F1b [r] == 0)
					break;
			}

			if (i1 < F1C) {
				i1++;
			}
			F1b [r] = 1;

		}else if (Stage2Clear && !Zako2) {
			while (i2 < F2C) {
				r = Random.Range (0, F2C);
				if (F2b [r] == 0)
					break;
			}

			if (i2 < F2C) {
				i2++;
			}
			F2b [r] = 1;

		} else if (Stage4Clear && !Zako3) {
			while (i3 < F3C) {
				r = Random.Range (0, F3C);
				if (F3b [r] == 0)
					break;
			}

			if (i3 < F3C) {
				i3++;
			}
			F3b [r] = 1;

		} else if (Stage5Clear && !Zako4) {
			while (i4 < F4C) {
				r = Random.Range (0, F4C);
				if (F4b [r] == 0)
					break;
			}

			if (i4 < F4C) {
				i4++;
			}
			F4b [r] = 1;
		}
	
		//------------------------------雑魚出現-------------------------------//
			if (!Zako0 && EnemyCount < 7) {			//雑魚一種目出現
				pos1 = F0[i0-1].transform.position;
				GameObject.Instantiate (Enemy0, pos1, Enemy0.transform.rotation);
				if (EnemyCount  == 6) {
					Zako0 = true;
				}
			} else if (Stage0Clear && !Zako1 && EnemyCount < F1C) {				//雑魚2種目出現
				Boss1 = false;
				pos2 = F1[i1-1].transform.position;
				GameObject.Instantiate (Enemy1, pos2, Enemy1.transform.rotation);
				if (EnemyCount  <= F1C) {
					Zako1 = true;
				}
			} else if (Stage2Clear && !Zako2 && EnemyCount < F2C) {			//雑魚3種目出現
				pos3 = F2[i2-1].transform.position;
				GameObject.Instantiate (Enemy2, pos3, Enemy2.transform.rotation);
				if (EnemyCount <= F2C) {
					Zako2 = true;
				}
			} else if (Stage4Clear && !Zako3 && EnemyCount < F3C) {				//雑魚4種目出現
				pos4 = F3[i3-1].transform.position;
				GameObject.Instantiate (Enemy3, pos4, Enemy3.transform.rotation);
				if (EnemyCount <= F3C) {
					Zako3 = true;
				}
			} else if (Stage5Clear && !Zako4 && EnemyCount < F4C) {				//雑魚5種目出現
				pos5 = F4[i4-1].transform.position;
				GameObject.Instantiate (Enemy4, pos5, Enemy4.transform.rotation);
				if (EnemyCount <= F4C) {
					Zako4 = true;
				}
			}

			//------------------------------雑魚全滅→ボス出現----------------------//

			if (EnemyCount == 0) {
			
				if (Zako0 && !Boss0) {
					Instantiate (BossKeru, new Vector3 (-665.061f, 71.23256f, -432f), new Quaternion (0, 0, 0, 0));
					Boss0 = true;
				} else if (Zako1 && !Boss1) {
					Instantiate (BossSyaka, new Vector3 (-54, -4, 116), new Quaternion (0, 180, 0, 0));
					Boss1 = true;
				} else if (Zako2 && !Boss2) {
					Instantiate (Bossdex, new Vector3 (975f,-26f,-1329f), new Quaternion (0, 0, 0, 0));
					Boss2 = true;
				} else if (Zako3 && !Boss3) {
					Instantiate (Bossika, new Vector3 (2707f,-16.34f,-928.9f), new Quaternion (0, 0, 0, 0));
					Boss3 = true;
				} else if (Zako4 && !Boss4) {
					Instantiate (Bossdragon, new Vector3 (4920,619,0.25f), new Quaternion (0, 0, 0, 0));
					Boss4 = true;
				}
				
				//------------------------------ボスＨＰ設定----------------------//


			if (EnemyCount == 0 && BossCount != 0) {
				BHPBar.enabled = true;
				if (!Stage0Clear) {
					BHPBar.value = BossKerube.HP;
				} else if (!Stage1Clear) {
					BHPBar.value = BossShaka.HP;				
				} else if (!Stage4Clear) {
					BHPBar.value = BossIkacs.HP;
				} else if (!Stage5Clear) {
					BHPBar.value = BossDexra.HP;
				} else if (!Stage6Clear) {
					BHPBar.value = BossDragon.HP;
				}
			} 
				//------------------------------エンディング----------------------//

				
			}
		}
	}
