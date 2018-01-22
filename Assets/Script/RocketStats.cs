using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

//Component required for updating UI
public class RocketStats : MonoBehaviour {

	//UI Text 
	Text fuelText = null;
	Text healthText = null;
	Text levelText = null;

	//Required for health management
	GameManager gameManager;

	//Initial fuel
	[SerializeField] float fuel = 100f;
	//Required to avoid race condition.
	private Object fuelSemaphore = new Object ();

	//Required to know the maximun fuel capacity.
	float initialFuel;

	// Use this for initialization
	void Start () 
	{
		initialFuel = fuel;

		//Search through the scene the desired components.
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		fuelText =  GameObject.Find ("FuelText").GetComponent<Text> ();
		healthText = GameObject.Find ("HealthText").GetComponent<Text> ();
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
	}
	
	//Updates UI Text
	void Update () 
	{
		UpdateHealthText ();
		UpdateFuelText ();
		UpdateLevelText ();
	}
		
	//Resets health.
	public void Reset()
	{
		gameManager.Reset();
	}


	private void UpdateHealthText()
	{
		healthText.text =  "Health : " + gameManager.health.ToString ();
	}

	private void UpdateFuelText()
	{
		int fuelAux = (int)fuel;
		fuelText.text =  "Fuel : " + fuelAux.ToString ();
	}

	private void UpdateLevelText()
	{
		levelText.text =  "Level : " + SceneManager.GetActiveScene ().buildIndex;
	}

	public float GetFuel()
	{
		return fuel;
	}

	//Increase fuel without excedding max fuel capacity.
	public void ChangeFuel(float x)
	{
		float resultFuel = fuel + x;

		if (resultFuel > 100)
		{
			resultFuel = 100;
		}
		else if (resultFuel < 0)
		{
			resultFuel = 0;	
		}

		lock(fuelSemaphore)
		{
			fuel = resultFuel;
		}
	}

	public int GetHealth()
	{
		return gameManager.health;
	}

	public void SetHealth(int inHealth)
	{
		gameManager.health = inHealth;
	}
}
