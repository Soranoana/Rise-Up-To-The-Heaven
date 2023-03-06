using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* プレイヤーに関連するアイテムについての処理を行う。具体的には以下の処理についてになる。 
 * ・取得物管理
 * ・インベントリ管理
 * ・アイテム使用管理
 */
public class playerItem : MonoBehaviour {

	private int itemKindNum;
	private int inventoryCapacity;       //インベントリのキャパシティ
	private int[] inventoryList;

	void Start() {

	}

	void Update() {

	}

	private void Awake() {
		// 		/* プラットホーム依存コンパイル */
		// #if UNITY_EDITOR
		// 		filePath = Directory.GetCurrentDirectory() + "/Assets/Resources/itemlist.txt";
		// #endif
		// #if UNITY_STANDALONE_WIN
		// 		filePath = Directory.GetCurrentDirectory() + "/Assets/Resources/itemlist.txt";
		// #endif
		// #if UNITY_ANDROID
		//         filePath = Directory.GetCurrentDirectory() + "/Assets/Resources/itemlist.txt";
		// #endif
		/* プラットホーム依存コンパイル ここまで*/
		itemKindNum = 18;

		inventoryCapacity = itemKindNum;
		inventoryList = new int[inventoryCapacity];
	}

	// /* アイテムの一覧をtxtファイルから作成 */
	// private void itemSetOnStart() {
	// 	sr = new StreamReader(filePath);
	// 	// InventorySet = new ItemAndWeaponSet[itemKindNum];
	// 	string[] text = sr.ReadLine().Split('\t');
	// 	//Debug.Log(text[0]);
	// 	for (int i = 0; i < itemKindNum; i++) {
	// 		text = sr.ReadLine().Split('\t');
	// 		//Debug.Log(text[0]);
	// 		// InventorySet[i] = new ItemAndWeaponSet() {
	// 		// ItemNumber = int.Parse(text[0]),
	// 		// ItemName = text[1],
	// 		// AttackPoint = int.Parse(text[2]),
	// 		// UseLimit = int.Parse(text[3]),
	// 		// ItemTips = text[4],
	// 		// ItemTypes = text[5]
	// 		// };
	// 	}
	// }

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

	/* 衝突したアイテムを拾う */
	private void ItemGet(Collider other, int ItemNumber) {
		int tmp = getEmptyinventoryNum();
		tmp = getEmptyinventoryNum();
		if (tmp < 0)
			return;//ToDo 持てません処理実装
		else
			other.gameObject.GetComponent<dropItem>().settingDestroySelf(0);
		inventoryList[tmp] = ItemNumber;
	}
}
