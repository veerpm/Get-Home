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

        float time = 0f;
        float textSize = 1f;
        Vector3 position = new Vector3(-1f, 1.4f, 0f);
        chatBubble = gameManager.GetComponent<ChatManager>().CreateBubble(this.gameObject,
            "Hey!", time, textSize, position);
        GameObject enemy;
        GameObject trash;

        // dialogue (E and Q)
        chatBubble.GetComponent<ChatBubble>().Setup("Hah! This schlub doesn’t know that pressing 'E' \n" +
            "throws out a light attack, while pressing 'Q' \n " +
            "throws out a heavy attack. Idiot.");

       enemy = CreateEnemy();
        yield return new WaitForSeconds(0.5f);

        // wait
        while (!enemy.GetComponent<EnemyHealth>().IsDead())
        {
            yield return null;
        }

        // dialogue (T and R)
        chatBubble.GetComponent<ChatBubble>().Setup("He probably doesn’t even know that he can " +
            "\n pickup trash by pressing 'T'.");

        trash = CreateTrash();
        // wait
        while (!pickedUpTrash || !player.GetComponent<PickupObjects>().IsHolding())
        {
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("I bet he didn't even think he could press 'T' again " +
            "\n while moving to throw it on enemies.");
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
        chatBubble.GetComponent<ChatBubble>().Setup("Honestly, only a guy like this would be dull enough " +
            "\n to use 'R' to pick up that bat.");

        // wait
        while (player.GetComponent<WeaponManagement>().EquippedWeapon != knife)
        {
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("I bet he wouldn't even be courageous enough " +
            "\n to try it on this dude.");
        enemy = CreateEnemy();
        yield return new WaitForSeconds(0.5f);

        // wait
        while (!enemy.GetComponent<EnemyHealth>().IsDead())
        {
            yield return null;
        }


        // pause menu
        chatBubble.GetComponent<ChatBubble>().Setup("I'm also sure he'll forget stuff and will need to press 'P' to remember.");

        // wait
        while (!paused)
        {
            yield return null;
        }

        // last advice
        chatBubble.GetComponent<ChatBubble>().Setup("If he's smart enough, he will use 'Q' and 'E' to do combos \n" +
            "and more damage on enemies. But clearly he's not!");

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
