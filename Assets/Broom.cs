using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : MonoBehaviour {
	public GameObject CanvasGO, GameOverGO;
	void OnCollisionEnter(Collision other)
    {
		if (other.gameObject.tag == "Floor")
		{
			CanvasGO.SendMessage("StopUpdate");
			GameOverGO.SendMessage("End");
			Canvas.SetActive("RestartButton", true);
		}
     }
}