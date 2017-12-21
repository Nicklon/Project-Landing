using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField] public int health = 5;
	int initialHealth;

	float gamePaused = 0.0f;
	GameObject pauseMenu;
	Button resumeButton;
	Button mainMenuButton;

	public void Start()
	{
		initialHealth = health;
		Screen.orientation = ScreenOrientation.Landscape;  
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
	{
		if (pauseMenu == null) 
		{
			print ("Scene Loaded succesfully "+ scene.buildIndex);
			pauseMenu = GameObject.Find ("PauseMenu").transform.GetChild(0).gameObject;
			resumeButton = pauseMenu.transform.GetChild(0).gameObject.transform.Find("ResumeButton").gameObject.GetComponent<Button>();
			mainMenuButton = pauseMenu.transform.GetChild(0).gameObject.transform.Find("MainMenuButton").gameObject.GetComponent<Button>();

			resumeButton.onClick.AddListener(OnResumeButton);
			mainMenuButton.onClick.AddListener(OnMainMenuButton);
		}

		if (SceneManager.GetActiveScene ().buildIndex == 0) 
		{
			Destroy(GameObject.FindGameObjectWithTag("Persistent"));
		}
	}

	private void OnResumeButton()
	{
		print ("RESUME");
		Pause ();
	}

	private void OnMainMenuButton()
	{
		Reset ();
		Pause ();
		SceneManager.LoadScene (0);
	}

	public void Update()
	{
		RespondOnPause ();
	}
		
	private void RespondOnPause()
	{
		if (Input.GetKeyDown (KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex>0) 
		{
			Pause();
		}
	}

	private void Pause()
	{
		Time.timeScale = gamePaused;
		gamePaused = (gamePaused == 0.0f) ? 1.0f : 0.0f;
		pauseMenu.SetActive (!pauseMenu.activeSelf);
	}

	private void Awake()
	{
		if(FindObjectsOfType<GameManager>().Length >1)
		{
			Destroy(gameObject);
		}
		else
		{
			DontDestroyOnLoad (gameObject);
		}
	}

	public int GetInitialHealth()
	{
		return initialHealth;
	}

	public void Reset()
	{
		health = initialHealth;
	}
}
