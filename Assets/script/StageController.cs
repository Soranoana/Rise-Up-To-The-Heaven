using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージレベルでのコントローラー
public class StageController : MonoBehaviour{

    private Vector3 startPosition=new Vector3(20.57f, 1.5f, 8.09f);
    public GameObject Player;

    void Start(){
        if(Player==null){
            Debug.LogError("in StageController, Player is Null.");
        }
    }

    void Update(){
        
    }

    public void Respawn(){
        Player.gameObject.transform.position=startPosition;
    }
}
