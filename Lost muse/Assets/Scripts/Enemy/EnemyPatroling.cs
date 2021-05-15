using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroling : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private float fastAttackMovingSpeed; // �������� ����� �� ����� �����
    [SerializeField] private float distanceVisualRay; // ���������� ��������� ����, ������� ��������� ����.
    [SerializeField] private float distanceToEnd; // ��������� ����, ������� ��������� ����.
    [SerializeField] private Transform groundDetector;
    [SerializeField] private LayerMask layerMask, player;
    public bool isAttack = false;
    public bool isGround = true;
    public bool isRight = true;
    public bool isMoving = true;
    public bool isCanRotate = true;

    private Rigidbody2D rb2d;
    public Collider2D colliderTouch;
    

    private void Awake()
    {
        colliderTouch = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isAttack == false)
        {
            Calm();
        }
        else if (isAttack == true)
        {
            FastAttacking();
        }
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, -Vector3.up, distanceToEnd, layerMask); // ���, ������� ��������� ����, ��� ����������� ���������

        if (groundInfo.collider == null)
        {
            isGround = false;
            if (isRight == true)
            {
                LeftRotate();
            }
            else
            {
                RightRotate();
            }
        }
        else
        {
            isGround = true;

        }
        Debug.DrawRay(groundDetector.position, Vector3.down * distanceVisualRay, Color.red); // ������������ ����

        FreezeEnemyPositon();
    }

    private void FreezeEnemyPositon()
    {
        if (colliderTouch.IsTouchingLayers(player))
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll; // ��������� �� ��� ��������, ����� ��������� ����� ������������� � �������.
        }
        else
        {
            rb2d.constraints = RigidbodyConstraints2D.None; // ������ ��������� �� ��������.
            rb2d.freezeRotation = true; // ���������� ����������� ��������� ����� � RigidBody
        }
    }



    public void LeftRotate()
    {
        if (isMoving && isCanRotate)
        {
            transform.eulerAngles = new Vector3(0, -180, 0); // �������, ����� ������� �� ���� ���������.
            isRight = false;
        }
    }

    public void RightRotate()
    {
        if (isMoving && isCanRotate)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // ������� � ��������� �������, ����� ������� �� ���������������� ���� ���������.
            isRight = true;
        }
    }

    public void Calm()
    {
        if(isGround == true)
        {
            if (isMoving == true)
            {
                transform.Translate(Vector2.right * movingSpeed * Time.fixedDeltaTime); // �������� ����� � ��������� ��������� 
            }
        }
    }

    public void FastAttacking()
    {
        if (isGround == true)
        {
            if (isMoving == true)
            {
                transform.Translate(Vector2.right * fastAttackMovingSpeed * Time.fixedDeltaTime); // �������� ����� �� ����� �����, �������������� ���������� "attackingMovingSpeed"
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            isMoving = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isMoving = true;
        }
    }

}
