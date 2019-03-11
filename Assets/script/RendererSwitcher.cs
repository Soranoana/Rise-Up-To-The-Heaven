using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererSwitcher : MonoBehaviour {

    public bool enable;
    public bool render;

	void Start () {
        this.gameObject.SetActive(enable);
        this.gameObject.GetComponent<MeshRenderer>().enabled=render;
	}
	
	void Update () {
		
	}
}
