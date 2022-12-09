
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class Player : MonoBehaviour
{

    [Header("Speed")]
    public float moveSpeed = 5f;
    public float speedIncreaseRate = 0.0001f;
    public float decreaseSpeedRate = 0.05f;
    public float speedRateLimit = 0.1f;
    public float horizontalSpeed = 10f;
    public float speedLimit = 30f;
    public float hitForce = 50f;

    [Header("Movement")]
    public float movementBoundary = 3.5f;
    //public float touchLimit = 20f;
    public GameObject hitParticle;
    public float leanAngle = 10f;
    public float dieTime = 2f;

    [Header("Camera")]
    public float collisionCamDelay = 0.5f;
    public CinemachineVirtualCamera collisionCamera;
    
    bool movable = true;
    bool died = false;
    bool invulnerability = false;

    Vector3 screenPosition;
    float moveDir;
    //Transform initialTransform;

    Rigidbody myBody;

    GameObject sledge;
    //Transform initialSledgeTransform;
    Rigidbody sledgeBody;
    
    ParticleSystem sledgeParticle;
    AudioSource myAudioSource;
    GameManager myGameManager;

    void Start()
    {
        myBody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
        myGameManager =FindObjectOfType<GameManager>();
        //initialTransform = transform;
    }

    public void SetRide(GameObject ride) {
        if(transform.GetComponentInChildren<RideSelection>()) return;
        

        sledge = Instantiate(ride, transform.position, Quaternion.identity);
        sledge.GetComponent<RideSelection>().enabled = false;
        sledge.transform.parent = gameObject.transform;
        sledgeParticle = sledge.GetComponentInChildren<ParticleSystem>();
        sledgeBody = sledge.GetComponent<Rigidbody>();
        //initialSledgeTransform = sledge.transform;
    }

    void Update()
    {
        if(movable){
            movePlayer();
            sledgeParticle.Play();

            if(moveSpeed < speedLimit) {
                moveSpeed += speedIncreaseRate * Time.deltaTime;
                //hitForce += speedIncreaseRate * Time.deltaTime;
                if(speedIncreaseRate > speedRateLimit) 
                    speedIncreaseRate -= decreaseSpeedRate * Time.deltaTime;
            }
        }
        else
        {
            if(sledgeParticle) sledgeParticle.Stop();
        }
    } 

    public bool isMovable
    {
        get 
        {
            return movable;
        }
        set 
        {
            movable = value;
        }
    }

    void movePlayer()
    {
        sledge.transform.rotation = Quaternion.Euler(0,0,0);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);

        #if UNITY_IOS || UNITY_ANDROID
            if(Input.touchCount > 0){
                screenPosition = Input.GetTouch(0).position;

                if(screenPosition.x > Screen.width/2 + touchLimit)
                {
                    MoveRight();
                }
                else if(screenPosition.x < Screen.width/2 - touchLimit)
                {
                    MoveLeft();
                }
            }

        #elif UNITY_STANDALONE || UNITY_WEBGL
            moveDir = Input.GetAxis("Horizontal");
            if(moveDir > 0)
            {
                MoveRight();
            }
            else if (moveDir < 0)
            {
                MoveLeft();
            }

        #endif

    }

    private void MoveLeft()
    {
        if(transform.position.x > -movementBoundary){
            //transform.Translate(Vector3.left * horizontalSpeed * Time.deltaTime);
            transform.position += Vector3.left * horizontalSpeed * Time.deltaTime;
            sledge.transform.rotation = Quaternion.Euler(0, -leanAngle, leanAngle / 2);
        }
        else transform.position = new Vector3(-movementBoundary, transform.position.y, transform.position.z);
    }

    private void MoveRight()
    {
        if(transform.position.x < movementBoundary){
            //transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime);
            transform.position += Vector3.right * horizontalSpeed * Time.deltaTime;
            sledge.transform.rotation = Quaternion.Euler(0, leanAngle, -leanAngle / 2);
        }
        else transform.position = new Vector3(movementBoundary, transform.position.y, transform.position.z);
    }

    void OnCollisionEnter(Collision other)
    {
        HitHandler(other);
    }

    private void HitHandler(Collision other)
    {
        if (other.gameObject.tag == "Obstacles")
        {
            if (invulnerability)
            {
                other.gameObject.GetComponent<Collider>().isTrigger = true;
            }
            else
            {
                other.gameObject.GetComponent<Collider>().isTrigger = false;

                ContactPoint contact = other.contacts[0];


                if(!died)
                {
                    ApplyHitForce(contact);

                    StartCoroutine(setCollisionCam());
                    StartCoroutine(Die());
                }
            }
        }
    }

    private void ApplyHitForce(ContactPoint contact)
    {
        moveSpeed = 0f;

        var force = transform.position - contact.point;
        force.Normalize();

        GameObject particle = Instantiate(hitParticle, contact.point, Quaternion.identity) as GameObject;
        particle.GetComponent<ParticleSystem>().Play();

        sledgeBody.constraints = RigidbodyConstraints.None;
        sledgeBody.AddForceAtPosition(force * hitForce , force, ForceMode.Impulse);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Obstacles" && !invulnerability)
        {
            other.isTrigger = false;
        }
    }

    IEnumerator Die()
    {
        movable = false;
        died = true;
        //AudioSource.PlayClipAtPoint(crashSound, transform.position, PlayerPrefs.GetFloat("SfxVolume", 1f));
        myAudioSource.volume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        myAudioSource.Play();

        yield return new WaitForSeconds(dieTime);
        FindObjectOfType<GameManager>().LoadEndScene();
    }

    IEnumerator setCollisionCam()
    {
        yield return new WaitForSeconds(collisionCamDelay);
        collisionCamera.LookAt = sledge.transform;
        FindObjectOfType<CameraSwitch>().setCollisionCameraActive();

    }

    public GameObject getPlayerSledgeHeadlight()
    {
        return sledge.transform.Find("Headlight").gameObject;
    }
    public GameObject getPlayerSledge()
    {
        return sledge;
    }

    public float getPlayerSpeed()
    {
        return moveSpeed;
    }

    // public void Replay()
    // {
    //     transform.position = initialTransform.position;
    //     transform.rotation = initialTransform.rotation;

    //     sledge.transform.localPosition = initialSledgeTransform.localPosition;
    //     sledge.transform.localRotation = initialSledgeTransform.localRotation;
    //     //Destroy(getPlayerSledge());
    //     //SetRide(sledge);
    // }

    public bool hasDied
    {
        get
        {
            return died;
        }
    }

    public bool isInvulnerable
    {
        get
        {
            return invulnerability;
        }
        set
        {
            invulnerability = value;
        }
    }
}

