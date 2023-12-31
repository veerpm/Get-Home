﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public GameObject player;
    public GameObject inventory;
    public GameObject healthBar;
    //SFX
    public AudioSource introSound;

    // Start is called before the first frame update
    void Start()
    {
        introSound.Play();
        player.SetActive(false);
        inventory.SetActive(false);
        healthBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private IEnumerator Wait()
    //{
    //    yield return new WaitForSeconds(1);
    //    player.SetActive(true);
    //    inventory.SetActive(true);
    //    healthBar.SetActive(true);
    //    gameObject.SetActive(false);
    //}

    void EnablePlayer()
    {
        player.SetActive(true);
        inventory.SetActive(true);
        healthBar.SetActive(true);
        gameObject.SetActive(false);
    }
}
