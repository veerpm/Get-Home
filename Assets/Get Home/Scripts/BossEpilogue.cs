using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class BossEpilogue : MonoBehaviour
{
    public GameObject boss;
    public GameObject player;
    public GameObject gameMusic;
    public DialogueManagerV2.Line[] lines;

    // credits
    public GameObject endingScreen;
    public TextMeshProUGUI endingTitle;
    public GameObject endingText;
    public GameObject mainCanvas;
    public float fadeSpeed;
    public float raiseSpeed;


    private bool dialogueStarted = false;
    
    private void Awake()
    {
        //IntroCutscene();
    }
    
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // start epilogue dialogue
        if (!dialogueStarted && boss.GetComponent<EnemyHealth>().IsDead())
        {
            //StartCoroutine("Wait");
            dialogueStarted = true;
            player.GetComponent<DialogueManagerV2>().StartDialogue(lines);
        }

        /*
        // show ending screen once dialogue is finished
        if (dialogueStarted && !player.GetComponent<DialogueManagerV2>().DialogueOn())
        {
            this.GetComponent<GamePause>().pause(true);
            endingScreen.SetActive(true);
            StartCoroutine(Credits());
        }
        */
    }

    private void IntroCutscene()
    {
        //pause everything
        this.GetComponent<GamePause>().pause(true);
        this.GetComponent<ChatManager>().enabled = false;
        player.GetComponent<DialogueManagerV2>().enabled = false;
        gameMusic.SetActive(false);
        boss.GetComponent<Animator>().SetTrigger("LandlordEntry");
    }

    IEnumerator Credits()
    {
        Color screenColor = endingScreen.GetComponent<Image>().color;
        Color textColor = endingTitle.color;
        float fadeAmount;

        // set back to transparent first
        endingScreen.GetComponent<Image>().color = new Color(screenColor.r, screenColor.g, screenColor.b, 0f);
        endingTitle.color = new Color(textColor.r, textColor.g, textColor.b, 0f);

        // fade to black
        while (endingScreen.GetComponent<Image>().color.a < 1 && endingTitle.color.a < 1)
        {
            fadeAmount = endingTitle.color.a + (fadeSpeed * Time.deltaTime);

            endingScreen.GetComponent<Image>().color = new Color(screenColor.r, screenColor.g, screenColor.b, fadeAmount);
            endingTitle.color = new Color(textColor.r, textColor.g, textColor.b, fadeAmount);
            yield return null;
        }

        while(endingText.GetComponent<RectTransform>().rect.yMin < mainCanvas.GetComponent<RectTransform>().rect.yMax)
        {
            Vector3 textPos = endingText.GetComponent<RectTransform>().anchoredPosition;
            Vector3 titlePos = endingTitle.GetComponent<RectTransform>().anchoredPosition;

            endingText.GetComponent<RectTransform>().anchoredPosition = new Vector3(textPos.x, textPos.y +raiseSpeed * Time.deltaTime, textPos.z);
            endingTitle.GetComponent<RectTransform>().anchoredPosition = new Vector3(titlePos.x, titlePos.y + raiseSpeed * Time.deltaTime, titlePos.z);
        }

        // change scene once screen is black
        SceneManager.LoadScene("Main Menu");
    }
}
