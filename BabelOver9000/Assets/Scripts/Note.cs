using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

	Rigidbody rb;
	public int speed = 0;
	public bool isSpecial = false;

	void Awake(){
		rb = GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start () {
		rb.velocity = new Vector3(0,-speed, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
