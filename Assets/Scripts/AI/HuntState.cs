using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntState : MonoBehaviour {


	private States states;
	private EnemyMovement enemyMovement;
	private FieldOfView fow;

	void Awake(){
		states = GetComponent<States>();
		enemyMovement = GetComponent<EnemyMovement> ();
		fow = GetComponent<FieldOfView> ();
	}

	void Update(){
		if (fow.FindVisibleTargets ()) {
			enemyMovement.UpdateDestinationAgent (fow.playerLooked);
		} else {
			states.EnableState(states.alert);
			return;
		}
	}

}
