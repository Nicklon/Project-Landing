using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour {

	[SerializeField] Vector3 movementVector;
	[SerializeField] float period;

	Vector3 startingPos;
	// Use this for initialization
	void Start () {
		startingPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (period <=Mathf.Epsilon) {return;}

		float cycles = Time.time / period;

		const float tau = Mathf.PI * 2;
		float rawSinWave = Mathf.Sin (cycles * tau);
		float movementFactor = rawSinWave / 2f;

		Vector3 offset = movementFactor * movementVector;
		transform.position = startingPos + offset;
	}
}
