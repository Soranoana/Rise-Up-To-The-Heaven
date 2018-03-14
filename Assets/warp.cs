using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class warp : MonoBehaviour {
	public GameObject GoPoint;
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			col.gameObject.transform.rotation = Quaternion.Euler (0, GoPoint.transform.parent.transform.localEulerAngles.y, 0);
			col.gameObject.transform.position = GoPoint.transform.position;

			if (GoPoint.tag == "GoTo1" && !Spawn.Stage0Clear) {
				Spawn.Stage0Clear = true;
			} else if (GoPoint.tag == "GoTo2" && !Spawn.Stage1Clear) {
				Spawn.Stage1Clear = true;
			} else if (GoPoint.tag == "GoTo3" && !Spawn.Stage2Clear) {
				Spawn.Stage2Clear = true;
			} else if (GoPoint.tag == "GoTo4" && !Spawn.Stage3Clear) {
				Spawn.Stage3Clear = true;
			} else if (GoPoint.tag == "GoTo5" && !Spawn.Stage4Clear) {
				Spawn.Stage4Clear = true;
			} else if (GoPoint.tag == "GoTo6" && !Spawn.Stage5Clear) {
				Spawn.Stage5Clear = true;
			} else if (GoPoint.tag == "Clear") {
					SceneManager.LoadScene ("GameClear");
			}

		}
	}
}
