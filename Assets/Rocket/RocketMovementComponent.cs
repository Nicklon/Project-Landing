using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RocketMovementComponent : MonoBehaviour {

	Rigidbody rigidBody;
	AudioSource audioSource;
	GameManager gameManager;
	RocketStats stats;

	bool noCollisionDebug = false;

	[SerializeField] float motorThrust = 1000f;
	[SerializeField] float motorRotationThrust = 100f;
	[SerializeField] float consumeFuelRating = 10f;

	[SerializeField] AudioClip mainEngineSound= null;
	[SerializeField] ParticleSystem mainEngineParticles= null;


	// Use this for initialization
	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		stats = GetComponent<RocketStats>();
	}

	void RespondOnDebugKeys()
	{
		if (Input.GetKey (KeyCode.L)) 
		{
			Invoke ("LoadNextLevel", 0.0f);
		}
		if (Input.GetKey (KeyCode.C)) 
		{
			noCollisionDebug = !noCollisionDebug;
		}
	}

	// Update is called once per frame
	void Update() 
	{
		if (Debug.isDebugBuild) 
		{
			RespondOnDebugKeys ();
		}

		RespondOnThrust();
		RespondOnRotate();

		if (Input.GetKey ("escape"))
		{
			SceneManager.LoadScene (0);
		}


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
		if (stats.GetFuel() > 0) 
		{
			rigidBody.AddRelativeForce (Vector3.up * (motorThrust * Time.deltaTime));
			ReduceFuel ();

			if (!audioSource.isPlaying) 
			{
				audioSource.PlayOneShot (mainEngineSound);
			}

			mainEngineParticles.Play ();
		} else 
		{
			mainEngineParticles.Stop ();
		}
	}

	private void ReduceFuel ()
	{
		stats.ChangeFuel(-(Time.deltaTime * consumeFuelRating));
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

}
