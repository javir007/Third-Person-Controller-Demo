using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInteractable : MonoBehaviour {

	[SerializeField]private bool DisableBehaviour;
	[SerializeField]private Interactable currentInteractable;

	private PlayerController playerController;

	private bool canInteract = false;

	void Awake(){
		playerController = GameManager.Instance.Player.GetComponent<PlayerController> ();
	}


	void Update(){
		if (Input.GetButtonDown ("Roll/Read")) {
			if (canInteract) {
				OnInteractableArea ();
			}
		}
	}

	 void OnInteractableArea(){
		currentInteractable.Interact ();
		print ("interaccion");
		/*if (DisableBehaviour) {
			//StartCoroutine (WaitForInteraction ());
		}*/
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			canInteract = true;
			playerController.IsInteracting = true;
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player")){
			canInteract = false;
			playerController.IsInteracting = false;
		}
	}


	/*private IEnumerator WaitForInteraction ()
	{
		//Disables and Enables the movement of the player

		yield return inputHoldWait;
		while (animator.GetCurrentAnimatorStateInfo (0).tagHash != hashLocomotionTag)
		{
			yield return null;
		}
		handleInput = true;}
	}*/
}
