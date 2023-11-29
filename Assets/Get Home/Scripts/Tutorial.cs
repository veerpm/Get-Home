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
    public GameObject enemyStopNuisance;

    private GameObject chatBubble;

    //private bool moved = false;
    private bool pickedUpTrash = false;
    private bool paused = false;
    private bool tutorialLaunched = false;


    public void Start()
    {
        StartCoroutine(Setup());
        toggleKnife(false);
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

    IEnumerator Setup()
    {
        // make homeless speak only when first conversation is done
        while (!player.GetComponent<DialogueManager>().isConversationDone(0))
        {
            yield return null;
        }

        // start dialogue
        float time = 0f;
        float textSize = 1f;
        Vector3 position = new Vector3(-1f, 1.4f, 0f);

        chatBubble = gameManager.GetComponent<ChatManager>().CreateBubble(this.gameObject,
            "This drunkard forgot he could move \n" +
            "with the <b> arrows keys </b>!", time, textSize, position);
    }

    IEnumerator tutorialLogic()
    {
        // freeze player
        this.GetComponent<LockFrame>().setEnemiesDefeated(true);
        this.GetComponent<LockFrame>().lockPlayer();

        // deactivate enemies that are too close
        DeactivateNuisance();

        // variables for instantiating
        GameObject enemy;
        GameObject trash;

        // dialogue (E and Q)
        chatBubble.GetComponent<ChatBubble>().Setup("Hah! This schlub doesn’t know that pressing <b>E</b> \n" +
            "throws out a <b>light attack</b>.");

        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("I doubt he knows he can do a <b> heavy attack </b> \n" +
            "by pressing <b>Q</b>. Idiot.");

        while (!Input.GetKeyDown(KeyCode.Q))
        {
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("He even has less chance of doing a <b> combo </b> \n" +
            "by pressing  <b> E E Q E </b> successively.");
        bool hasPressedQ = false;

        yield return null; // resets Input.GetKeyDown.Q

        while (!hasPressedQ || !Input.GetKeyDown(KeyCode.E))
        {
            // check if player pressed Q
            hasPressedQ = hasPressedQ || Input.GetKeyDown(KeyCode.Q);
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("He won't have the guts to try it on this guy.");


        enemy = CreateEnemy(75);
        yield return new WaitForSeconds(1f);
        bool didCombo = false;

        // wait
        while (!didCombo && !enemy.GetComponent<EnemyHealth>().IsDead())
        {
            // check if player just did (or already did) the combo
            didCombo = didCombo || player.GetComponent<PlayerCombatMelee>().comboActive;

            // spawn enemy if he's dead
            if (enemy.GetComponent<EnemyHealth>().IsDead())
            {
                enemy = CreateEnemy(75);
            }

            yield return null;
        }

        // dialogue (T and R)
        chatBubble.GetComponent<ChatBubble>().Setup("He probably doesn’t even know that he can " +
            "\n  <b> pickup trash cans </b> by pressing <b>R</b>.");

        trash = CreateTrash();
        // wait
        while (!pickedUpTrash || !player.GetComponent<PickupObjects>().IsHolding())
        {
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("I bet he's not strong enough to <b>throw it</b> by moving \n" +
            "and pressing the button <b>R</b> again.");

        while (!trash.GetComponent<ThrownObjectsHitDetect>().thrown)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        chatBubble.GetComponent<ChatBubble>().Setup("Tss, he's not smart enough to throw trash against enemies anyway.");

        trash = CreateTrash();
        enemy = CreateEnemy();
        bool hasThrown = false;

        yield return new WaitForSeconds(0.5f);
        // wait
        while (!enemy.GetComponent<EnemyHealth>().IsDead() && !hasThrown)
        {
            // check if player has thrown a trash
            hasThrown = hasThrown || trash.GetComponent<ThrownObjectsHitDetect>().thrown;

            // resets trash if removed
            if (trash.GetComponent<ThrownObjectsHitDetect>().thrown)
            {
                trash =  CreateTrash();
            }
            // resets enemy if dead
            if (enemy.GetComponent<EnemyHealth>().IsDead())
            {
                enemy = CreateEnemy();
            }
            yield return null;
        }

        toggleKnife(true);

        // dialogue (T and R)
        chatBubble.GetComponent<ChatBubble>().Setup("Honestly, only a guy like this would be dull enough " +
            "\n to use <b>R</b> to pick up that knife.");

        // wait
        while (player.GetComponent<WeaponManagement>().EquippedWeapon != knife)
        {
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("I bet he wouldn't even be courageous enough " +
            "\n to try this knife on this dude.");
        enemy = CreateEnemy();
        yield return new WaitForSeconds(0.5f);

        // wait
        while (!enemy.GetComponent<EnemyHealth>().IsDead())
        {
            yield return null;
        }

        chatBubble.GetComponent<ChatBubble>().Setup("As incompetent as he looks, \n he won't even be able to drop the knife using <b>F</b>");
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;
        }

        // pause menu
        chatBubble.GetComponent<ChatBubble>().Setup("I'm also sure he'll forget stuff \n and will need to press <b>P</b> to remember.");

        // wait
        while (!paused)
        {
            yield return null;
        }

        // last advice
        /*
        chatBubble.GetComponent<ChatBubble>().Setup("If he's smart enough, he will use 'Q' and 'E' to do combos \n" +
            "and more damage on enemies. But clearly he's not!");
        */

        this.GetComponent<LockFrame>().unlockPlayer();
        Destroy(chatBubble,3);

        // reactivate enemies that were too close
        ReactivateNuisance();
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

        // set sounds of enemies to player's volume
        SetSound(enemy);

        return enemy;
    }

    // spawn trash when player misses his/her throw
    private GameObject CreateTrash()
    {
        GameObject trash = Instantiate(trashCan, trashPos, Quaternion.identity);

        // set sounds of trash to player's volume
        SetSound(trash);

        return trash;
    }

    private void SetSound(GameObject entity)
    {
        // set volume of all audio sources to player's setting
        if (PlayerPrefs.HasKey("Volume"))
        {
            AudioSource[] sounds = entity.GetComponents<AudioSource>();
            foreach (AudioSource audioSource in sounds)
            {
                audioSource.volume = PlayerPrefs.GetFloat("Volume");
            }
        }
    }

    // 2 enemies are too close to the tutorial and need to be deactivated while it's active
    private void DeactivateNuisance()
    {
        enemyStopNuisance.GetComponent<LockFrame>().setEnemiesDefeated(true);
        foreach (GameObject enemy in enemyStopNuisance.GetComponent<LockFrame>().enemies)
        {
            enemy.SetActive(false);
        }
    }

    // the 2 enemies too close are reactivated
    private void ReactivateNuisance()
    {
        enemyStopNuisance.GetComponent<LockFrame>().setEnemiesDefeated(false);
        foreach (GameObject enemy in enemyStopNuisance.GetComponent<LockFrame>().enemies)
        {
            enemy.SetActive(true);
        }
    }

    private void toggleKnife(bool activate)
    {
        knife.GetComponentInChildren<BoxCollider2D>().enabled = activate;
        knife.GetComponent<SpriteRenderer>().enabled = activate;
    }
}
