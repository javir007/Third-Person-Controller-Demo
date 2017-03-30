using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnleashWeapon : MonoBehaviour {

	private bool unleashingWeapon = false;
	private PlayerController playerController;
	private bool unleashing = false;
	private float unleashTimer = 0f;

	private Animator animator;


	//Cambiar por la duracion de la animacion
	private float unleashCd = 0.2f;

	//Input
	private const string UNLEASH = "Roll/Read";

	//Getters and Setters
	public bool UnleashingWeapon{
		get{ 
			return unleashingWeapon;
		}
		set{ 
			unleashingWeapon = value;
		}
	}

	// Use this for initialization
	void Start () {
		playerController = GetComponent<PlayerController> ();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetButtonDown(UNLEASH) && !playerController.isRolling) && !unleashing && !playerController.IsInteracting){
				unleashing = true;
				unleashTimer = unleashCd;
				if (unleashingWeapon) {
					unleashingWeapon = false;
					animator.Play ("Disarm");
				} else {
					unleashingWeapon = true;
					animator.Play ("Equip");
				}
			}
			
		if (unleashing) {
			if (unleashTimer > 0) {
				unleashTimer -= Time.deltaTime;
			} else {
				unleashing = false;
			}
		}
	}
}
