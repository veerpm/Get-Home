using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public GameObject dialogueBox;
    public float textSpeed;

    private string speaker;
    private string[] lines;

    private int index;
    private TextMeshProUGUI textObj;
    private bool dialogueOn;

    // Start is called before the first frame update
    void Start()
    {
        textObj = dialogueBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        dialogueOn = false;

        // testing
        string[] lines = { "What a night...", "And I still need to get home!" };
        StartDialogue(lines, "Randy");
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueOn && Input.anyKeyDown)
        {
            if(textObj.text == speaker + lines[index])
            {
                // finished displaying. Jump to next line
                NextLine();
            }
            else
            {
                // skip text animation & display everything
                StopAllCoroutines();
                textObj.text = speaker + lines[index];
            }
        }
    }

    void StartDialogue(string[] lines, string speaker)
    {
        // setup 
        index = 0;
        dialogueOn = true;
        dialogueBox.SetActive(true);
        GetComponent<GamePause>().pause(true);

        // dialogue info
        this.speaker = "<b>" + speaker + "</b>: ";
        this.lines = lines;

        // for starting the dialogue
        textObj.text = string.Empty + this.speaker;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textObj.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < lines.Length - 1)
        {
            // still more lines to read
            index++;
            textObj.text = string.Empty + speaker;
            StartCoroutine(TypeLine());
        }
        else
        {
            // read all lines
            dialogueOn = false;
            dialogueBox.SetActive(false);
            GetComponent<GamePause>().pause(false);
        }
    }
}

