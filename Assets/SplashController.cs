using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour {

	[SerializeField] Button startGameButton;
	[SerializeField] Button quitGameButton;

	// Use this for initialization
	void Start ()
	{
		startGameButton.onClick.AddListener (StartGame);
		quitGameButton.onClick.AddListener (QuitGame);
	}
	
	private void StartGame()
	{
		SceneManager.LoadScene (1);
	}

	private void QuitGame()
	{
		Application.Quit ();
	}
}
