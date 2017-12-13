using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;
	AudioSource audioSource;
	GameManager gameManager;
	bool noCollisionDebug = false;

	[SerializeField] float levelLoadDelay = 2f;

	[SerializeField] float fuel = 100f;
	[SerializeField] float motorThrust = 100f;
	[SerializeField] float motorRotationThrust = 100f;
	[SerializeField] float consumeFuelRating = 10f;

	[SerializeField] Text fuelText;
	[SerializeField] Text healthText;
	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip success;
	[SerializeField] AudioClip death;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem successParticles;
	[SerializeField] ParticleSystem deathParticles;

	int health;

	enum State{
		Alive,
		Dying,
		Transcending
		}

	State state = State.Alive;

	// Use this for initialization
	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource> ();

		//Scene level = GetComponentInParent<Scene> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();;
		health = gameManager.health;
	}

	void RespondOnDebugKeys()
	{
		if (Input.GetKey (KeyCode.L)) {
			Invoke ("LoadNextLevel", 0.0f);
		}
		if (Input.GetKey (KeyCode.C)) {
			noCollisionDebug = !noCollisionDebug;
		}
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (state == State.Alive)
		{
			if (Debug.isDebugBuild) 
			{
				RespondOnDebugKeys ();
			}

			RespondOnThrust();
			RespondOnRotate();
			UpdateFuel ();
			UpdateHealth ();

			if (Input.GetKey ("escape"))
			{
				SceneManager.LoadScene (0);
			}

		}
	}

	private void onDeath()
	{
		state = State.Dying;
		health--;
		if (health == 0) 
		{
			gameManager.health = 3;
			Invoke ("LoadFirstLevel", levelLoadDelay);
		} 
		else 
		{
			gameManager.health = health;
			Invoke ("LoadLevel", levelLoadDelay);
		}

		deathParticles.Play ();
		audioSource.Stop ();
		audioSource.PlayOneShot (death);
	}

	public void LoadLevel()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void LoadNextLevel()
	{
		int levelIndex = SceneManager.GetActiveScene ().buildIndex;
		int nextLevelIndex = levelIndex + 1;

		if (nextLevelIndex == SceneManager.sceneCountInBuildSettings) 
		{
			nextLevelIndex = 0;
		}

		SceneManager.LoadScene (nextLevelIndex);
	}

	public void LoadFirstLevel()
	{
		SceneManager.LoadScene (0);
	}

	private void onFinish()
	{
		state = State.Transcending;
		Invoke ("LoadNextLevel", levelLoadDelay);
		successParticles.Play ();
		audioSource.Stop ();
		audioSource.PlayOneShot (success);
	}

	private void RespondOnThrust()
	{
		if (Input.GetKey (KeyCode.Space)) 
		{
			ApplyThrust ();

		}else if(Input.GetKeyUp(KeyCode.Space) )
		{
			audioSource.Stop ();
			mainEngineParticles.Stop ();
		}
	}

	void ApplyThrust ()
	{
		if (fuel > 0) {
			rigidBody.AddRelativeForce (Vector3.up * (motorThrust * Time.deltaTime));
			ReduceFuel ();

			if (!audioSource.isPlaying) {
				audioSource.PlayOneShot (mainEngine);
			}

			mainEngineParticles.Play ();
		} else 
		{
			mainEngineParticles.Stop ();
		}
	}

	private void ReduceFuel ()
	{
		fuel -= Time.deltaTime * consumeFuelRating;
	}


	private void RespondOnRotate()
	{

		rigidBody.freezeRotation = true; //to prevent spinning out of control if you collide with obstacles while rotation

		float rotationSpeed = motorRotationThrust * Time.deltaTime;

		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (Vector3.forward * rotationSpeed);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (-Vector3.forward * rotationSpeed);
		}


		rigidBody.freezeRotation = false;
	}

	private void UpdateFuel()
	{
		int fuelAux = (int)fuel;
		fuelText.text =  "Fuel : " + fuelAux.ToString ();
	}

	private void UpdateHealth()
	{
		healthText.text =  "Health : " + health.ToString ();
	}


	void OnCollisionEnter(Collision collision)
	{
		GameObject collisionOrigin = collision.gameObject;

		if (state != State.Alive) 
		{
			return;
		}


		switch (collision.gameObject.tag) 
		{
		case "Fuel":
			
			fuel += 50f;
			if (fuel > 100)fuel = 100f;
			collisionOrigin.SetActive (false);
			break;
		case "Friendly":
			break;
		case "Finish":
			onFinish ();
			break; 
		default:
			if (!noCollisionDebug) 
			{
				onDeath ();
			}
				break;
			}

	}
}
