using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float frequency;
    [SerializeField] private float magnitude;
    [SerializeField] private float distanceVisualRay; // ���������� ��������� ����, ������� ��������� ����.
    [SerializeField] private float distanceToEnd; // ��������� ����, ������� ��������� ����.
    public bool isRight = true, lookRight = true;
    Vector3 pos;
    public Transform groundDetector;

    void Start()
    {
        pos = transform.position;
    }

    void FixedUpdate()
    {
        //transform.Translate(Vector2.right * movingSpeed * Time.fixedDeltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, -Vector3.up, distanceToEnd); // ���, ������� ��������� ����, ��� ����������� ���������

        if (groundInfo.collider == false)
        {
            if (isRight == true)
            {
                //transform.eulerAngles = new Vector3(0, -180, 0); // �������, ����� ������� �� ���� ���������.
                isRight = false;
                pos -= transform.right * Time.deltaTime * moveSpeed;
                transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
            }
            else
            {
               // transform.eulerAngles = new Vector3(0, 0, 0); // ������� � ��������� �������, ����� ������� �� ���������������� ���� ���������.
                isRight = true;
                pos += transform.right * Time.deltaTime * moveSpeed;
                transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
                
            }
        }
        Debug.DrawRay(groundDetector.position, Vector3.down * distanceVisualRay, Color.red); // ������������ ����
    }




    private void MoveRight()
    {
        pos += transform.right * Time.deltaTime * moveSpeed;
        transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
    }

    private void MoveLeft()
    {
        pos -= transform.right * Time.deltaTime * moveSpeed;
        transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
    }

}
