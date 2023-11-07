using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject player;
    public GameObject trashCan;
    public Vector3 trashPos;
    public GameObject baseballEnemy;
    public Vector3 enemyPos;

    private GameObject chatBubble;

    //private bool moved = false;
    private bool punched = false;
    private bool pickedUpTrash = false;
    private bool paused = false;
    private bool pickedupWeapon = false;

    public void Start()
    {
        StartCoroutine(tutorialLogic());
    }

    private void Update()
    {
        /*
        if(Input.GetAxisRaw("Horizontal") != 0|| Input.GetAxisRaw("Vertical") != 0)
        {
            moved = true;
        }
        */

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q))
        {
            punched = true;
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            pickedUpTrash = true;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            pickedupWeapon = true;
        }
    }

    IEnumerator tutorialLogic()
    {
        // freeze player
        this.GetComponent<LockFrame>().setEnemiesDefeated(true);
        this.GetComponent<LockFrame>().lockPlayer();

        chatBubble = gameManager.GetComponent<ChatManager>().CreateBubble(this.gameObject,
            "Hey!", 0);
        // telling player to move seems useless (they always start moving)
        /*
        yield return new WaitForSeconds(1);

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
        */

        // dialogue (E and Q)
        chatBubble.GetComponent<ChatBubble>().Setup("Hey you! You know how to punch? It's 'E' and 'Q'.");

        // wait
        while (!punched)
        {
            yield return null;
        }

        // dialogue (T and R)
        chatBubble.GetComponent<ChatBubble>().Setup("Not bad! Pickup the thrash here using 'T'");

        // wait
        while (!pickedUpTrash && !player.GetComponent<PickupObjects>().IsHolding())
        {
            yield return null;
        }
        Debug.Log(player.GetComponent<PickupObjects>().IsHolding());

        chatBubble.GetComponent<ChatBubble>().Setup("Now the fun part: press 'T' again while moving to throw it on this enemy!");
        GameObject enemy = CreateEnemy();

        yield return new WaitForSeconds(0.5f);
        // wait
        while (!enemy.GetComponent<EnemyHealth>().IsDead())
        {
            // bring back enemy if he's dead
            Debug.Log(trashCan.transform.position);
            if (!player.GetComponent<PickupObjects>().IsHolding())
            {
                trashCan.transform.position = trashPos;
            }
            yield return null;
        }

        // dialogue (T and R)
        chatBubble.GetComponent<ChatBubble>().Setup("See that knife on the ground? Press 'R' while near it to get an edge in battle!");

        // wait
        while (!pickedupWeapon && !player.GetComponent<PickupObjects>().IsHolding())
        {
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("Try using it on that enemy");
        enemy = CreateEnemy();

        // wait
        while (!enemy.GetComponent<EnemyHealth>().IsDead())
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

    private GameObject CreateEnemy(int health = 10)
    {
        GameObject enemy = Instantiate(baseballEnemy, enemyPos, Quaternion.identity);
        // set properties
        baseballEnemy.GetComponent<MeleeEnemyFollowPlayer>().player = player.transform;
        baseballEnemy.GetComponent<MeleeEnemyFollowPlayer>().attackDamage = 5;
        baseballEnemy.GetComponent<EnemyHealth>().maxHealth = health;
        baseballEnemy.GetComponent<EnemyHealth>().SetFullHealth();

        return enemy;
    }
}
