using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject playgrounds;
    public float spawnSpacing = 200f;
    float zPos = 50f;
    float initialZPos = 50f;
    public float generateTime = 0.1f;
    public int sectionsLimit = 3;
    private int previousSection = -1;
    private int secNum = 0;

    DestroySection[] sections;

    Transform playGroundTransform;

    void Start()
    {
        playGroundTransform = playgrounds.GetComponent<Transform>();
        startGroundSpawn();
    }

    public void startGroundSpawn()
    {
        sections = playGroundTransform.GetComponentsInChildren<DestroySection>();
        foreach(var sec in sections)
        {
            sec.gameObject.SetActive(false);
        }

        generatePlayground();
    }


    // Update is called once per frame
    // void Update()
    // {
    //     StartCoroutine(GenerateSection());
    // }

    // IEnumerator  GenerateSection()
    // {
    //     sections = FindObjectsOfType<DestroySection>();
    //     if(sections.Length < sectionsLimit)
    //     {
    //         begin:
    //         int secNum = Random.Range(0, ground.Length);    
    //         if(secNum == previousSection)
    //         {
    //             goto begin;
    //         }
    //         previousSection = secNum;
            
    //         GameObject newSection = Instantiate<GameObject>(ground[secNum], new Vector3(0, 0, zPos), Quaternion.identity);
    //         zPos += spawnSpacing;
    //         yield return new WaitForSeconds(generateTime);
    //     }
    // }

    public void generatePlayground()
    {
        sections = playGroundTransform.GetComponentsInChildren<DestroySection>();
        while(sections.Length < sectionsLimit)
        {
            begin:
            secNum = Random.Range(0, playGroundTransform.childCount);    
            if(playGroundTransform.GetChild(secNum).gameObject.activeInHierarchy) goto begin;
            
            playGroundTransform.GetChild(secNum).gameObject.transform.position = new Vector3(0, 0, zPos);
            playGroundTransform.GetChild(secNum).gameObject.SetActive(true);
            zPos += spawnSpacing;

            sections = playGroundTransform.GetComponentsInChildren<DestroySection>();
        }
    }

    public void ResetGround()
    {
        zPos = initialZPos;

        foreach(var sec in sections)
        {
            sec.gameObject.SetActive(false);
        }

        generatePlayground();
    }
}
