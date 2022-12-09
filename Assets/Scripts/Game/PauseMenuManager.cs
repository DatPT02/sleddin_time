using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseContent;
    public GameObject tutorialContent;
    public Image tutorialButtonImage;
    public Sprite tutorialSprite;
    public Sprite closeTutorialSprite;

    bool tutorialOpened = false;
    // void Start()
    // {
    //     pauseContent.SetActive(true);
    //     tutorialContent.SetActive(false);
    // }
    void OnEnable()
    {
        pauseContent.SetActive(true);
        tutorialContent.SetActive(false);
    }
    void Update()
    {
        tutorialOpened = tutorialContent.activeInHierarchy;
    }
    
    public void Tutorial()
    {
        if(!tutorialOpened){
            pauseContent.SetActive(false);
            tutorialContent.SetActive(true);
            tutorialButtonImage.sprite = closeTutorialSprite;
        }
        else
        {
            pauseContent.SetActive(true);
            tutorialContent.SetActive(false);
            tutorialButtonImage.sprite = tutorialSprite;
        }
    }
}
