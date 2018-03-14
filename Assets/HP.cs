using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HP : MonoBehaviour {

	public float PLHP=100.0f;
	public Slider slider;  

	// Use this for initialization
	void Start () {
		this.slider = GetComponent<Slider>();
		slider.value = PLHP;
	}

	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision col){

		if(col.gameObject.tag == "Enemy"){
			PLHP-=10.0f;
		}
	}
}