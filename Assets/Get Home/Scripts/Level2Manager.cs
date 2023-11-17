using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueManager;

public class Level2Manager : MonoBehaviour
{
    private GameObject player;
    public float pos1;
    public bool activated1 = false;
    public float pos2;
    private bool activated2 = false;
    private float sensibility = 0.25f;
    public GameObject bicycleEnemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.transform.position.x - pos1) < sensibility && !activated1)
        {
            for (int i = 3; i > 0; i--)
            {
                GameObject bicycleEnemy = Instantiate(bicycleEnemySpawner);
                bicycleEnemy.GetComponent<BicycleEnemySpawner>().spawnDelay = Random.Range(3, 5);
                bicycleEnemy.GetComponent<BicycleEnemySpawner>().spawnLowerBound = -3.7f;
                bicycleEnemy.GetComponent<BicycleEnemySpawner>().spawnUpperBound = 2.14f;
                Destroy(bicycleEnemy, 30);
            }
            activated1 = true;
        }
    }
}
