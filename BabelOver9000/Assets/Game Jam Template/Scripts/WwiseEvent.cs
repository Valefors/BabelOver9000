using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void onPointerHover () {
    AkSoundEngine.PostEvent("Play_Select",gameObject);
    }
}
