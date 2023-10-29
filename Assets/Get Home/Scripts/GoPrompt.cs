using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoPrompt : MonoBehaviour
{
    public float blinkInterval = 0.5f;
    private float nextToggleTime;
    private float startTime;
    public float displayTime = 1f;
    private bool startBlinking = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > displayTime)
        {
            startBlinking = false;
            GetComponent<Image>().enabled = false;
        }

        if (startBlinking && Time.time >= nextToggleTime)
        {
            GetComponent<Image>().enabled = !GetComponent<Image>().enabled;
            nextToggleTime = Time.time + blinkInterval;
        }
    }

    public void Display()
    {
        startBlinking = true;
        startTime = Time.time;
    }

}
