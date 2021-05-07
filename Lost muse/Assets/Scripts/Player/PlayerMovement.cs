using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController2D controller;
	public Animator animator;

    [SerializeField] private float runSpeed;
	[SerializeField] private bool isCanMove = true;

	private float horizontalMove = 0f;
	private bool jump = false;
	private bool crouch = false;
	public bool wallGrab = false;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		}

		else if(Input.GetButtonUp("Crouch")) // Надо поменять букву для приседания. Чтобы не было конфликта с движениями Vertical
		{
			crouch = false;
		}

        if (controller.m_Wall && Input.GetButtonDown("Grab"))
        {
            wallGrab = true;
        }
		else if (!controller.m_Wall || Input.GetButtonUp("Grab"))
		{
			wallGrab = false;
			// Здесь нужно менять анимацию в аниматоре.
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

	void FixedUpdate()
	{
		if(isCanMove == true)
        {
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
			// Move our character
			controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
			jump = false;
		}
	}

}
