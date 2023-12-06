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

    // intro
    private bool introStarted = false;

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
        //PauseMode(true);
        //boss.GetComponent<Animator>().SetTrigger("LandlordEntry");
        //player.GetComponent<DialogueManagerV2>().enabled = false;
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //print(this.GetComponent<GamePause>().dialogueOngoing);
        // wait that intro dialogue finishes to activate back everything
        if (player.GetComponent<DialogueManagerV2>().DialogueOn() && !introStarted)
        {
            PauseMode(false);
            introStarted = true;
        }

        // show ending screen once dialogue is finished
        if (dialogueStarted && !player.GetComponent<DialogueManagerV2>().DialogueOn())
        {
            PauseMode(true);
            endingScreen.SetActive(true);
            StartCoroutine(Credits());
            dialogueStarted = true;
        }
    }

    // dialogue when boss enters
    public void StartIntroDialogue()
    {
        PauseMode(false);
        player.GetComponent<DialogueManagerV2>().enabled = true;
    }

    // final dialogue starts when boss dies
    public void StartEndingDialogue()
    {
        player.GetComponent<DialogueManagerV2>().StartDialogue(lines);
        dialogueStarted = true;
    }

    public void PauseMode(bool pauseActivated)
    {
        //pause everything
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            // deactivate all enemies
            if(enemy.name != "Landlord")
            {
                enemy.SetActive(!pauseActivated);
            }
        }

        // disable chat boxes, music and player's controls & sounds
        this.GetComponent<ChatManager>().enabled = !pauseActivated;
        player.GetComponent<PlayerMovement>().enabled = !pauseActivated;
        gameMusic.SetActive(!pauseActivated);
        AudioSource[] sounds = player.GetComponents<AudioSource>();
        foreach(AudioSource sound in sounds)
        {
            sound.enabled = !pauseActivated;
        }
        // stop boss from doing anything funky
        boss.GetComponent<BossBehaviour>().started = !pauseActivated;
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

        yield return new WaitForSeconds(1);

        // shortcut to one of the element we are moving up
        RectTransform rectT = endingText.GetComponent<RectTransform>();
        float elementTop = rectT.anchoredPosition.y - rectT.rect.height * (1 - rectT.pivot.y);

        // scroll until all the text has passed the screen
        while (elementTop < mainCanvas.GetComponent<RectTransform>().rect.yMax)
        {
            Vector3 textPos = endingText.GetComponent<RectTransform>().anchoredPosition;
            Vector3 titlePos = endingTitle.GetComponent<RectTransform>().anchoredPosition;

            // move up
            endingText.GetComponent<RectTransform>().anchoredPosition = new Vector3(textPos.x, textPos.y +raiseSpeed * Time.deltaTime, textPos.z);
            endingTitle.GetComponent<RectTransform>().anchoredPosition = new Vector3(titlePos.x, titlePos.y + raiseSpeed * Time.deltaTime, titlePos.z);
            
            // update position
            elementTop = rectT.anchoredPosition.y - rectT.rect.height * (1 - rectT.pivot.y);

            yield return null;
        }

        // change scene once screen is black
        SceneManager.LoadScene("Main Menu");
    }
}
