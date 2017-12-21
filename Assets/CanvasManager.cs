using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour {

	// Use this for initialization
	void Awake(){
		if(FindObjectsOfType<Canvas>().Length >1)
		{
			Destroy(gameObject);
		}
		else
		{
			DontDestroyOnLoad (gameObject);
		}
	}
}
