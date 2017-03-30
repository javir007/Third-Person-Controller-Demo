using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class States : MonoBehaviour {

	public MonoBehaviour patrol;
	public MonoBehaviour alert;
	public MonoBehaviour hunt;
	public MonoBehaviour firstSate;

	private MonoBehaviour currentState;
	
	// Use this for initialization
	void Start () {
		EnableState(firstSate);
	}
	
	// Update is called once per frame
	public void EnableState(MonoBehaviour newState){
		if(currentState != null)
			currentState.enabled = false;
		
		currentState = newState;
		currentState.enabled = true;
	}
}
