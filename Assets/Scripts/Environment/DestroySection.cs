using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySection : MonoBehaviour
{
    public float timeToDestroy = 0.5f;
    public float distanceToDestroy = 100f;

    GenerateLevel levelGenerator;

    void Start()
    {
        levelGenerator = FindObjectOfType<GenerateLevel>();
    }

    void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            StartCoroutine(destroySection(other.gameObject));
        }
    }

    IEnumerator destroySection(GameObject player)
    {
        while(gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(timeToDestroy);
            if(Vector3.Distance(transform.position, player.transform.position) > distanceToDestroy)
            { 
                gameObject.SetActive(false);
                levelGenerator.generatePlayground();
            }
        }
    }

    void OnDisable()
    {
        Coin[] coins = GetComponentsInChildren<Coin>();

        if(coins.Length > 0){
            foreach(var coin in coins)
            {
                coin.Respawn();
            }
        }

        MovingObstacle[] movingObs = GetComponentsInChildren<MovingObstacle>();

        if(movingObs.Length > 0) {
            foreach(var obs in movingObs)
            {
                obs.Respawn();
            }
        }
    }
}
