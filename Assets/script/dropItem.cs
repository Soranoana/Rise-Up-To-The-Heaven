using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropItem : MonoBehaviour {

    private int ItemNum=-1;
    private int[] notRotateNum = new int[] {13,14,15,16,17,18 };
    private bool isRotate = true;
    public bool publicRotateFlg;

	void Start () {
		if (gameObject.name == "DeadSickle") {
            ItemNum=1;
        } else if (gameObject.name == "DogSpear") {
            ItemNum=2;
        } else if (gameObject.name == "BuddhaShining") {
            ItemNum=3;
        } else if (gameObject.name == "DullahanSword") {
            ItemNum=4;
        } else if (gameObject.name == "KrakenHand") {
            ItemNum=5;
        } else if (gameObject.name == "DragonBurner") {
            ItemNum=6;
        } else if (gameObject.name == "DeadIncarnation") {
            ItemNum=7;
        } else if (gameObject.name == "DogMating") {
            ItemNum=8;
        } else if (gameObject.name == "BuddhaStatue") {
            ItemNum=9;
        } else if (gameObject.name == "DullahanArmor") {
            ItemNum=10;
        } else if (gameObject.name == "KrakenChild") {
            ItemNum=11;
        } else if (gameObject.name == "DragonBaby") {
            ItemNum=12;
        } else if (gameObject.name == "Potion") {
            ItemNum=13;
        } else if (gameObject.name == "HighPotion") {
            ItemNum=14;
        } else if (gameObject.name == "SoulCrystal") {
            ItemNum=15;
        } else if (gameObject.name == "Nayuta") {
            ItemNum=16;
        } else if (gameObject.name == "DreamCrystal") {
            ItemNum=17;
        } else if (gameObject.name == "testWeapon1") {
            ItemNum=18;
        }
        for (int i=0; i < notRotateNum.Length; i++) {
            if (ItemNum == notRotateNum[i]) {
                isRotate = publicRotateFlg;
                i = notRotateNum.Length;
            }
        }
	}
	
	void Update () {
        rotateSelf();
	}

    private void renameSelf() {
        if (true) { }
    }

    private void rotateSelf() {
        if (isRotate) {
            gameObject.transform.Rotate(new Vector3(0, 1, 0));
        }
    }

    public void settingDestroySelf(float time) {
        Destroy(gameObject, time);
    }

    public void settingDestroySelf() {
        Destroy(gameObject, 2);
    }

    public int getItemNum() {
        return ItemNum;
    }
}
