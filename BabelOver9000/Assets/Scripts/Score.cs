using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI text;
    [HideInInspector]
    public int score = 0;

	// Use this for initialization
	void Start () {
        score = 0;
        UpdateText();
	}


    public void Add(int valueToAdd)
    {
        score += valueToAdd;
        UpdateText();
    }

    void UpdateText()
    {
        text.text = score.ToString();
    }
}
