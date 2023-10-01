using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public GameObject player;
    public GameObject pauseScreen;

    private bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gamePaused = !gamePaused;
            player.GetComponent<PlayerMovement>().enabled = !gamePaused;
            pauseScreen.SetActive(gamePaused);
        }
    }
}
