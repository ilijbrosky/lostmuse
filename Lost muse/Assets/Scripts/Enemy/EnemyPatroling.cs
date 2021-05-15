using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroling : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private float fastAttackMovingSpeed; // скорость врага во время атаки
    [SerializeField] private float distanceVisualRay; // Визуальная дистанция луча, который направлен вниз.
    [SerializeField] private float distanceToEnd; // Дистанция луча, который направлен вниз.
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
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, -Vector3.up, distanceToEnd, layerMask); // Луч, который направлен вниз, для обнаружения платформы

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
        Debug.DrawRay(groundDetector.position, Vector3.down * distanceVisualRay, Color.red); // Вызуализация луча

        FreezeEnemyPositon();
    }

    private void FreezeEnemyPositon()
    {
        if (colliderTouch.IsTouchingLayers(player))
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll; // заморозка по трём позициям, когда коллайдер врага соприкасается с игроком.
        }
        else
        {
            rb2d.constraints = RigidbodyConstraints2D.None; // снятие заморозки по позициям.
            rb2d.freezeRotation = true; // отключение возможности прокрутки врага в RigidBody
        }
    }



    public void LeftRotate()
    {
        if (isMoving && isCanRotate)
        {
            transform.eulerAngles = new Vector3(0, -180, 0); // Поворот, когда доходит до края платформы.
            isRight = false;
        }
    }

    public void RightRotate()
    {
        if (isMoving && isCanRotate)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Поворот в начальную позицию, когда доходит до противоположного края платформы.
            isRight = true;
        }
    }

    public void Calm()
    {
        if(isGround == true)
        {
            if (isMoving == true)
            {
                transform.Translate(Vector2.right * movingSpeed * Time.fixedDeltaTime); // Движение энеми в спокойном состоянии 
            }
        }
    }

    public void FastAttacking()
    {
        if (isGround == true)
        {
            if (isMoving == true)
            {
                transform.Translate(Vector2.right * fastAttackMovingSpeed * Time.fixedDeltaTime); // Движение энеми во время атаки, контролируется переменной "attackingMovingSpeed"
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
