using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnim : MonoBehaviour {

	public Animation anim;
	Animator totor;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animation> ();
		totor = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space))
			anim.Play ();
		//if (totor.GetCurrentAnimatorStateInfo (0).IsName ("AnimCloudTestToScreen"))
		if (!anim.IsPlaying ("AnimCloudTestToScreen"))
			print ("it's over");
		else
			print ("playing");
	}
}
