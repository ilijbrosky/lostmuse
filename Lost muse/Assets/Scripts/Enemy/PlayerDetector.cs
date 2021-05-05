
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
    private Animator anim;
    RaycastHit2D hit, backHit, fastAttackHit;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, rayCastLength, layerMask); // ���, ������� ������� � ����������� �������� �����. ��������� �� ���� ������������� � Layer Mask. 
        backHit = Physics2D.Raycast(transform.position, -transform.right, backRayCastLength, layerMask); // ���, ������� ������� �� ����� ����� � ��������� ��������� ������ �� ������ �����.
        fastAttackHit = Physics2D.Raycast(fastAttackDetector.position, transform.right, fastAttackLength, layerMask); // ���, ������� � ����������� �������� �����, ��� ������������ �����.


        if (hit.collider != null) // ���� ��� ����� � ���������, ������� ����������� � �������������� � Layer Mask ����. ��������� �������.
        {
            anim.SetBool("isPlayer", true);
            patrolingScript.Calm(); // �������� ����� �����, ������� ��������� � ������� EnemyPatroling 
            patrolingScript.isAttack = true;
            isReadyFastAttack = false; // ���������� �������� ����, ����� �� ���� ��������� ����� ������ � ������� �����.
            anim.SetBool("isFastAttack", false);
        }
        else if(hit.collider == null) 
        {
            anim.SetBool("isPlayer", false);
            isReadyFastAttack = true; // ��������� ���� �������� ����, ����� �������� �������� ������� ��������� ������� ����. 
            patrolingScript.isAttack = false;
        }
        if(backHit.collider != null) // �������� ����, ������� ������� �� ����� �����. �� ������� ������ �� ������ �����. 
        {
            if(patrolingScript.isRight == true)
            {
                patrolingScript.LeftRotate(); // ���� ����� �� ������ �����. � ���� �������� ������. ����� ����������� ������� ����� � ������� ������.
                patrolingScript.FastAttacking(); // �������� ����� �����, ������� ��������� � ������� EnemyPatroling 
                patrolingScript.isAttack = true;
            }
            else if(patrolingScript.isRight != true)
            {
                patrolingScript.RightRotate(); // ���� ����� �� ������ �����. � ���� �������� �����. ����� ����������� ������� ����� � ������� ������.
                patrolingScript.FastAttacking(); // �������� ����� �����, ������� ��������� � ������� EnemyPatroling 
                patrolingScript.isAttack = true;
            }
        }
        if(isReadyFastAttack == true) // ��������, ����� �� ���� ������ ��������� ��� ���. �������� �������� �� ����� ����� ������������ � ����������.
        {
            if (fastAttackHit.collider != null)  // �������� ����, ������� ������� ������� �����, ��� ����������� ������. ���� ����� ���������, ����������� �����.
            {
                patrolingScript.FastAttacking();
                anim.SetBool("isFastAttack", true);
            }
            else if (fastAttackHit.collider == null)
            {
                anim.SetBool("isFastAttack", false);
            }
        }
        Debug.DrawRay(transform.position, transform.right * rayCastLength, Color.green); // ������������ ����, ������� ������� ������� �����, ��� ������� �����.
        Debug.DrawRay(transform.position, -transform.right * backRayCastLength, Color.yellow); // ������������ ����, ������� ������� ����� �����, ��� ����������� ������.
        Debug.DrawRay(fastAttackDetector.position, transform.right * fastAttackLength, Color.red); // ������������ ����, ������� ������� ������� �����, ��� �����.
    }
}
