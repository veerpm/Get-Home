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

    // Start is called before the first frame update
    void Start()
    {
        leftBound = transform.position.x - leftBoundOffset;
        rightBound = transform.position.x + rightBoundOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // update horizontal bounds on player's position
        if (!freeze)
        {
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

    public void Freeze(float upBound = 0, float lowBound = 0, float leftOffset = 0, float rightOffset = 0)
    {
        freeze = true;

        upperBound = upBound;
        lowerBound = lowBound;
        leftBoundOffset = leftOffset;
        rightBoundOffset = rightOffset;
    }

    public void unFreeze()
    {
        freeze = false;
    }
}
