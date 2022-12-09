using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CrazyGames;

public class StartSceneManager : MonoBehaviour
{

    public float loadingWaitTime = 1f;
    public Slider progressBar;

    AsyncOperation operation;
    float current = 0;
    float target = 0;
    //bool loaded = false;
    void Start()
    {
        //loaded = false;
        StartCoroutine(loadMainScene());
    }

    // void Update()
    // {
    //     // target = operation.progress / 0.9f;
    //     // current = Mathf.MoveTowards(current, target, Time.deltaTime);

    //     // progressBar.value = current;

    //     // if(Mathf.Approximately(current, 1) && !loaded){
    //     //     StartCoroutine(loadMainScene());
    //     //     loaded = true;
    //     // }
    // }

    IEnumerator loadMainScene()
    {
        operation = SceneManager.LoadSceneAsync("MainScene");
        operation.allowSceneActivation = false;

        while(!Mathf.Approximately(current, 1))
        {
            target = operation.progress / 0.9f;
            current = Mathf.MoveTowards(current, target, Time.deltaTime);

            progressBar.value = current;
            yield return null;
        }

        //yield return new WaitForSeconds(loadingWaitTime);
        operation.allowSceneActivation = true;
    }
}
