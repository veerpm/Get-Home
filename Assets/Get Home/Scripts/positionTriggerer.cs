﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum Functionality
{
    CHAT,
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
                continue;
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
            case Functionality.CHAT:
                makeChat(trigger);
                break;
            case Functionality.CHECKPOINT:
                setCheckpoint(trigger.xPosition);
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
        float time = 3f;
        gameManager.GetComponent<ChatManager>().CreateBubble(this.gameObject, trigger.text, time);
    }

    // update checkpoint
    void setCheckpoint(float xPosition)
    {
        Vector3 pos = new Vector3(xPosition, -1f, 0f);
        gameManager.GetComponent<GamePause>().updateCheckpoint(pos);
    }
}
