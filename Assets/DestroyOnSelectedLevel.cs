using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSelectedLevel : MonoBehaviour {

	[SerializeField]int level;

	// Use this for initialization
	void Start () 
	{
		if (gameObject != null) 
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
	}

	private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
	{
		print(gameObject.name + " BUILD LEVEL INDEX : " + scene.buildIndex + " LEVEL SELECTED : " + level);

		Scene activeScene = SceneManager.GetActiveScene();
		if (activeScene.buildIndex == level) 
		{
			if (gameObject == null) {
				print ("LOL");
			
			} 
			else
			{
				//gameObject.SetActive (false);
				this.gameObject.SetActive(false);
				SceneManager.sceneLoaded -= OnSceneLoaded;
			}
		}
	}
}
