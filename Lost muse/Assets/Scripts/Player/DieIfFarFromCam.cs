using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieIfFarFromCam : MonoBehaviour
{
    private Transform Cam;
    void Start()
    {
        Cam = GameObject.Find("Main Camera").transform;
    }

    void Update()
    {
        if (gameObject.transform.position.y + 5 < Cam.position.y)
        {
            DieFunction();
        }
    }

    public void DieFunction()
    {
        StartCoroutine(WaitForLoad());
    }
    IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(0.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
