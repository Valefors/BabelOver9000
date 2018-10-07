using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

	Rigidbody rb;
	public int speed = 0;
	public bool isSpecial = false;
	public bool isLastNote = false;

	void Awake(){
		rb = GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start () {
		rb.velocity = new Vector3(0,-speed, 0);
	}
}
