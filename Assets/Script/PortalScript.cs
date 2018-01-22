using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {

	[Header("Portal properties")]
	[SerializeField] Vector3 exitForceVector;
	[Tooltip("For attract expel purposes")][SerializeField] float portalForce;
	[SerializeField] float forceRadius;

	bool attracting = true;

	void Update()
	{
		ApplyGravitationalForce ();
	} 

	void ApplyGravitationalForce()
	{
		foreach(Collider collider in Physics.OverlapSphere(transform.position,forceRadius))
		{
			//Check if the leg of the rocket is inside the radius
			if (collider.name == "Rocket") 
			{
				//get direction from portal to rocket
				Vector3 forceDirection = transform.position - collider.transform.position;

				if (!attracting) 
				{
					forceDirection = -forceDirection;
				}

				//apply force to the rocket
				collider.attachedRigidbody.AddForce (forceDirection.normalized * portalForce * Time.fixedDeltaTime);
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		//Retrieve the collision object.
		GameObject collisionOrigin = collision.gameObject;

		//Switch with all the tags of the possible collision objects.
		switch (collision.gameObject.name) 
		{
		case "Rocket":
			SendMessageUpwards ("MoveThroughPortals", gameObject.name);
			break;

		default:
			break;
		}
	}
		
	//Invert force of the portal when someone pass through
	public void InverseAttraction()
	{
		attracting = !attracting;
	}

	//Return portal force direction
	public int GetAttraction ()
	{
		return attracting ? 1 : 0;
	}

	public Vector3 GetPortalPosition()
	{
		return transform.position;
	}

	public Vector3 GetExitVector()
	{
		return exitForceVector;
	}
}
