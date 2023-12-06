using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class BossEpilogue : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossClone;
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
   
    public bool creditsLaunched = false;
    private bool endingDialogueStarted = false;
    private GameObject[] enemies;

    private void Awake()
    {
        // get all enemies in scene
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // intro
        BeginningCinematic();
    }

    // Update is called once per frame
    void Update()
    {
        // wait that intro dialogue finishes to activate back everything
        if (player.GetComponent<DialogueManagerV2>().DialogueOn() && !introStarted)
        {
            print("activated!");
            PauseMode(false);
            introStarted = true;
        }

        // show ending screen once dialogue is finished
        if (endingDialogueStarted && !player.GetComponent<DialogueManagerV2>().DialogueOn() && !creditsLaunched)
        {
            PauseMode(true);
            endingScreen.SetActive(true);
            StartCoroutine(Credits());
            creditsLaunched = true;
        }
    }

    // immediately called at beginning of level (for boss' entry)
    public void BeginningCinematic()
    {
        PauseMode(true);
        CutAllSounds(true);
        boss.GetComponent<Animator>().SetTrigger("LandlordEntry");
        player.GetComponent<DialogueManagerV2>().enabled = false;

        // start animation
        //boss.GetComponent<SpriteRenderer>().enabled = false;
        boss.SetActive(false);
        bossClone.SetActive(true);
    }

    // dialogue when boss enters
    public void StartIntroDialogue()
    {
        // switch to real boss
        //boss.GetComponent<SpriteRenderer>().enabled = true;
        boss.SetActive(true);
        bossClone.SetActive(false);

        CutAllSounds(false);
        player.GetComponent<DialogueManagerV2>().enabled = true;
    }

    // when boss dies
    public void BossDieCinematic()
    {
        CutAllSounds(true);
        PauseMode(true);
    }

    // final dialogue starts when boss dies
    public void StartEndingDialogue()
    {
        print("started ending");
        player.GetComponent<DialogueManagerV2>().StartDialogue(lines);
        endingDialogueStarted = true;
    }

    // deactivate moving elements of game
    public void PauseMode(bool pauseActivated)
    {
        //pause everything
        print("enemies!");
        foreach(GameObject enemy in enemies)
        {
            print(enemy);
            // deactivate all enemies
            if(enemy.name != "Landlord")
            {
                enemy.SetActive(!pauseActivated);
            }
        }

        // disable chat boxes, music and player's controls
        this.GetComponent<ChatManager>().enabled = !pauseActivated;
        player.GetComponent<PlayerMovement>().enabled = !pauseActivated;

        // stop boss from doing anything funky
        boss.GetComponent<BossBehaviour>().started = !pauseActivated;
    }

    // deactivate audio sources of game
    private void CutAllSounds(bool activated)
    {
        gameMusic.SetActive(!activated);
        AudioSource[] sounds = player.GetComponents<AudioSource>();
        foreach (AudioSource sound in sounds)
        {
            sound.enabled = !activated;
        }
    }

    // ending
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
