using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum Functionality
{
    MOVELOCK,
    CHAT,
    DIALOGUE,
    CHECKPOINT
};

[System.Serializable]
public struct Trigger
{
    public float xPosition;
    public Functionality function;
    public bool repeats;
    public string text;
}

public class positionTriggerer : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject player;
    public List<Trigger> triggers;

    private float sensibility = 0.25f;
    private List<Trigger> pastTriggers = new List<Trigger>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = this.transform.position.x;
        foreach (Trigger trigger in triggers)
        {
            // if trigger was already used, pass it
            if (pastTriggers.Contains(trigger))
            {
                break;
            }

            // if we are near trigger's position, activate it
            if(Mathf.Abs(xPos-trigger.xPosition) < sensibility)
            {
                activateTrigger(trigger);
            }
        }
    }

    void activateTrigger(Trigger trigger)
    {
        Functionality func = trigger.function;

        switch (func)
        {
            case Functionality.MOVELOCK:
                break;
            case Functionality.CHAT:
                makeChat(trigger);
                break;
            case Functionality.DIALOGUE:
                makeDialogue(trigger);
                break;
        }

        // won't play trigger again if it doesn't repeats
        if (!trigger.repeats)
        {
            pastTriggers.Add(trigger);
        }
    }

    // create chat bubble
    void makeChat(Trigger trigger)
    {
        gameManager.GetComponent<ChatManager>().CreateBubble(this.gameObject, trigger.text, 3f);
    }

    // create dialogue boxes
    void makeDialogue(Trigger trigger)
    {
        //string[] dialogues= trigger.text.Split('\n');
        string[] dialogues = { trigger.text };
        gameManager.GetComponent<DialogueManager>().StartDialogue(dialogues, "Randy");
    }
}
