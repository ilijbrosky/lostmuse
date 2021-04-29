
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] private float speed;
    public Vector3 minValues, maxValues;
    public Transform target;


    void Start()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }
    void LateUpdate()
    {
        Vector3 boundaries = new Vector3(
            Mathf.Clamp(target.position.x, minValues.x, maxValues.x),
            Mathf.Clamp(target.position.y, minValues.y, maxValues.y),
            Mathf.Clamp(target.position.z, minValues.z, maxValues.z));

        Vector3 CamPos = Vector3.Lerp(transform.position, boundaries, speed * Time.deltaTime);
        transform.position = CamPos;
    }
}
