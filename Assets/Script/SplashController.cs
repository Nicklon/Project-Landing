using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Component required to handle the UI on the main menu level.
public class SplashController : MonoBehaviour {

	[SerializeField] Button startGameButton;
	[SerializeField] Button quitGameButton;

	//Adds on click events.
	void Start ()
	{
		startGameButton.onClick.AddListener (StartGame);
		quitGameButton.onClick.AddListener (QuitGame);
	}

	//Loads first level.
	private void StartGame()
	{
		SceneManager.LoadScene (1);
	}

	//Quits application.
	private void QuitGame()
	{
		Application.Quit ();
	}
}
