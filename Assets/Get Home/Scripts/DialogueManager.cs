using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [System.Serializable]
    public struct Line
    {
        public Sprite sprite;
        public string text;
    }

    [System.Serializable]
    public struct Conversation
    {
        public float xPosition;
        public string speaker;
        public Line[] lines;
    };

    public GameObject gameManager;
    public GameObject dialogueBox;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI mainText;
    public Image speakerSprite;
    public float textSpeed;

    public Conversation[] conversations;

    // dialogue
    private string speaker;
    private Line[] lines;

    // utility
    private int index;
    private SpriteRenderer spriteObj;
    private bool dialogueOn;
    private float sensibility = 0.25f; // trigger sensibility
    private HashSet<float> pastDialogues = new HashSet<float>();

    // updating text 1 time per frame
    private int frameCount;


    // Start is called before the first frame update
    void Start()
    {
        dialogueOn = false;
        frameCount = Time.frameCount;
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = this.transform.position.x;
        foreach (Conversation conversation in conversations)
        {
            // if we are near conversation's trigger, start it
            if (!pastDialogues.Contains(conversation.xPosition) && Mathf.Abs(xPos - conversation.xPosition) < sensibility)
            {
                pastDialogues.Add(conversation.xPosition);
                StartDialogue(conversation.speaker.ToUpper(), conversation.lines);
            }
        }
        // Continue conversation if one is being run
        if (dialogueOn && Input.anyKeyDown &&
            frameCount < Time.frameCount &&
        !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))) // any key except the mouse activates next dialogue
        {
            frameCount = Time.frameCount; // call at max. 1 per frame

            //sound FX
            if (mainText.text == lines[index].text)
            {
                // finished displaying. Jump to next line
                NextLine();
            }
            else
            {
                // skip text animation & display everything
                StopAllCoroutines();
                mainText.text = lines[index].text;
            }
        }
    }

    public void StartDialogue(string speaker, Line[] lines)
    {
        // setup 
        index = 0;
        this.dialogueOn = true;
        dialogueBox.SetActive(true);
        gameManager.GetComponent<GamePause>().pause(true);

        // dialogue info
        speakerText.text = speaker;
        this.lines = lines;

        // for starting the dialogue
        mainText.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    // type 1 line 1 character at a time (coroutine)
    IEnumerator TypeLine()
    {
        speakerSprite.sprite = lines[index].sprite;
        foreach (char c in lines[index].text.ToCharArray())
        {
            mainText.text += c;
            yield return new WaitForSecondsRealtime(textSpeed); // changed to realtime so that TimeScale doesn't affect on pause
        }
    }

    // move on to next line
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            // still more lines to read
            index++;
            mainText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            // read all lines
            dialogueOn = false;
            dialogueBox.SetActive(false);
            gameManager.GetComponent<GamePause>().pause(false);
        }
    }

    void RunAllDialogues()
    {
        // start all conversations
        foreach (Conversation conversation in conversations)
        {
            if (!pastDialogues.Contains(conversation.xPosition))
            {
                pastDialogues.Add(conversation.xPosition);
                StartDialogue(conversation.speaker.ToUpper(), conversation.lines);
            }
        }
    }

}

