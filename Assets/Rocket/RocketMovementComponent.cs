using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

//Component required to handle player input and rocket movement.
public class RocketMovementComponent : MonoBehaviour {
	//Required for the movement physics.
	Rigidbody rigidBody;
	//Required for thrust sound.
	AudioSource audioSource;
	//Required for fuel management.
	RocketStats stats;
	//Required for mobile controls.
	UIButtonHandler buttonhandler;

	//Rocket engine variables.
	[SerializeField] float motorThrust = 1000f;
	[SerializeField] float motorRotationThrust = 100f;
	[SerializeField] float consumeFuelRating = 10f;

	//Particles and sound effects.
	[SerializeField] AudioClip mainEngineSound= null;
	[SerializeField] ParticleSystem mainEngineParticles= null;

	//Mobile UI buttons.
	Button BMobileLeft;
	Button BMobileRight;
	Button BMobileThrust;

	//Method called when all the objects in the scene are loaded.
	void Start()
	{
		//Getting components of the rocket's root.
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		stats = GetComponent<RocketStats>();

		//Getting components of the mobile UI if mobile platform.
		if (Application.isMobilePlatform)
		{
			BMobileThrust=GameObject.Find ("ThrustMovementMobile").GetComponent<Button>();
			BMobileLeft=GameObject.Find ("LeftMovementMobile").GetComponent<Button>();
			BMobileRight=GameObject.Find ("RightMovementMobile").GetComponent<Button>();
		}
	}

	//Method called every frame, used for response to the player's input.
	void Update() 
	{
		if (Application.isMobilePlatform)
		{
			RespondOnMobile ();
		}

		RespondOnThrust();
		RespondOnRotate();
	}
		
	//Method required to check if thrust input is pressed.
	private void RespondOnThrust()
	{
		//Apply thrust if key pressed.
		if (Input.GetKey(KeyCode.Space)) 
		{
			ApplyThrust ();
		}
		//If key is not pressed stop sounds and particles.
		else if(Input.GetKeyUp(KeyCode.Space))
		{
			audioSource.Stop ();
			mainEngineParticles.Stop ();
		}
	}

	//Method required to check if rotate input is pressed.
	private void RespondOnRotate()
	{
		//Apply rotation in one direction or another depending of the input received.
		if (Input.GetKey(KeyCode.A)) 
		{
			Rotate (-1);
		}
		else if(Input.GetKey(KeyCode.D))
		{
			Rotate (1);
		}
	}

	//Method required to check if mobile buttons are pressed.
	private void RespondOnMobile()
	{
		UIButtonHandler BMobileThrustButton = BMobileThrust.GetComponent<UIButtonHandler> ();
		UIButtonHandler BMobileLeftButton = BMobileLeft.GetComponent<UIButtonHandler> ();
		UIButtonHandler BMobileRightButton = BMobileRight.GetComponent<UIButtonHandler> ();

		//Apply rotation in one direction or another depending of the button pressed.
		if (BMobileLeftButton.isButtonClicked ()) 
		{
			Rotate (-1);
		}
		else if(BMobileRightButton.isButtonClicked())
		{
			Rotate (1);	
		}

		//Apply thrust if button pressed.
		if (BMobileThrustButton.isButtonClicked()) 
		{
			ApplyThrust ();

		}
		//If key is not pressed stop sounds and particles.
		else
		{
			audioSource.Stop ();
			mainEngineParticles.Stop ();
		}
	}

	//Method required to apply the rockets rotation.
	private void Rotate(float rotation)
	{
		//to prevent spinning out of control if you collide with obstacles while rotation
		rigidBody.freezeRotation = true; 

		float rotationSpeed = motorRotationThrust * Time.deltaTime;

		transform.Rotate (Vector3.forward * - rotation * rotationSpeed);

		rigidBody.freezeRotation = false;
	}

	//Method required to apply the sound, particles and rocket thrust.
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

	//Method required to consume fuel and send it to the UI manager.
	private void ReduceFuel ()
	{
		stats.ChangeFuel(-(Time.deltaTime * consumeFuelRating));
	}

}
