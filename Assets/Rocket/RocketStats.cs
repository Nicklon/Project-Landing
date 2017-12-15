using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RocketStats : MonoBehaviour {

	[SerializeField] Text fuelText = null;
	[SerializeField] Text healthText = null;

	GameManager gameManager;

	[SerializeField] float fuel = 100f;
	private Object fuelSemaphore = new Object ();

	float initialFuel;
	int initialHealth;
	// Use this for initialization
	void Start () 
	{
		initialFuel = fuel;
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		initialHealth = gameManager.health;
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateHealthText ();
		UpdateFuelText ();
		
	}
		
	public void Reset()
	{
		gameManager.health = initialHealth;
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

	public float GetFuel()
	{
		return fuel;
	}

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
