using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject gameManager;

    private GameObject chatBubble;

    private bool moved = false;
    private bool punched = false;
    private bool pickedUp = false;
    private bool paused = false;

    void Start()
    {
        StartCoroutine(tutorialLogic());
    }

    private void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0|| Input.GetAxisRaw("Vertical") != 0)
        {
            moved = true;
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q))
        {
            punched = true;
        }


        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.R))
        {
            pickedUp = true;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = true;
        }
    }

    IEnumerator tutorialLogic()
    {
        // freeeze player
        this.GetComponent<LockFrame>().setEnemiesDefeated(true);
        this.GetComponent<LockFrame>().lockPlayer();

        chatBubble = gameManager.GetComponent<ChatManager>().CreateBubble(this.gameObject,
            "Hey you! Remember how to move? Use the arrows!", 0);

        Debug.Log(moved);
        // wait
        while (!moved)
        {
            Debug.Log(moved);
            yield return null;
        }
        Debug.Log("REACHED " + moved);

        // dialogue (E and Q)
        chatBubble.GetComponent<ChatBubble>().Setup("Great. And punching? It's 'E' and 'Q', yeah?");

        // wait
        while (!punched)
        {
            yield return null;
        }

        // dialogue (T and R)
        chatBubble.GetComponent<ChatBubble>().Setup("Not bad! To pickup objects and weapons, use 'T' and 'R'");

        // wait
        while (!pickedUp)
        {
            yield return null;
        }

        // pause menu
        chatBubble.GetComponent<ChatBubble>().Setup("Don't forget: use 'P' to remember what I taught you!");

        // wait
        while (!paused)
        {
            yield return null;
        }

        // last advice
        chatBubble.GetComponent<ChatBubble>().Setup("Last pro tip: use combos for real damage. Good luck!");

        this.GetComponent<LockFrame>().unlockPlayer();
        Destroy(chatBubble,3);
    }
}
