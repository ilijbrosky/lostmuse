using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroling : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private float fastAttackMovingSpeed; // �������� ����� �� ����� �����
    [SerializeField] private float distanceVisualRay; // ���������� ��������� ����, ������� ��������� ����.
    [SerializeField] private float distanceToEnd; // ��������� ����, ������� ��������� ����.
    public bool isAttack = false;
    public bool isRight = true;
    public Transform groundDetector;
    public LayerMask layerMask;

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
            if (isRight == true)
            {
                LeftRotate();
            }
            else
            {
                RightRotate();
            }
        }
        Debug.DrawRay(groundDetector.position, Vector3.down * distanceVisualRay, Color.red); // ������������ ����
    }

    public void LeftRotate()
    {
        transform.eulerAngles = new Vector3(0, -180, 0); // �������, ����� ������� �� ���� ���������.
        isRight = false;
    }

    public void RightRotate()
    {
        transform.eulerAngles = new Vector3(0, 0, 0); // ������� � ��������� �������, ����� ������� �� ���������������� ���� ���������.
        isRight = true;
    }

    public void Calm()
    {
        transform.Translate(Vector2.right * movingSpeed * Time.fixedDeltaTime); // �������� ����� � ��������� ��������� 
    }

    public void FastAttacking()
    {
        transform.Translate(Vector2.right * fastAttackMovingSpeed * Time.fixedDeltaTime); // �������� ����� �� ����� �����, �������������� ���������� "attackingMovingSpeed"
    }

}
