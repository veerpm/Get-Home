using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject canvas;
    public GameObject levelNameObject;
    public string levelName;
    public float acceleration;

    public GameObject player;


    void Start()
    {
        // sets volume preferences
        if (PlayerPrefs.HasKey("Volume"))
        {
            audioSource.volume = PlayerPrefs.GetFloat("Volume");
        }

        // load player
        //LoadPosition();

        // sets the animation when entering new level
        levelNameObject.SetActive(true);
        levelNameObject.GetComponent<TextMeshProUGUI>().text = "<i>" + levelName + "</i>";

        StartCoroutine(levelNameAnimation());
    }

    IEnumerator levelNameAnimation()
    {
        // move text while it's not outside of UI
        // Note: speeds up dramatically
        while (-500f < levelNameObject.transform.position.x)
        {
            float xPos = levelNameObject.transform.position.x;
            float newXPos = xPos - Mathf.Pow(acceleration * Time.deltaTime,2);
            levelNameObject.transform.position = new Vector3(newXPos, levelNameObject.transform.position.y, 0f);
            acceleration += 3;
            yield return null;
        }
        levelNameObject.SetActive(false);

    }

    void LoadPosition()
    {
        float XPos = -5.34f;
        float YPos = 0f;
        
        if (PlayerPrefs.HasKey("XPos"))
        {
            XPos = PlayerPrefs.GetFloat("XPos");
        }
        if (PlayerPrefs.HasKey("YPos"))
        {
            XPos = PlayerPrefs.GetFloat("YPos");
        }
        
        player.transform.position = new Vector3(XPos, YPos, 0f);
    }

}
