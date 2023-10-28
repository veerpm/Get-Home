using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public float upperBound;
    public float lowerBound;
    public float leftBound;
    public float rightBound;

    public bool freeze = false;

    public float leftFreeze;
    public float rightFreeze;

    // Update is called once per frame
    void Update()
    {
        float currentLeft;
        float currentRight;

        // world bounds if unfreezed
        if (!freeze)
        {
            currentLeft = leftBound;
            currentRight = rightBound;
        }
        // local bounds if freezed
        else
        {
            // if local bounds are past global bounds, we refer back to world bounds
            if(leftFreeze < leftBound)
            {
                currentLeft = leftBound;
            }
            else
            {
                currentLeft = leftFreeze;
            }
            // right side
            if(rightFreeze > rightBound)
            {
                currentRight = rightBound;
            }
            else
            {
                currentRight = rightFreeze;
            }
        }

        // set bounds
        if(transform.position.x < currentLeft)
        {
            transform.position = new Vector2(currentLeft, transform.position.y);
        }
        if (transform.position.x > currentRight)
        {
            transform.position = new Vector2(currentRight, transform.position.y);
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

    public void Freeze(float leftBound = 0, float rightBound = 0)
    {
        freeze = true;

        this.leftFreeze = leftBound;
        this.rightFreeze = rightBound;
    }

    public void unFreeze()
    {
        freeze = false;
    }
}
