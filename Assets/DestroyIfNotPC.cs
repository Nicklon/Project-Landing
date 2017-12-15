using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotPC : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		if (Application.isMobilePlatform) 
		{
			Destroy (gameObject);
		}
	}

}
