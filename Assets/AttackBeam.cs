using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;// ←new!
using UnityStandardAssets.CrossPlatformInput;


public class AttackBeam: MonoBehaviour {

	public float t;
	public float edt;
	public float r;
	ParticleSystem BeamP;
	LineRenderer line;

	void Awake(){
		BeamP = GetComponent<ParticleSystem> ();
		line  = GetComponent<LineRenderer> ();
	}

	void Update ()
	{
		t += Time.deltaTime;	
		r = 10;
		int mask =1 << LayerMask.GetMask(new string[] {"Player"});
		RaycastHit hit=new RaycastHit();
		shot (mask,hit);
		if (t >= 3) {
		disableEffect ();
		}	
	}

	private void shot(int mask,RaycastHit hit){
		t = 0;
		BeamP.Stop ();
		BeamP.Play ();
		line.enabled = true;
		line.SetPosition (0, transform.position);

		if (Physics.Raycast (this.transform.position, transform.forward, 2.0f, mask)) {
			if (hit.collider)
				Debug.Log ("nonke");
		}
	}		
		

//	line.SetPosition (1,(transform.position*r));


	private void disableEffect ()
	{
		BeamP.Stop ();
		line.enabled = false;
	}	
}