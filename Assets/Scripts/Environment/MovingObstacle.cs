using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float moveDistance = 30f;
    public float respawnTime = 5f;
    
    public GameObject endPoint;
    Player myPlayer;
    PlayerData myPlayerData;
    GameObject Object;
    GameManager myGameManager;
    bool spawned = false;
    bool collided = false;
    Vector3 initialPos;
    Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = FindObjectOfType<Player>();
        myPlayerData = FindObjectOfType<PlayerData>();
        myGameManager = FindObjectOfType<GameManager>();
        myTransform = GetComponent<Transform>();

        initialPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(myGameManager.GameHasStarted && !spawned)
        {
            Object = Instantiate(myPlayer.getPlayerSledge(), myTransform.position, myTransform.rotation);
            Object.transform.Rotate(90 * Vector3.up);
            Object.transform.parent = myTransform;
            spawned = true;

            if(!myPlayerData.isDayTime) 
                Destroy(Object.gameObject.GetComponentInChildren<Light>().gameObject);
        }

        if(Mathf.Abs(myTransform.position.z - myPlayer.transform.position.z) < moveDistance && !collided)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, endPoint.transform.position, myPlayer.getPlayerSpeed() * Time.deltaTime);
        }


    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<Player>() && !other.gameObject.GetComponent<Player>().isInvulnerable)
        {
            collided = true;
        }
    }

    public void Respawn()
    {
        //myTransform.position = initialPos;
        transform.localPosition = initialPos;
    }
}
