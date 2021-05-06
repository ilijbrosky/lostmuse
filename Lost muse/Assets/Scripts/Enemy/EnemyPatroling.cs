using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroling : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private float fastAttackMovingSpeed; // скорость врага во время атаки
    [SerializeField] private float distanceVisualRay; // Визуальная дистанция луча, который направлен вниз.
    [SerializeField] private float distanceToEnd; // Дистанция луча, который направлен вниз.
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
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, -Vector3.up, distanceToEnd, layerMask); // Луч, который направлен вниз, для обнаружения платформы

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
        Debug.DrawRay(groundDetector.position, Vector3.down * distanceVisualRay, Color.red); // Вызуализация луча
    }

    public void LeftRotate()
    {
        transform.eulerAngles = new Vector3(0, -180, 0); // Поворот, когда доходит до края платформы.
        isRight = false;
    }

    public void RightRotate()
    {
        transform.eulerAngles = new Vector3(0, 0, 0); // Поворот в начальную позицию, когда доходит до противоположного края платформы.
        isRight = true;
    }

    public void Calm()
    {
        transform.Translate(Vector2.right * movingSpeed * Time.fixedDeltaTime); // Движение энеми в спокойном состоянии 
    }

    public void FastAttacking()
    {
        transform.Translate(Vector2.right * fastAttackMovingSpeed * Time.fixedDeltaTime); // Движение энеми во время атаки, контролируется переменной "attackingMovingSpeed"
    }

}
