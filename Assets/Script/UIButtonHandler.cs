using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class UIButtonHandler : MonoBehaviour,IPointerDownHandler,IPointerUpHandler{

	bool buttonDown;
	// Use this for initialization
	void Start () {
		buttonDown = false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		buttonDown = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		buttonDown = false;
	} 

	public bool isButtonClicked()
	{
		return buttonDown;
	}
}
