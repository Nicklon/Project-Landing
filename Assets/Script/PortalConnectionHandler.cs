using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalConnectionHandler : MonoBehaviour {

	PortalScript _portalScript;

	[SerializeField] GameObject firstPortal;
	[SerializeField] GameObject secondPortal;
	[SerializeField] float portalExitDistance;

	PortalScript firstPortalScript;
	PortalScript secondPortalScript;

	void Start()
	{
	}

	void MoveThroughPortals(string origin)
	{
		PortalScript firstPortalScript = firstPortal.GetComponent<PortalScript>();
		PortalScript secondPortalScript = secondPortal.GetComponent<PortalScript>();

		GameObject rocket = GameObject.Find ("Rocket");

		Vector3 exitVector = new Vector3 ();
		Vector3 exitPosition = new Vector3 ();

		if (rocket == null) 
		{
			return ;
		}

		if (origin == firstPortal.name)
		{
			exitPosition = secondPortalScript.GetPortalPosition() + (portalExitDistance * secondPortalScript.GetExitVector());

			rocket.transform.position = exitPosition;
			rocket.transform.localRotation = Quaternion.Euler(0, 0, 0);
		} 
		else 
		{
			exitPosition = firstPortalScript.GetPortalPosition() + (portalExitDistance * firstPortalScript.GetExitVector());

			rocket.transform.position = exitPosition;
			rocket.transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
	}
}
