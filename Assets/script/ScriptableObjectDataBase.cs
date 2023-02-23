using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 擬似的なDBの作成を行うプログラム
 * DBのテーブルに相当する
 * 検索等は未実装
 */

//projectビューの 右クリック＞Create に本オブジェクトの追加ボタンを配置
[CreateAssetMenu(fileName = "ScriptableObjectDataBase",menuName = "Create_ScriptableObjectDataBase")]
public class ScriptableObjectDataBase : ScriptableObject {
	
	//レコード一覧
    public List<Item> items = new List<Item>();

}
