using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 0.5f;
    public float magnetSpeed = 0.01f;
    public float distanceToRespawn = 1f;
    bool magneted = false;
    bool collected = false;
    Vector3 initialPosition ;

    GameManager myGameManager;
    AudioSource myAudioSource;
    Transform myTransform;
    Player myPlayer;
    void Start()
    {
        myGameManager = FindObjectOfType<GameManager>();
        myAudioSource = GetComponent<AudioSource>();
        myTransform = GetComponent<Transform>();
        myPlayer =FindObjectOfType<Player>();

        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!collected){
            myTransform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);

            if(magneted)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, myPlayer.transform.position, magnetSpeed * Time.deltaTime);

                if(myTransform.position.z < myPlayer.transform.position.z) myTransform.position = myPlayer.transform.position;

            }
        }

        // if(myPlayer.transform.position.z - transform.position.z > distanceToRespawn && !myPlayer.hasDied)
        // {
        //     Respawn();
        // }
    }

    public bool isMagneted
    {
        get{
            return magneted;
        }
        set{
            magneted = value;
        }
    }

    void pickedUp()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        collected = true;
        magneted = false;

        myGameManager.increaseCoinCount();
        myAudioSource.volume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        myAudioSource.Play();
    }

    public void Respawn()
    {
        gameObject.transform.localPosition = initialPosition;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
        collected = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") 
        {
            if(!other.GetComponent<Player>().hasDied){
                pickedUp();
            }
        }
    }
}
