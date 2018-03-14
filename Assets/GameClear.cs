using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour {

	public float t = 0;
	public Text an;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		if (t >= 8) {
			SceneManager.LoadScene ("Title");
		}
	}
}
