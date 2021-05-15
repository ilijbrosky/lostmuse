
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{

    [SerializeField] private float rayCastLength; // ��������� ��� ������������ ������� ����� �����.
    [SerializeField] private float backRayCastLength; // ��������� ���������� ������, ���� �� ���������� �� ������ �����
    [SerializeField] private float fastAttackLength; // ��������� ������������ ����� ��� �����.
    [SerializeField] private LayerMask layerMask; 
    [SerializeField] private bool isReadyFastAttack = true;
    public EnemyPatroling patrolingScript;
    public Transform fastAttackDetector;
    private BoxCollider2D boxCollider;
    private Animator anim;
    
    RaycastHit2D hit, backHit, fastAttackHit;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (patrolingScript.isMoving)
        {
        IsFastAttacking();
        ForwardAttacking();
        IsBackRay();
        }

        if (Physics2D.IsTouchingLayers(boxCollider, LayerMask.GetMask("Painter")))
        {
            anim.SetBool("isPlayer", true);
            patrolingScript.isAttack = true;
        }

        hit = Physics2D.Raycast(transform.position, transform.right, rayCastLength, layerMask); // ���, ������� ������� � ����������� �������� �����. ��������� �� ���� ������������� � Layer Mask. 
        backHit = Physics2D.Raycast(transform.position, -transform.right, backRayCastLength, layerMask); // ���, ������� ������� �� ����� ����� � ��������� ��������� ������ �� ������ �����.
        fastAttackHit = Physics2D.Raycast(fastAttackDetector.position, transform.right, fastAttackLength, layerMask); // ���, ������� � ����������� �������� �����, ��� ������������ �����.
    }

    private void ForwardAttacking()
    {
        if (hit.collider != null) // ���� ��� ����� � ���������, ������� ����������� � �������������� � Layer Mask ����. ��������� �������.
        {
            anim.SetBool("isPlayer", true);
            patrolingScript.isAttack = true;
            isReadyFastAttack = false; // ���������� �������� ����, ����� �� ���� ��������� ����� ������ � ������� �����.
            anim.SetBool("isFastAttack", false);
        }
        else if (hit.collider == null)
        {
            patrolingScript.isAttack = false;
            anim.SetBool("isPlayer", false);
            isReadyFastAttack = true; // ��������� ���� �������� ����, ����� �������� �������� ������� ��������� ������� ����. 
        }
        Debug.DrawRay(transform.position, transform.right * rayCastLength, Color.green); // ������������ ����, ������� ������� ������� �����, ��� ������� �����.

    }

    private void IsFastAttacking()
    {
        if (isReadyFastAttack == true) // ��������, ����� �� ���� ������ ��������� ��� ���. �������� �������� �� ����� ����� ������������ � ����������.
        {
            if (fastAttackHit.collider != null)  // �������� ����, ������� ������� ������� �����, ��� ����������� ������. ���� ����� ���������, ����������� �����.
            {
                patrolingScript.isAttack = true;
                anim.SetBool("isFastAttack", true);
            }
            else if (fastAttackHit.collider == null)
            {
                patrolingScript.isAttack = false;
                anim.SetBool("isFastAttack", false);
            }
        }
        Debug.DrawRay(fastAttackDetector.position, transform.right * fastAttackLength, Color.red); // ������������ ����, ������� ������� ������� �����, ��� �����.
    }

    private void IsBackRay()
    {
        if (backHit.collider != null) // �������� ����, ������� ������� �� ����� �����. �� ������� ������ �� ������ �����. 
        {
            if (patrolingScript.isRight == true)
            {
                patrolingScript.isAttack = true;
                patrolingScript.LeftRotate(); // ���� ����� �� ������ �����. � ���� �������� ������. ����� ����������� ������� ����� � ������� ������.
            }
            else if (patrolingScript.isRight != true)
            {
                patrolingScript.isAttack = false;
                patrolingScript.RightRotate(); // ���� ����� �� ������ �����. � ���� �������� �����. ����� ����������� ������� ����� � ������� ������.
            }
        }
        Debug.DrawRay(transform.position, -transform.right * backRayCastLength, Color.yellow); // ������������ ����, ������� ������� ����� �����, ��� ����������� ������.

    }

}
