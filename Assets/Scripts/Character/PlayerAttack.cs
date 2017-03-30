using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float fireRate = 2;
    public string[] comboParams;
    private int comboIndex = 0;
    private Animator animator;
    private float resetTimer;

	private UnleashWeapon unleashWeapon;
	private PlayerController playerController;


    void Awake()
    {
        if (comboParams == null || (comboParams != null && comboParams.Length == 0)){
			comboParams = new string[] { "Attack1", "Attack2"};
		}
        animator = GetComponent<Animator>();
		unleashWeapon = GetComponent<UnleashWeapon> ();
		playerController = GameManager.Instance.Player.GetComponent<PlayerController> ();
    }

    void Update(){
		
		//checks if the player has a weapon, can attack
		if (unleashWeapon && !playerController.IsInteracting) {
			if (Input.GetButtonDown("Fire1") && comboIndex < comboParams.Length)
			{
				Debug.Log(comboParams[comboIndex] + " triggered");
				animator.SetTrigger(comboParams[comboIndex]);

				// If combo must not loop
				comboIndex++;

				// If combo can loop
				// comboIndex = (comboIndex + 1) % comboParams.Length ;

				resetTimer = 0f;
			}
		}
        
        // Reset combo if the user has not clicked quickly enough
        if (comboIndex > 0)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer > fireRate)
            {
                animator.SetTrigger("Reset");
                comboIndex = 0;
            }
        }
    }
		
}
