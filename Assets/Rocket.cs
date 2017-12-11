using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;
	AudioSource audioSource;
	bool noCollisionDebug = false;

	[SerializeField] float levelLoadDelay = 2f;

	[SerializeField] float motorThrust = 100f;
	[SerializeField] float motorRotationThrust = 100f;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip success;
	[SerializeField] AudioClip death;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem successParticles;
	[SerializeField] ParticleSystem deathParticles;

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
		}
	}

	private void onDeath()
	{
		state = State.Dying;
		print ("U dead m8");
		Invoke ("LoadFirstLevel", levelLoadDelay);
		deathParticles.Play ();
		audioSource.Stop ();
		audioSource.PlayOneShot (death);
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
		rigidBody.AddRelativeForce (Vector3.up * (motorThrust * Time.deltaTime));
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot (mainEngine);
		}
		mainEngineParticles.Play ();
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

	public void LoadNextLevel()
	{
		int levelIndex = SceneManager.GetActiveScene ().buildIndex;
		int nextLevelIndex = levelIndex + 1;

		if (nextLevelIndex == SceneManager.sceneCountInBuildSettings) {
			nextLevelIndex = 0;
		}

		SceneManager.LoadScene (nextLevelIndex);
	}

	public void LoadFirstLevel()
	{
		SceneManager.LoadScene (0);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (state != State.Alive) 
		{
			return;
		}


		switch (collision.gameObject.tag) 
		{
		case "Friendly":
			print ("OK m8");
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
