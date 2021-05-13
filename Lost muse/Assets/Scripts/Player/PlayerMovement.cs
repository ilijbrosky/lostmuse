using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController2D controller;
	public Animator animator;

    [SerializeField] private float runSpeed;

	private float horizontalMove = 0f;
	private bool jump = false;
	private bool crouch = false;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
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
        }
        else if (!controller.m_Wall || Input.GetButtonUp("Grab"))
        {
            controller.m_Grabbing = false;
        }
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
