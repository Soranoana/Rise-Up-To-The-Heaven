using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/* 擬似的なDBの作成を行うプログラム
 * DBのレコードに相当する
 * カラムの追加は対象のオブジェクト用の変数の定義を行うだけ
 */

[Serializable]
//projectビューの 右クリック＞Create に本オブジェクトの追加ボタンを配置
[CreateAssetMenu(fileName = "Item", menuName = "CreateItem")]
public class Item : ScriptableObject {

	//アイテムID
	public int itemId;
	//アイテム名
	public String itemName;
	//分類
	public enum Type {
		weapon,
		item,
	}
	public Type type;
	//レア度
	public enum Legendaly {
		common,
		rare,
		legend,
	}
	public Legendaly legendaly;
	//3Dモデル
	public GameObject model;
	//アタッチスクリプト
	public MonoBehaviour script;
	//フレーバーテキスト
	public String flaverText;

	//コンストラクタ
	public Item(Item item) {
		this.itemId = item.itemId;
		this.itemName = item.itemName;
		this.type = item.type;
		this.legendaly = item.legendaly;
		this.model = item.model;
		this.script = item.script;
		this.flaverText = item.flaverText;
	}

}
