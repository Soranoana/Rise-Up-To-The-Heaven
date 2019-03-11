/*======================================================================
Project Name    : joyStickHandMaid
File Name       : UIFade.cs
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
using UnityEngine.EventSystems;

public class UIFade : MonoBehaviour {

    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    joystick joystickScript;

    void Start() {
        canvasGroup.alpha = 1;
    }

    void Update() {
        if (canvasGroup.alpha>0.1f) {
            canvasGroup.alpha-=0.01f;
        }
        GetAnyAction();
    }

    /* マウスクリックイベント */
    private void GetAnyAction(){
        if (Input.GetMouseButton(1)||Input.GetMouseButton(0)) {
            canvasGroup.alpha = 1f;
        }
    }

    /* タップイベント */
    public void OnPointerDown(PointerEventData eventData) {
        canvasGroup.alpha = 1f;
    }
}
