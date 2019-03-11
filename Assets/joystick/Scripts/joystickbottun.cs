/*======================================================================
Project Name    : joyStickHandMaid
File Name       : joystickbottun.cs
Creation Date   : 2018/07/27

Copyright © 2018- Soranoana. All rights reserved.

Rights owner
Nickname: ソラノアナ(Soranoana)
Twitter : @Inanis_foramen
Github  : https://github.com/BzLOVEman

This source code or any portion thereof must not be
reproduced or used in any manner whatsoever.
======================================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickbottun : MonoBehaviour {

    public GameObject EffectiveObject;

    /* オブジェクトをタップしたら */
    public void PushDown() {
        //処理
        PushDownEvent();
    }

    /* オブジェクトをタップし終えたら */
    public void PushUp() {
        //処理
        PushUpEvent();
    }

    /* タップ時のイベント一覧 */
    private void PushDownEvent() {
        if (EffectiveObject==null) {
            Debug.LogWarning("EffectiveObject is missing. Error comes from Joystick System.\nMy ObjectName is " + transform.name+". This event is PushDownEvent");
        }else if (EffectiveObject.name=="Cube") {
            //処理1
            EffectiveObject.transform.GetComponent<MeshRenderer>().enabled=false;
        } else if (EffectiveObject.name=="") {
            //処理2
        } else {
            Debug.LogError("Unknown obejct was tapped.");
        }
    }

    /* タップ終了時のイベント一覧 */
    private void PushUpEvent() {
        if (EffectiveObject==null) {
            Debug.LogWarning("EffectiveObject is missing. Error comes from Joystick System.\nMy ObjectName is "+transform.name+". This event is PushUpEvent") ;
        } else if (EffectiveObject.name=="Cube") {
            //処理1
            EffectiveObject.transform.GetComponent<MeshRenderer>().enabled=true;
        } else if (EffectiveObject.name=="") {
            //処理2
        } else {
            Debug.LogError("Unknown obejct was tapEnded.");
        }
    }
}
