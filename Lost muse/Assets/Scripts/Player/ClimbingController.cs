using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingController : MonoBehaviour
{
    public Transform WallCheckUp;
    public Transform WallCheckDown;
    public Transform LedgeCorrect;
    public CharacterController2D controller;

    public bool onWallUp;
    public bool onWallDown;
    public bool onLedge;

    public float wallCheckRayDistance = 1f;


    public LayerMask whatIsGround;


    void FixedUpdate()
    {
        ChekingWall();
        ChekingLedge();
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
            controller.m_CanFlip = false;
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
