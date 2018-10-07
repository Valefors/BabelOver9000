using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour l'animation des bulles quand une note est validée
//il faut juste faire la même chose avec un autre sprite (et probablement un autre nom) pour la bulle de note ratée

public class BubbleScript: MonoBehaviour {

	public Animation animWin;
	Animator totor;
	public Sprite win;
	public Sprite defaultSprite;

	// Use this for initialization
	void Start () 
	{
		animWin = GetComponent<Animation> ();
		totor = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.P))//condition de note réussie (à appeler du script, probablement)
			totor.SetBool("input",true);
		/*if (!animWin.IsPlaying ("BubbleAnimWin"))
			print ("it's over");
		else
			print ("playing");*/
	}

	public void setInputfalse(){
		totor.SetBool ("input", false);
	}
}
