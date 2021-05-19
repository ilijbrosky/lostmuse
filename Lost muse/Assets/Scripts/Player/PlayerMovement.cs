using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController2D controller;
	public ClimbingController ClimbingController;
	public Animator animator;

    [SerializeField] private float runSpeed;

	private float horizontalMove = 0f;
	private bool jump = false;
	private bool crouch = false;

    void FixedUpdate()
    {
        if (controller.m_CanMove)
        {
            // Move our character
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            jump = false;
        }
    }

    void Update()
    {
        if (controller.m_CanMove)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                controller.m_CanClimb = false;
                controller.offsetDisable = true;
            }

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
                controller.m_sit = true;
            }

            else if (Input.GetButtonUp("Crouch"))
            {

                crouch = false;
            }

            if (controller.m_Wall && Input.GetButtonDown("Grab"))
            {
                controller.m_Grabbing = true;
                controller.LedgeDetector();
            }
            else if (!controller.m_Wall || Input.GetButtonUp("Grab") && !ClimbingController.onLedge)
            {
                controller.m_Grabbing = false;
            }

            if (Input.GetButtonDown("LedgeUpping"))
            {
                ClimbingController.onLedgeUpping = true;

            }
            if (Input.GetButtonUp("LedgeUpping"))
            {
               ClimbingController.onLedgeUpping = false;
            }
        }
    }


    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }






}
