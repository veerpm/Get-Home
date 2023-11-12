using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject player;
    public float triggerPosX;
    public GameObject trashCan;
    public Vector3 trashPos;
    public GameObject baseballEnemy;
    public Vector3 enemyPos;
    public GameObject knife;

    private GameObject chatBubble;

    //private bool moved = false;
    private bool pickedUpTrash = false;
    private bool paused = false;
    private bool tutorialLaunched = false;

    public void Start()
    {
    }

    private void Update()
    {
        // launch tuto near trigger
        if(Mathf.Abs(player.transform.position.x - triggerPosX) < 0.1 && !tutorialLaunched)
        {
            tutorialLaunched = true;
            StartCoroutine(tutorialLogic());
        }
        /*
        if(Input.GetAxisRaw("Horizontal") != 0|| Input.GetAxisRaw("Vertical") != 0)
        {
            moved = true;
        }
        */


        if (Input.GetKeyDown(KeyCode.T))
        {
            pickedUpTrash = true;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = true;
        }
    }

    IEnumerator tutorialLogic()
    {
        // freeze player
        this.GetComponent<LockFrame>().setEnemiesDefeated(true);
        this.GetComponent<LockFrame>().lockPlayer();

        chatBubble = gameManager.GetComponent<ChatManager>().CreateBubble(this.gameObject,
            "Hey!", 0);
        GameObject enemy;
        GameObject trash;

        // dialogue (E and Q)
        chatBubble.GetComponent<ChatBubble>().Setup("Hey you! You know how to punch? \n " +
            "It's 'E' for light attacks and 'Q' for heavy attacks.\n Try it on this guy!");

        enemy = CreateEnemy();
        yield return new WaitForSeconds(0.5f);

        // wait
        while (!enemy.GetComponent<EnemyHealth>().IsDead())
        {
            yield return null;
        }

        // dialogue (T and R)
        chatBubble.GetComponent<ChatBubble>().Setup("Not bad! Pickup the trash here using 'T'");

        trash = CreateTrash();
        // wait
        while (!pickedUpTrash || !player.GetComponent<PickupObjects>().IsHolding())
        {
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("Now the fun part: press 'T' again while moving to throw it on this enemy!");
        enemy = CreateEnemy();

        yield return new WaitForSeconds(0.5f);
        // wait
        while (!enemy.GetComponent<EnemyHealth>().IsDead())
        {
            if (trash.GetComponent<ThrownObjectsHitDetect>().thrown)
            {
                trash =  CreateTrash();
            }
            yield return null;
        }

        // dialogue (T and R)
        chatBubble.GetComponent<ChatBubble>().Setup("See that knife on the ground? Press 'R' while near it to pick it up.");

        // wait
        while (player.GetComponent<WeaponManagement>().EquippedWeapon != knife)
        {
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("OK, now try using it on that enemy");
        enemy = CreateEnemy();
        yield return new WaitForSeconds(0.5f);

        // wait
        while (!enemy.GetComponent<EnemyHealth>().IsDead())
        {
            yield return null;
        }


        // pause menu
        chatBubble.GetComponent<ChatBubble>().Setup("Nice! And don't forget: use 'P' to remember what I taught you.");

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

    // spawn baseball enemy in front of player
    private GameObject CreateEnemy(int health = 10)
    {
        GameObject enemy = Instantiate(baseballEnemy, enemyPos, Quaternion.identity);
        // set properties
        enemy.GetComponent<MeleeEnemyFollowPlayer>().player = player.transform;
        enemy.GetComponent<MeleeEnemyFollowPlayer>().attackDamage = 5;
        enemy.GetComponent<EnemyHealth>().maxHealth = health;
        enemy.GetComponent<EnemyHealth>().SetFullHealth();

        return enemy;
    }

    // spawn trash when player misses his/her throw
    private GameObject CreateTrash()
    {
        GameObject trash = Instantiate(trashCan, trashPos, Quaternion.identity);
        return trash;
    }
}
