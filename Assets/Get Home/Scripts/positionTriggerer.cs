using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum Functionality
{
    MOVELOCK,
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
    public GameObject mainCamera;
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
            case Functionality.MOVELOCK:
                lockPlayer();
                break;
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
        gameManager.GetComponent<ChatManager>().CreateBubble(this.gameObject, trigger.text, 3f);
    }

    // update checkpoint
    void setCheckpoint(float xPosition)
    {
        Vector3 pos = new Vector3(xPosition, -1f, 0f);
        gameManager.GetComponent<GamePause>().updateCheckpoint(pos);
    }

    // lock player's camera & bounds
    void lockPlayer()
    {
        // freeze camera
        mainCamera.GetComponent<CameraMovement>().setFreeze(true);
        // freeze player's position to camera bounds
        float leftBound = mainCamera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        float rightBound = mainCamera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x;
        float width = player.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2;
        player.GetComponent<Boundaries>().Freeze(leftBound+width, rightBound-width);
    }
}
