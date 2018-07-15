using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーンマネジメントを有効にする

public class SceneTransition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		// 左クリック or タップ
        if (Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
        {
            SceneManager.LoadScene("Game");
        }

    }
}