using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideSelection : MonoBehaviour
{
    public float animSpeed = 0.1f;
    public int price = 0;
    public string  description;
    Vector3 finalPos = new Vector3();
    Vector3 initialPos = new Vector3();
    Player myPlayer;

    bool unlocked = false;
    // Start is called before the first frame update
    void Awake()
    {
        initialPos = transform.position;
        myPlayer = FindObjectOfType<Player>();
        if(price == 0) isUnlocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        finalPos = myPlayer.transform.position;
        int currentRideChild = 0;
        for(int i=0; i < myPlayer.transform.childCount; i++)
        {
            if(myPlayer.transform.GetChild(i).GetComponent<RideSelection>()) currentRideChild ++;
        }
        if(currentRideChild <= 0){  
            //transform.position = Vector3.Lerp(transform.position, finalPos, animSpeed);
            transform.position = finalPos;
        }
        else
        {
            transform.position = finalPos;
        
            for(int i=0; i < myPlayer.transform.childCount; i++)
            {
                if(myPlayer.transform.GetChild(i).gameObject.GetComponent<RideSelection>()) 
                    Destroy(myPlayer.transform.GetChild(i).gameObject);
            }
        }
    }

    public bool isUnlocked
    {
        get 
        {
            return PlayerPrefs.GetInt(transform.name, 0) == 1;
        }
        set
        {
            unlocked = value;
            if(unlocked) PlayerPrefs.SetInt(transform.name, 1);
        }
    }

    public int Price
    {
        get
        {
            return price;
        
        }
        set
        {
            price = value;
        }
    }

    public string getDescriptionText()
    {
        return description;
    }

    void OnDisable()
    {
        transform.position = initialPos;
    }
}
