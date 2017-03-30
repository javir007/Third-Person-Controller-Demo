using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region Component Variables
	CharacterController controller;
	Animator anim;
	Transform cam;
	#endregion
	[SerializeField] float turnSpeed = 5; // How fast the character run
	//[SerializeField] float moveSpeed = 3f;

	private bool isInteracting = false;

	Vector3 directionPos; // The direction character look at 
	Vector3 lookPos; // Where the character look at, used in IK
	Vector3 storeDir;
	float horizontal;
	float vertical;
	bool rollInput;


	[HideInInspector]
	public bool isRolling = false;

	public bool IsInteracting{
		get{ 
			return isInteracting;
		}
		set{ 
			isInteracting = value;
		}
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		cam = Camera.main.transform;
		controller = GetComponent<CharacterController> ();
		SetupAnimator();
	}

	// Update is called once per frame
	void FixedUpdate () {
		//Inputs
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		rollInput = Input.GetButtonDown("Roll/Read");

		//the same movement of the camera always, look the player horizontal way
		//if(horizontal == 0){
		storeDir = cam.right;
		//}

		if (controller.isGrounded) {
			Vector3 moveVector  = Vector3.zero;
			moveVector.y = -100f;
			controller.SimpleMove (moveVector * Time.deltaTime);
			if (!isInteracting) {
				if (rollInput && (Mathf.Abs (vertical) > 0.5 || Mathf.Abs (horizontal) > 0.5)) {
					anim.SetTrigger ("Roll");
					isRolling = true;
				} else {
					isRolling = false;
				}
			}

		} 

		//Find a position front of where the camera is looking
		directionPos = transform.position + (storeDir * horizontal) + (cam.forward * vertical);
		//Find the direction from that position
		Vector3 dir = directionPos - transform.position;
		dir.y = 0;

		// Turn the input into animator values
		float animValue = Mathf.Abs(vertical) + Mathf.Abs(horizontal);
		animValue = Mathf.Clamp(animValue,0f,1f);
		//Pass the value to the Animator
		anim.SetFloat("Forward", animValue, .1f, Time.deltaTime);

		//If there's movement, that means it has movement input from the player
		if(horizontal !=0 || vertical != 0){
			//Find the angle, between the character's rotation and where the camera is looking
			float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir));

			//and if it's not zero(avoid the warning on console)
			if(angle != 0){
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
			}
		}
		// If there's no input, then the camera will orbit around the player
	}

	void SetupAnimator(){
		//Animator ref on the root
		anim = GetComponent<Animator>();

		//Use avatar from a child animator Component of present
		// this is to enable easy swappong of the character model as a child node
		foreach(var childAnimator in GetComponentsInChildren<Animator>()){
			if(childAnimator != anim){
				anim.avatar = childAnimator.avatar;
				Destroy(childAnimator);
				break;//when the first animator is founded, stop searching
			}
		}

	}
			
}
