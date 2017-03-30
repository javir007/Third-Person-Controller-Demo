using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : MonoBehaviour {
	public float velTurn = 120f;
	public float timeLooking = 4f;


	private States states;
	private FieldOfView fow;
	private float timer;

	void Awake(){
		states = GetComponent<States>();
		fow = GetComponent<FieldOfView> ();

	}

	void OnEnable(){
		timer = 0f;
	}

	void Update(){
		if (fow.FindVisibleTargets()) {
			states.EnableState(states.hunt);
			return;
		}
			
		transform.Rotate(0f, velTurn * Time.deltaTime, 0f);
		timer += Time.deltaTime;
		if(timer >= timeLooking){
			states.EnableState(states.patrol);
			return;
		}
	}

}
