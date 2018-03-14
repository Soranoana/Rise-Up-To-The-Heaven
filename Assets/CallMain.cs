using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;// ←new!

public class CallMain : MonoBehaviour {
	public static bool D=false;


	void OnGUI() {
		if (GUI.Button (new Rect (400, 400, 200, 60), "ゲーム開始"))
			SceneManager.LoadScene("SUB");
		if (GUI.Button (new Rect (400, 600, 200, 60), "デバッグ"))
			D = true;
			SceneManager.LoadScene("SUB");
	}
}