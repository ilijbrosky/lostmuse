using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce;                          // Amount of force added when the player jumps.
	[SerializeField] private float m_grabSpeedModifier;              
	[SerializeField] private int wallSide;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement

	private Animator anim;
	const float k_GroundedRadius = .03f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	const float k_CeilingRadius = .003f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;
	public bool m_CanShoot = true; // ��� �����������, ����� �� �������� ����� �� ����� ������ ��� ���.
	public bool m_isCrouching = false; 
	public PlayerMovement playerMovement;
	[Header("Collision")]

	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	public bool m_Wall = false;                         
	public bool m_RightWall = false;                         
	public bool m_LeftWall = false;                         
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private LayerMask m_WhatIsWall;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	[SerializeField] private float m_rightOffset;
	[SerializeField] private float m_leftOffset;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
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
                if (!wasGrounded)
                    OnLandEvent.Invoke();
				if(m_Grounded && !m_wasCrouching)
                {
					m_CanShoot = true;
					anim.SetBool("IsJumping", false);
				}
			}
		}

        if (playerMovement.wallGrab == true)
        {
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_grabSpeedModifier);
			m_Rigidbody2D.gravityScale = 0;
        }
		else
        {
			m_Rigidbody2D.gravityScale = 1;
		}
	}

    private void Update()
    {

		if (!m_Grounded && !m_wasCrouching)
        {
            m_CanShoot = false;
			anim.SetBool("IsJumping", true);
		}

		m_Wall = Physics2D.Raycast(transform.position, Vector2.right, m_rightOffset, m_WhatIsWall)
			|| Physics2D.Raycast(transform.position, Vector2.left, m_leftOffset, m_WhatIsWall);

		m_RightWall = Physics2D.Raycast(transform.position, Vector2.right, m_rightOffset, m_WhatIsWall);
		m_LeftWall = Physics2D.Raycast(transform.position, Vector2.left, m_leftOffset, m_WhatIsWall);
		wallSide = m_RightWall ? 1 : -1;

		Debug.DrawRay(transform.position, Vector2.right * m_rightOffset, Color.green); // ������������ ����, ������� ������� ����� �����, ��� ����������� ������.
		Debug.DrawRay(transform.position, Vector2.left * m_leftOffset, Color.red); // ������������ ����, ������� ������� ������� �����, ��� �����.
	}


    public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (Physics2D.IsTouchingLayers(m_CrouchDisableCollider, LayerMask.GetMask("GroundCheck")))
		{
            if (m_isCrouching)
            {
				crouch = true;
				Debug.Log("222");
			}
            else
            {
				crouch = false;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// If crouching
			if (crouch)
			{
				anim.SetBool("IsCrouching", true);
				m_CrouchDisableCollider.isTrigger = true;
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
				}
                else
                {
					m_CanShoot = false;
				}
				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

			}
			else
			{
				anim.SetBool("IsCrouching", false);
				if (m_wasCrouching)
				{
					m_CrouchDisableCollider.isTrigger = false;
					m_wasCrouching = false;
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

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
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		transform.Rotate(0f, 180f, 0f);
	}

}
