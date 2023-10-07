using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : EnemyHealth
{
    public GameObject gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;
        base.keepActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (base.currentHealth <= 0)
        {
            activateCheckpoint();
        }
    }

    protected void activateCheckpoint()
    {
        // set checkpoint
        gameManager.GetComponent<GamePause>().updateCheckpoint(transform.position);
        // change color :D
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
    }
}
