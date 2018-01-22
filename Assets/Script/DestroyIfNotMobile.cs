using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotMobile: MonoBehaviour 
{
	
	void Start () 
	{
		if (!Application.isMobilePlatform) 
		{
			Destroy (gameObject);
		}
	}
}
