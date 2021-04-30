using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroling : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private float distanceVisualRay; // ���������� ��������� ����, ������� ��������� ����.
    [SerializeField] private float distanceToEnd; // ��������� ����, ������� ��������� ����.
    public bool isRight = true;
    public Transform groundDetector;

    void FixedUpdate()
    {
        transform.Translate(Vector2.right * movingSpeed * Time.fixedDeltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, -Vector3.up, distanceToEnd); // ���, ������� ��������� ����, ��� ����������� ���������

        if (groundInfo.collider == false)
        {
            if (isRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0); // �������, ����� ������� �� ���� ���������.
                isRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0); // ������� � ��������� �������, ����� ������� �� ���������������� ���� ���������.
                isRight = true;
            }
        }
        Debug.DrawRay(groundDetector.position, Vector3.down * distanceVisualRay, Color.red); // ������������ ����
    }
}
