/*======================================================================
Project Name    : joyStickHandMaid
File Name       : MovingCube.cs
Creation Date   : 2018/07/27

Copyright © 2018- Soranoana. All rights reserved.

Rights owner
Nickname: ソラノアナ(Soranoana)
Twitter : @Inanis_foramen
Github  : https://github.com/BzLOVEman

This source code or any portion thereof must not be
reproduced or used in any manner whatsoever.
======================================================================*/
using UnityEngine;
using System.Collections;

public class MovingCube : MonoBehaviour {

    [SerializeField]
    private joystick _joystick_move = null;
    [SerializeField]
    private joystick _joystick_rotate = null;

    Vector3 pos = Vector3.zero;
    Vector3 rot = Vector3.zero;

    //移動速度
    private const float posSpeed = 0.1f;
    private const float rotSpeed = 1f;

    private void Update() {
        pos = transform.position;
        rot = transform.eulerAngles;

        pos.x += _joystick_move.Position.x * posSpeed;
        pos.z += _joystick_move.Position.y * posSpeed;
        rot.x += _joystick_rotate.Position.x * rotSpeed;
        rot.y += _joystick_rotate.Position.y * rotSpeed;

        transform.position = pos;
        transform.eulerAngles = rot;
    }

}