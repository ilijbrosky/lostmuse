
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{

    [SerializeField] private float rayCastLength;
    [SerializeField] private float backRayCastLength;
    [SerializeField] private float fastAttackLength;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Animator anim;
    public EnemyPatroling patrolingScript;
    public Transform fastAttackDetector;
    RaycastHit2D hit, backHit, fastAttackHit;
    private void Update()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, rayCastLength, layerMask); // Лук, который выпущен в направлении движения врага. Реагирует на Слой установленный в Layer Mask. 
        backHit = Physics2D.Raycast(transform.position, -transform.right, backRayCastLength, layerMask);
        fastAttackHit = Physics2D.Raycast(fastAttackDetector.position, transform.right, fastAttackLength, layerMask);


        if (hit.collider != null) // Если лук попал в коллайдер, который принадлежит к установленному в Layer Mask слою. Выполняет условие.
        {
            anim.SetBool("isPlayer", true);
            patrolingScript.Attacking(); // Вызывает метод атаки, который находится в скрипте EnemyPatroling 
            patrolingScript.isAttack = true;
            fastAttackLength = 0f;
        }
        else if(hit.collider == null)
        {
            anim.SetBool("isPlayer", false);
            patrolingScript.isAttack = false;
            fastAttackLength = 1f;
        }
        if(backHit.collider != null)
        {
            if(patrolingScript.isRight == true)
            {
                patrolingScript.LeftRotate();
                patrolingScript.Attacking(); // Вызывает метод атаки, который находится в скрипте EnemyPatroling 
                patrolingScript.isAttack = true;
            }
            else if(patrolingScript.isRight != true)
            {
                patrolingScript.RightRotate();
                patrolingScript.Attacking(); // Вызывает метод атаки, который находится в скрипте EnemyPatroling 
                patrolingScript.isAttack = true;
            }
        }
        if (fastAttackHit.collider != null)
        {
            patrolingScript.FastAttacking();
            anim.SetBool("isFastAttack", true);
        }
        else if(fastAttackHit.collider == null)
        {
            anim.SetBool("isFastAttack", false);
        }

        Debug.DrawRay(transform.position, transform.right * rayCastLength, Color.yellow); // Вызуализация луча
        Debug.DrawRay(transform.position, -transform.right * backRayCastLength, Color.yellow); // Вызуализация луча
        Debug.DrawRay(fastAttackDetector.position, transform.right * fastAttackLength, Color.red); // Вызуализация луча
    }
}
