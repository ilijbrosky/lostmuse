using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingController : MonoBehaviour
{
    public Transform WallCheckUp;
    public Transform WallCheckDown;
    public Transform LedgeCorrect;
    public Transform groundDetector;
    public CharacterController2D controller;
    public LayerMask layerMask, whatIsGround;

    public bool onWallUp;
    public bool onWallDown;
    public bool onLedge;
    public bool onLedgeUpping;

    public float wallCheckRayDistance = 1f;
    public float distanceToGround = 1f;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        ChekingWall();
        ChekingLedge();

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, -Vector3.up, distanceToGround, layerMask);
        Debug.DrawRay(groundDetector.position, Vector3.down * distanceToGround, Color.red); // Вызуализация луча

        if (groundInfo && onLedgeUpping && onLedge)
        {
            anim.SetBool("isLedgeUp", true);
            controller.m_CanMove = false;
        }
    }

    public void FinishUppingAnim()
    {
        transform.position = groundDetector.transform.position;
        controller.m_CanMove = true;
        onLedgeUpping = false;
        anim.SetBool("isLedgeUp", false);
    }




    private void ChekingWall()
    {
        onWallUp = Physics2D.Raycast(WallCheckUp.position, transform.right, wallCheckRayDistance, whatIsGround);
        onWallDown = Physics2D.Raycast(WallCheckDown.position, transform.right, wallCheckRayDistance, whatIsGround);
    } 

    private void ChekingLedge()
    {
        if (onWallDown && !onWallUp)
        {
            onLedge = !Physics2D.Raycast(LedgeCorrect.position, transform.right, wallCheckRayDistance, whatIsGround);
        }
        else
        {
            onLedge = false;
        }

        if (onLedge)
        {
            controller.m_CanFlip = false; //отключаем возможность поворота игрока, во время висения на уступе.
            controller.m_ledge = true;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(WallCheckUp.position, transform.right * wallCheckRayDistance, Color.blue);
        Debug.DrawRay(WallCheckDown.position, transform.right * wallCheckRayDistance, Color.blue);
        Debug.DrawRay(LedgeCorrect.position, transform.right * wallCheckRayDistance, Color.red);
    }
}
