using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour {

	public KeyCode key;
	SpriteRenderer sr;
	bool active = false;
	GameObject note;
	Color old;
	public bool createMode;
	public GameObject n;

	void Awake(){
		sr = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		old = sr.color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(createMode){
			if(Input.GetKeyDown(key)){
				Instantiate(n, transform.position, Quaternion.identity);
			}
		}

		else{
			if(Input.GetKeyDown(key)) StartCoroutine(Pressed());

			if(Input.GetKeyDown(key) && active){
				Destroy(note);
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		active = true;

		if(col.gameObject.tag == "Note")
		{
			note = col.gameObject;
		}
	}

	void OnTriggerExit(Collider col)
	{
		active = false;
	}

	IEnumerator Pressed(){

		sr.color = new Color(0,0,0);

		yield return new WaitForSeconds(.05f);
		sr.color = old;
	}
}
