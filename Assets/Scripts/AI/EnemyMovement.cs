using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

	[HideInInspector]
	public Transform player;
	private NavMeshAgent agent;
	private Animator anim;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
	}

	void Update(){
		anim.SetFloat("velocity",agent.velocity.magnitude);
	}

	public void UpdateDestinationAgent(Vector3 target){
		agent.destination = target;
		transform.LookAt(target);
		agent.Resume();
	}

	public void StopAgent(){
		agent.Stop();
	}

	public bool Landed(){
		return agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending;
	}
}
