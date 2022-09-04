using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
 * プレイヤーが触れた際に死亡カウントにして、再リスポーンさせる。
 */
public class DeadZone : MonoBehaviour{

    public GameObject StageController;

    void Start(){
        if(StageController==null){
            Debug.LogError("in DeadZone, StageController is null.");
        }
    }

    void Update(){
        
    }

    // void OnCollisionEnter(Collision collisionInfo){
    //     // collisionInfo.gameObject.name
    //     Debug.Log(collisionInfo.gameObject.name);
    // }

    void OnTriggerEnter(Collider other){
        // other.gameObject.name
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Player"){
            StageController.gameObject.GetComponent<StageController>().Respawn();
        }
        
    }
}
