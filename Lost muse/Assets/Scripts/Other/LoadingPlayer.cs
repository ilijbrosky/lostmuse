using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject LoadingEffect;


    private void Awake() // button
    {
        StartCoroutine(WaitForLoad());
    }

    IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(0.6f);
        LoadingEffect.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        player.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        LoadingEffect.SetActive(false);
    }
}
