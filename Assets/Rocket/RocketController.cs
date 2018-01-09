using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Component required to handle scene changes, collisions and debug inputs.
public class RocketController : MonoBehaviour {

//This is required for the sound effects.
AudioSource audioSource;

//This is required to get and update the health/fuel of the rocket.
RocketStats stats;

//True when the key C its pressed to make the rocket indestructible.
bool noCollisionDebug = false;

//This is required to be able to use the methods when there is a door locked in the level.
OpenDoorLocked _lockKeyScript;

//Delay of the transition between levels.
[SerializeField] float levelLoadDelay = 2f;

//Sounds effects.
[SerializeField] AudioClip successSound = null;
[SerializeField] AudioClip deathSound= null;

//Particles. 
[SerializeField] ParticleSystem successParticles= null;
[SerializeField] ParticleSystem deathParticles= null;

//States of the rocket.
//
enum State{
	Alive,
	Dying,
	Transcending
}

//Initial state.
State state = State.Alive;

//Method called when all the objects in the scene are loaded.
void Start()
{
	//Getting components of the rocket's root
	audioSource = GetComponent<AudioSource> ();
	stats = GetComponent<RocketStats>();
}

//Method called every frame, used for response to the player's input.
void Update() 
{
	if (state == State.Alive)
	{
		if (Debug.isDebugBuild) 
		{
			RespondOnDebugKeys ();
		}
	}
}

//Method called from update attendant of checking and activate the actions assigned to the debug keys.
void RespondOnDebugKeys()
{
	//Check if L key is pressed and advance level.
	if (Input.GetKey (KeyCode.L)) {
		Invoke ("LoadNextLevel",levelLoadDelay);
	}
	//Check if C key is pressed and reverse the boolean, explained before.
	if (Input.GetKey (KeyCode.C)) {
		noCollisionDebug = !noCollisionDebug;
	}
}

//Method called when the rocket dies.
private void onDeath()
{
	//Change state to dying to prevent things like finishing the level dead.
	state = State.Dying;
	//Reduce health in stats manager.
	stats.SetHealth(stats.GetHealth()-1);
	
	//Check if rocket is dead, and if so, reset health and load main menu.
	if (stats.GetHealth() == 0) 
	{
		stats.Reset ();
		Invoke ("LoadFirstLevel", levelLoadDelay);
	} 
	//if not dead, load the same level.
	else 
	{
		Invoke ("LoadLevel", levelLoadDelay);
	}
	
	//Play particles and sounds
	deathParticles.Play ();
	audioSource.Stop ();
	audioSource.PlayOneShot (deathSound);
}

//Method called to load same scene as the player is.
public void LoadLevel()
{
	SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
}

//Method called to load the next scene, or the main menu if your in the last already.
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
		
//Method called to load the main menu.
public void LoadFirstLevel()
{
	SceneManager.LoadScene (0);
}

//Method called when the rocket hits the green platform.
private void onFinish()
{
	//Change state to prevent dying between scenes.
	state = State.Transcending;

	//Advance level calling method after delay.
	Invoke ("LoadNextLevel", levelLoadDelay);

	//Play particles and sound.
	successParticles.Play ();
	audioSource.Stop ();
	audioSource.PlayOneShot (successSound);
}

//Method called when the rocket collides with a power-up.
void OnTriggerEnter(Collider collider)
{
		//Retrieve the collider object.
		GameObject colliderOrigin = collider.gameObject;

		//If the rocket is not alive there are no collisions.
		if (state != State.Alive) 
		{
			return;
		}

		//Switch with all the power-ups available.
		switch (collider.gameObject.tag) 
		{

		//Fuel recharge 50 fuel in the stats manager.
		case "Fuel":
			stats.ChangeFuel (50);
			break;

		//Health increase in the stats manager.
		//TODO more specific method like in fuel.
		case "Health":
			stats.SetHealth (stats.GetHealth()+1);
			break;

		//Retrieve Lock object and open the object assigned at the lock power-up.
		case "Lock":
			_lockKeyScript = colliderOrigin.GetComponent<OpenDoorLocked> ();
			_lockKeyScript.Open ();
			break;
		}

		//Disable power-up.
		colliderOrigin.SetActive (false);
}

//Method called when the rocket collides with an object.
void OnCollisionEnter(Collision collision)
{
	//Retrieve the collision object.
	GameObject collisionOrigin = collision.gameObject;

	//If the rocket is not alive there are no collisions.
	if (state != State.Alive) 
	{
		return;
	}
	
	//Switch with all the tags of the possible collision objects.
	switch (collision.gameObject.tag) 
	{
		//Used in the blue platforms, it does nothing.
		case "Friendly":
			break;

		//Calls onFinish() on the green platforms.
		case "Finish":
			onFinish ();
			break; 
		
		//In other cases the rocket dies if not in debug mode.
		default:
			if (!noCollisionDebug) 
			{
				onDeath ();
			}
			break;
	}

}
}
