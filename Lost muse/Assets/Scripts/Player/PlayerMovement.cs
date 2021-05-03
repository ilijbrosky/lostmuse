using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController2D controller;
	public Animator animator;
	public Collider2D circleCollider;
	public LayerMask layerMask;
	public Rigidbody2D rigidBody;

    [SerializeField] private float runSpeed = 40f;
	[SerializeField] private float climbSpeed = .05f;  // Скорость лазания по горе
	float horizontalMove = 0f;
	float verticalMove = 0f;
	bool jump = false;
	bool crouch = false;

	// Update is called once per frame
	void Update()
	{

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
		}
		else if(Input.GetButtonUp("Crouch"))
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
	}

	public void OnCrouching(bool isCrouching)
	{
		animator.SetBool("IsCrouching", isCrouching);
	}

	public void OnClimbing()
    {
		if (!circleCollider.IsTouchingLayers(LayerMask.GetMask("Walls")))
		{
			return;
		}
		else if (circleCollider.IsTouchingLayers(LayerMask.GetMask("Walls")))
		{
			Vector2 climbVelocity = new Vector2(rigidBody.velocity.x, verticalMove * climbSpeed);
			rigidBody.velocity = climbVelocity;

			bool isVertical = Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;

		}



		verticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;
	}

	void FixedUpdate()
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
