using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public float upperBound;
    public float lowerBound;
    public float leftBoundOffset;
    public float rightBoundOffset;
    public bool freeze = false;

    private float leftBound;
    private float rightBound;
    private float freezedX;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // update horizontal bounds on player's position
        if (!freeze)
        {
            freezedX = transform.position.x;
            leftBound = transform.position.x - leftBoundOffset;
            rightBound = transform.position.x + rightBoundOffset;
        }

        // set bounds
        if(transform.position.x < leftBound)
        {
            transform.position = new Vector2(leftBound, transform.position.y);
        }
        if (transform.position.x > rightBound)
        {
            transform.position = new Vector2(rightBound, transform.position.y);
        }
        if (transform.position.y < lowerBound)
        {
            transform.position = new Vector2(transform.position.x, lowerBound);

        }
        if (transform.position.y > upperBound)
        {
            transform.position = new Vector2(transform.position.x, upperBound);
        }
    }
}
