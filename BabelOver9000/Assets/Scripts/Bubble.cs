using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

	public KeyCode key;
	public KeyCode key2;

	SpriteRenderer sr;

	bool active = false;
	public bool activeSpecial = false;
	GameObject note;
	Color old;

	public bool createMode;
	public GameObject n;

	Sprite bubbleActivate;
	Sprite bubbleUnactivate;

	void Awake(){
		sr = GetComponent<SpriteRenderer>();
		bubbleActivate = Resources.Load<Sprite>("Sprites/Bubble_Activate");
		bubbleUnactivate = Resources.Load<Sprite>("Sprites/Bubble");
	}

	// Use this for initialization
	void Start () {
		old = sr.color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(createMode)
		{
			if(Input.GetKeyDown(key) ||Input.GetKeyDown(key2))
			{
				Instantiate(n, transform.position, Quaternion.identity);
			}
		}

		else{
			if(Input.GetKeyDown(key) ||Input.GetKeyDown(key2)) StartCoroutine(Pressed());

			if((Input.GetKeyDown(key) || Input.GetKeyDown(key2)) && active)
			{
				if(note.gameObject == null) return;

				if (note.GetComponent<Note>().isSpecial) {
					SetModeActive();
				}

				Destroy(note);
			}
		}
	}

	void SetModeActive()
	{
		sr.sprite = bubbleActivate;
		LevelManager.instance.IncrementBubbleActivateNumber();
	}

	public void SetModeNormal(){
		activeSpecial = false;
		sr.sprite = bubbleUnactivate;

		float newX = GetComponent<Transform>().position.x;
		float newY = GetComponent<Transform>().position.y + LevelManager.instance.FLOOR_OFFSET_Y;
		float newZ = GetComponent<Transform>().position.z;

		GetComponent<Transform>().position = new Vector3(newX, newY, newZ);
	}

	void OnTriggerEnter(Collider col)
	{
		active = true;

		if(col.gameObject.tag == "Note")
		{
			note = col.gameObject;

			if(note.GetComponent<Note>().isSpecial) activeSpecial = true; //DESACTIVER QUAND RECOMMENCE TOUR
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
