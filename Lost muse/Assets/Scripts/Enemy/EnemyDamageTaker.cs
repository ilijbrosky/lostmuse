using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTaker : MonoBehaviour
{
    [SerializeField] private EnemyPatroling  patroling;
    [SerializeField] private PlayerDetector  detector;
    [SerializeField] private float  inPainStaying;
    public Animator anim;
    private Rigidbody2D rb2d;
    private BoxCollider2D bx2d;
    public EnemyPatroling patrolingScript;

    private void Start()
    {
        anim = GetComponent<Animator>();
        bx2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void DamageTaken()
    {
        
        anim.SetBool("isPain", true);
        anim.SetBool("isFastAttack", false);
        anim.SetBool("isPlayer", false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            DamageTaken();
            patrolingScript.isMoving = false;
            patrolingScript.isCanRotate = false;
            bx2d.isTrigger = true; // ��������� ���������� ���������, ����� Player ��� ��������� ����� �����
            rb2d.gravityScale = 0; //��������� ����������, ����� ���� �� ��� ����������� ���� �� ����� (isTrigget) �� BoxCollider2D
        }
    }

    public void MovingActivator()
    {
        bx2d.isTrigger = false; // ������������ ���������, ����� Player �� ���� ��������� ����� �����(����� ���������� �������� ��������� ����� �� �����)
        rb2d.gravityScale = 1; // ������������ ���������� (����� ���������� �������� ��������� ����� �� �����).
        anim.SetBool("isPain", false);
        patrolingScript.isMoving = true;
        patrolingScript.isCanRotate = true;
    }


}
