using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : MonoBehaviour {

	public Vector3[] waypoints;

	private EnemyMovement enemyMovement;
	private States states;
	private FieldOfView fow;
	private int currentTargetIndex;

	void Awake(){
		enemyMovement = GetComponent<EnemyMovement> ();
		fow = GetComponent<FieldOfView> ();
		states = GetComponent<States>();
	}


	void OnEnable () {
		currentTargetIndex = 0;	
		enemyMovement.UpdateDestinationAgent (waypoints [currentTargetIndex]);
	}
	
	// Update is called once per frame
	void Update () {
		if (fow.FindVisibleTargets()) {
			states.EnableState(states.hunt);
			return;
		}

		if (enemyMovement.Landed ()) {
			NextPoint ();
		}
		enemyMovement.UpdateDestinationAgent (waypoints [currentTargetIndex]);
	}

	public void NextPoint(){
		currentTargetIndex = (currentTargetIndex < waypoints.Length - 1) ? currentTargetIndex + 1 : 0;
	}

	public void Reset(){
		waypoints = new Vector3[4];
		float defaultDistance = 5f;
		waypoints [0] = new Vector3 (0f, 0f, defaultDistance) + transform.position;
		waypoints [1] = new Vector3 (defaultDistance, 0f, 0f) + transform.position;
		waypoints [2] = new Vector3 (0f, 0f, defaultDistance) + transform.position;
		waypoints [3] = new Vector3 (defaultDistance, 0f, 0f) + transform.position;
	}


}
