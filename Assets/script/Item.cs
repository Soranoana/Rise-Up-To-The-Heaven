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
	//アイテムの種類（武器、回復アイテムetc..）
	public enum Type{
		UserItem,
		CraftItem,
		KeyItem,
	}

	public Type type;
	//アイテムの説明
	public String information;

	//コンストラクタ
	public Item(Item item){
		this.type=item.type;
		this.information = item.information;
	}

}
