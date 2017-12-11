﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField] public int health = 5;

	private void Awake()
	{
		if(FindObjectsOfType<GameManager>().Length >1)
		{
			Destroy(gameObject);
		}
		else
		{
			DontDestroyOnLoad (gameObject);
		}
	}


}
