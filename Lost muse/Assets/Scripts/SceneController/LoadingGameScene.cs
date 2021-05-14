using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingGameScene : MonoBehaviour
{

    public void PlayFunction() // button
    {
        StartCoroutine(WaitForLoad());
    }

    IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(0.2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
