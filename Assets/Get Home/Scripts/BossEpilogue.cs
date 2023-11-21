using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEpilogue : MonoBehaviour
{
    public GameObject boss;
    public GameObject player;
    public GameObject endingScreen;
    public DialogueManagerV2.Line[] lines;

    private bool dialogueStarted = false;

    // Update is called once per frame
    void Update()
    {
        // start epilogue dialogue
        if (boss.GetComponent<EnemyHealth>().IsDead() && !dialogueStarted)
        {
            //StartCoroutine("Wait");
            dialogueStarted = true;
            player.GetComponent<DialogueManagerV2>().StartDialogue(lines);
        }

        // show ending screen once dialogue is finished
        if (dialogueStarted && !player.GetComponent<DialogueManagerV2>().DialogueOn())
        {
            this.GetComponent<GamePause>().pause(true);
            endingScreen.SetActive(true);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        dialogueStarted = true;
        player.GetComponent<DialogueManagerV2>().StartDialogue(lines);
    }
}
