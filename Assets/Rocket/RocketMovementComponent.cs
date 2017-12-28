using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;


public class RocketMovementComponent : MonoBehaviour {

	Rigidbody rigidBody;
	AudioSource audioSource;
	GameManager gameManager;
	RocketStats stats;
	UIButtonHandler buttonhandler;

	bool noCollisionDebug = false;

	[SerializeField] float motorThrust = 1000f;
	[SerializeField] float motorRotationThrust = 100f;
	[SerializeField] float consumeFuelRating = 10f;

	[SerializeField] AudioClip mainEngineSound= null;
	[SerializeField] ParticleSystem mainEngineParticles= null;

	Button BMobileLeft;
	Button BMobileRight;
	Button BMobileThrust;

	// Use this for initialization
	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		stats = GetComponent<RocketStats>();

		if (Application.isMobilePlatform)
		{
			BMobileThrust=GameObject.Find ("ThrustMovementMobile").GetComponent<Button>();
			BMobileLeft=GameObject.Find ("LeftMovementMobile").GetComponent<Button>();
			BMobileRight=GameObject.Find ("RightMovementMobile").GetComponent<Button>();
		}
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

		if (Application.isMobilePlatform)
		{
			RespondOnMobile ();
		}

		RespondOnThrust();
		RespondOnRotate();
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
		if (Input.GetKey(KeyCode.Space)) 
		{
			ApplyThrust ();
		}
		else if(Input.GetKeyUp(KeyCode.Space))
		{
			audioSource.Stop ();
			mainEngineParticles.Stop ();
		}
	}

	private void RespondOnRotate()
	{
		if (Input.GetKey(KeyCode.A)) 
		{
			Rotate (-1);
		}
		else if(Input.GetKey(KeyCode.D))
		{
			Rotate (1);
		}
	}

	private void RespondOnMobile()
	{
		UIButtonHandler BMobileThrustButton = BMobileThrust.GetComponent<UIButtonHandler> ();
		UIButtonHandler BMobileLeftButton = BMobileLeft.GetComponent<UIButtonHandler> ();
		UIButtonHandler BMobileRightButton = BMobileRight.GetComponent<UIButtonHandler> ();

		if (BMobileLeftButton.isButtonClicked ()) 
		{
			Rotate (-1);
		}
		else if(BMobileRightButton.isButtonClicked())
		{
			Rotate (1);	
		}

		if (BMobileThrustButton.isButtonClicked()) 
		{
			ApplyThrust ();

		}
		else
		{
			audioSource.Stop ();
			mainEngineParticles.Stop ();
		}
	}

	private void AlternateRotate(float rotation)
	{

		rigidBody.freezeRotation = true; //to prevent spinning out of control if you collide with obstacles while rotation

		float rotationSpeed = motorRotationThrust * Time.deltaTime;
		rigidBody.AddRelativeForce (Vector3.right * ((motorThrust/2) * Time.deltaTime) * rotation);
		transform.Rotate ((Vector3.forward * - rotation * rotationSpeed)/2);

		rigidBody.freezeRotation = false;
	}

	private void Rotate(float rotation)
	{

		rigidBody.freezeRotation = true; //to prevent spinning out of control if you collide with obstacles while rotation

		float rotationSpeed = motorRotationThrust * Time.deltaTime;

		transform.Rotate (Vector3.forward * - rotation * rotationSpeed);

		rigidBody.freezeRotation = false;
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
		} 
		else 
		{
			mainEngineParticles.Stop ();
		}
	}

	private void ReduceFuel ()
	{
		stats.ChangeFuel(-(Time.deltaTime * consumeFuelRating));
	}

}
