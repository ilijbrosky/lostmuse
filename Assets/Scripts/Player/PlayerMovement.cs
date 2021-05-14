using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;

	[SerializeField] private float runSpeed = 40f;
	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	
	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("IsJumping", true);

		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		} else
		{
			crouch = false;
		}

		if (Input.GetButtonDown("Shoot"))
		{
			Attack();
		}

	}

	public void OnLanding()
    {
		animator.SetBool("IsJumping", false);
		Debug.Log("sadasd");
    }

	public void OnCrouching (bool isCrouching)
    {
		animator.SetBool("IsCrouching", isCrouching);
    }

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}

	void Attack()
	{
		// Play an attack animation
		animator.SetTrigger("Attack");

		// Detect enemies in range of attack
		// Damage them 
	}
}
