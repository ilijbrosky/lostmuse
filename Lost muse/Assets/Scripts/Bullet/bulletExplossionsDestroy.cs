using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletExplossionsDestroy : MonoBehaviour
{
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
