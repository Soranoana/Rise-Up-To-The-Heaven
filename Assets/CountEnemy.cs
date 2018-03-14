using UnityEngine;
using System.Collections;

public class CountEnemy : MonoBehaviour {
	public int Count;
	public Rect zahyou = new Rect (0, 0, 100, 50);

	void Start () {
		
	}
	
	// Update is called once per frame
	void OnGUI()
	{
	
		GUI.Label ( zahyou, "あと"+Count+"体");
	
	}
}