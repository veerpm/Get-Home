using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnablePlayer()
    {
        player.SetActive(true);
        gameObject.SetActive(false);
    }
}
