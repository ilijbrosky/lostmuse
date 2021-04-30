using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroling : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private float distanceVisualRay; // Визуальная дистанция луча, который направлен вниз.
    [SerializeField] private float distanceToEnd; // Дистанция луча, который направлен вниз.
    public bool isRight = true;
    public Transform groundDetector;

    void FixedUpdate()
    {
        transform.Translate(Vector2.right * movingSpeed * Time.fixedDeltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, -Vector3.up, distanceToEnd); // Луч, который направлен вниз, для обнаружения платформы

        if (groundInfo.collider == false)
        {
            if (isRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0); // Поворот, когда доходит до края платформы.
                isRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0); // Поворот в начальную позицию, когда доходит до противоположного края платформы.
                isRight = true;
            }
        }
        Debug.DrawRay(groundDetector.position, Vector3.down * distanceVisualRay, Color.red); // Вызуализация луча
    }
}
