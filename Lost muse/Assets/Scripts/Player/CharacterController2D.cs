using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CharacterController2D : MonoBehaviour
{
	[Range(0, 1)]
	[SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[SerializeField] private float m_JumpForce;                          // Amount of force added when the player jumps.
	[SerializeField] private float m_grabSpeedModifier;        
	
	[HideInInspector]
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;

	[HideInInspector]
	public bool m_CanShoot = true; // Для определения, может ли стрелять игрок во время прыжка или нет.
	[HideInInspector]
	public bool m_Wall = false, m_RightWall = false, m_LeftWall = false;
	[HideInInspector]
	public bool m_sit = false, m_Grabbing = false;


	
	
	public bool m_ledge;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	public bool m_CanFlip = true;
	public bool m_CanClimb = true;
	private bool m_Grounded;            // Whether or not the player is grounded.
	public bool offsetDisable = false;           

	const float k_GroundedRadius = .03f; // Radius of the overlap circle to determine if grounded
	const float k_CeilingRadius = .003f; // Radius of the overlap circle to determine if the player can stand up

	public PlayerMovement playerMovement;

	private Animator anim;
	private Rigidbody2D m_Rigidbody2D;


	[Header("Collision")]
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private LayerMask m_WhatIsWall;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	[HideInInspector]
	[SerializeField] private float m_rightOffset, m_leftOffset;

	private Vector3 m_Velocity = Vector3.zero;
	private int wallSide;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	[Header("Events")]
	[Space]

	public BoolEvent OnCrouchEvent;
	public bool m_wasCrouching = false;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();


		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		GroundDetector();
		Grabbing();
		LedgeDetector();
		GrabbingBlock();
		GrabbingDisabler();
	}

	private void Update()
	{
		m_Wall = Physics2D.Raycast(transform.position, Vector2.right, m_rightOffset, m_WhatIsWall)
			|| Physics2D.Raycast(transform.position, Vector2.left, m_leftOffset, m_WhatIsWall);

		m_RightWall = Physics2D.Raycast(transform.position, Vector2.right, m_rightOffset, m_WhatIsWall);
		m_LeftWall = Physics2D.Raycast(transform.position, Vector2.left, m_leftOffset, m_WhatIsWall);
		wallSide = m_RightWall ? 1 : -1;

		Debug.DrawRay(transform.position, Vector2.right * m_rightOffset, Color.green); // Visual Ray outgoing from the Player.
		Debug.DrawRay(transform.position, Vector2.left * m_leftOffset, Color.red); // Visual Ray outgoing from the Player.
	}

	public void Grabbing()
    {

		if (m_Grabbing && !m_ledge) // Wall grabbing controlling by PlayerMovement script(Input manager)
		{
			m_CanFlip = false;
			anim.SetBool("isGrabbing", true);
			anim.SetBool("IsJumping", false);
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_grabSpeedModifier);
			m_Rigidbody2D.gravityScale = 0;
		}
		else if (!m_Grabbing)
		{
			m_Rigidbody2D.gravityScale = 1;
			anim.SetBool("isGrabbing", false);
			anim.SetBool("isLedge", false);
			m_ledge = false;
			m_CanFlip = true;
		}
	}

	public void LedgeDetector()
    {
		
		if (m_ledge && m_CanClimb)
		{
			anim.SetBool("isLedge", true);
			anim.SetBool("IsJumping", false);
			m_Rigidbody2D.velocity = new Vector2(0, 0);
			m_Rigidbody2D.gravityScale = 0;
			m_CanShoot = false;
        }
	}

	public void GroundDetector()
    {
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (m_Grounded && !m_wasCrouching) // When Player stay on the ground and not crouching
				{
					offsetDisable = false;
					m_CanClimb = true;
					m_CanShoot = true;
					anim.SetBool("IsJumping", false);
				}
			}
		}
	}


    public void Move(float move, bool crouch, bool jump)
	{
		if (!m_Grounded && !m_wasCrouching && !m_Grabbing)
		{
			anim.SetBool("IsJumping", true);
			m_CanShoot = false;
		}
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				if (m_sit)
				{
					crouch = true;
				}
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || !m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				m_CanShoot = false;
				if (!m_wasCrouching)
				{

					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
					anim.SetBool("IsCrouching", true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
				{
					m_CrouchDisableCollider.enabled = false;
				}
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
				{
					m_CrouchDisableCollider.enabled = true;
					m_sit = false;
					crouch = false;
				}


				if (m_wasCrouching)
				{
					m_sit = false;
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
				anim.SetBool("IsCrouching", false);
			}

			// Move the character by finding the target velocity
			//Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			//m_Rigidbody2D.velocity = targetVelocity;


			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, move * Time.deltaTime);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	private void Flip()
	{
        if (m_CanFlip)
        {
			// Switch the way the player is labelled as facing.
			m_FacingRight = !m_FacingRight;

			// Multiply the player's x local scale by -1.
			transform.Rotate(0f, 180f, 0f);
		}
	}

	private void GrabbingBlock()
    {
		if(m_LeftWall && m_FacingRight)
        {
			m_Grabbing = false;
        }
		else if (m_RightWall && !m_FacingRight)
		{
			m_Grabbing = false;
		}
	}

	private void GrabbingDisabler()
    {
		
        if (offsetDisable)
        {
			m_rightOffset = 0f;
			m_leftOffset = 0f;
        }
        else
        {
			m_rightOffset = 0.05f;
			m_leftOffset = 0.05f;
		}
    }

}
