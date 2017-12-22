using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorLocked : MonoBehaviour {

	[SerializeField] GameObject barrier;

	// Use this for open the gameobject assigned in the serializefield
	public void Open()
	{
		barrier.SetActive (false);
	}
}
