using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockEnemies : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject player;

    private bool enemiesLocked;
    // Update is called once per frame
    void Update()
    {
        if (enemiesLocked)
        {
            foreach (GameObject enemy in enemies)
            {
                Debug.Log("Called 0 ");
                enemy.transform.position = player.GetComponent<Boundaries>().SetInBounds(enemy.transform.position);
            }
        }
    }

    public void Lock()
    {
        enemiesLocked = true;
    }

    public void Unlock()
    {
        enemiesLocked = false;
    }
}
