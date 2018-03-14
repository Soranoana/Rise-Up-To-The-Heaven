using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public float t = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		if (t >= 8) {
//			Application.LoadLevel ("Title");
			SceneManager.LoadScene ("Title");
		}
	}
}
