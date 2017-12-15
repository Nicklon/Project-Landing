using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour {

AudioSource audioSource;
RocketStats stats;
bool noCollisionDebug = false;

[SerializeField] float levelLoadDelay = 2f;

[SerializeField] AudioClip successSound = null;
[SerializeField] AudioClip deathSound= null;

[SerializeField] ParticleSystem successParticles= null;
[SerializeField] ParticleSystem deathParticles= null;

enum State{
	Alive,
	Dying,
	Transcending
}

State state = State.Alive;

// Use this for initialization
void Start()
{
	audioSource = GetComponent<AudioSource> ();
	stats = GetComponent<RocketStats>();
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
	}
}

private void onDeath()
{
	state = State.Dying;
	stats.SetHealth(stats.GetHealth()-1);
	
	if (stats.GetHealth() == 0) 
	{
		stats.Reset ();
		Invoke ("LoadFirstLevel", levelLoadDelay);
	} 
	else 
	{
		Invoke ("LoadLevel", levelLoadDelay);
	}

	deathParticles.Play ();
	audioSource.Stop ();
	audioSource.PlayOneShot (deathSound);
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
	audioSource.PlayOneShot (successSound);
}

void OnTriggerEnter(Collider collider)
{
		GameObject colliderOrigin = collider.gameObject;

		switch (collider.gameObject.tag) 
		{
		case "Fuel":
		stats.ChangeFuel (50);
		colliderOrigin.SetActive (false);
		break;

		case "Health":
		stats.SetHealth (stats.GetHealth()+1);
		colliderOrigin.SetActive (false);
		break;
		}
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
