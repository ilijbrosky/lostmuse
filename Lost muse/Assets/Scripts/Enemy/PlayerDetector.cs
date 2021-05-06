
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{

    [SerializeField] private float rayCastLength; // Дальность для срабатывания ближней атаки врага.
    [SerializeField] private float backRayCastLength; // Дальность вычесления игрока, если он появляется за спиной врага
    [SerializeField] private float fastAttackLength; // Дальность срабатывания рывка для атаки.
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
        hit = Physics2D.Raycast(transform.position, transform.right, rayCastLength, layerMask); // Луч, который выпущен в направлении движения врага. Реагирует на Слой установленный в Layer Mask. 
        backHit = Physics2D.Raycast(transform.position, -transform.right, backRayCastLength, layerMask); // Луч, который выпущен со спины врага и проверяет наявность игрока за спиной врага.
        fastAttackHit = Physics2D.Raycast(fastAttackDetector.position, transform.right, fastAttackLength, layerMask); // Луч, выпущен в направлении движения врага, для срабатывания рывка.


        if (hit.collider != null) // Если луч попал в коллайдер, который принадлежит к установленному в Layer Mask слою. Выполняет условие.
        {
            anim.SetBool("isPlayer", true);
            patrolingScript.isAttack = true;
            isReadyFastAttack = false; // Выключение красного луча, чтобы не было конфликта между зелёным и красным лучом.
            anim.SetBool("isFastAttack", false);
        }
        else if(hit.collider == null) 
        {
            patrolingScript.isAttack = false;
            anim.SetBool("isPlayer", false);
            isReadyFastAttack = true; // Включение луча красного луча, когда персонаж покидает область видимости зелёного луча. 
            patrolingScript.isAttack = false;
        }
        if(backHit.collider != null) // Проверка луча, который выпущен со спины врага. На наличие игрока за спиной врага. 
        {
            if(patrolingScript.isRight == true)
            {
                patrolingScript.isAttack = true;
                patrolingScript.LeftRotate(); // Если игрок за спиной врага. И враг движется вправо. Здесь срабатывает поворот врага в сторону игрока.
            }
            else if(patrolingScript.isRight != true)
            {
                patrolingScript.isAttack = false;
                patrolingScript.RightRotate(); // Если игрок за спиной врага. И враг движется влево. Здесь срабатывает поворот врага в сторону игрока.
            }
        }
        if(isReadyFastAttack == true) // Проверка, может ли враг быстро атаковать или нет. Скорость движения во время атаки регулируется в инспекторе.
        {
            if (fastAttackHit.collider != null)  // Проверка луча, который выпущен впереди врага, для обнаружения игрока. Если игрок обнаружен, срабатывает рывок.
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
        Debug.DrawRay(transform.position, transform.right * rayCastLength, Color.green); // Вызуализация луча, который выпущен впереди врага, для обычной атаки.
        Debug.DrawRay(transform.position, -transform.right * backRayCastLength, Color.yellow); // Вызуализация луча, который выпущен сзади врага, для обнаружения игрока.
        Debug.DrawRay(fastAttackDetector.position, transform.right * fastAttackLength, Color.red); // Вызуализация луча, который выпущен впереди врага, для рывка.
    }
}
