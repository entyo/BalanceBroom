using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {
	private int score;
	private bool stoppingUpdate = false;
	public UnityEngine.UI.Text scoreText;

	// Use this for initialization
	void Start () {
		this.score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!stoppingUpdate)
		{
			score++;	
		}
		this.scoreText.text = this.score.ToString();
	}

	public void StopUpdate() {
		this.stoppingUpdate = true;
	}
}
