using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;
	AudioSource motorSound;

	[SerializeField] float motorThrust = 100f;
	[SerializeField] float motorRotationThrust = 100f;

	enum State{
		Alive,
		Dying,
		Transcending
		}

	State state = State.Alive;

	// Use this for initialization
	void Start(){
		rigidBody = GetComponent<Rigidbody>();
		motorSound = GetComponent<AudioSource> ();
		
	}
	
	// Update is called once per frame
	void Update() {
		if (state = State.Alive)
		{
			Thrust();
			Rotate();
		}
	}

	private void Thrust()
	{
		if (Input.GetKey (KeyCode.Space)) {
			rigidBody.AddRelativeForce (Vector3.up * (motorThrust * Time.deltaTime));

			if (!motorSound.isPlaying) {
				motorSound.Play ();
			}
		}else if(Input.GetKeyUp(KeyCode.Space) ){
			motorSound.Stop ();}
	}

	private void Rotate()
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
		SceneManager.LoadScene (1);
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
			state = State.Transcending;
			Invoke ("LoadNextLevel", 1f);
			break; 
		default:
			state = State.Dying;
			print ("U dead m8");
			Invoke ("LoadFirstLevel", 1f);
			break;
		}
	}
}
