using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public float upperBound;
    public float lowerBound;
    public float leftBound;
    public float rightBound;
    public float leftOffset = 1f;
    public float rightOffset = 1f;
    public bool freeze = false;

    public float leftFreeze;
    public float rightFreeze;

    private float leftFloatingBound;
    private float rightFloatingBound;

    // Start is called before the first frame update
    void Start()
    {
        leftFloatingBound = transform.position.x - leftOffset;
        rightFloatingBound = transform.position.x + rightOffset;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // update horizontal bounds on player's position
        if (!freeze)
        {
            leftFloatingBound = transform.position.x - leftOffset;
            rightFloatingBound = transform.position.x + rightOffset;
        }
        */
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
        /*
        if(transform.position.x < leftFloatingBound)
        {
            transform.position = new Vector2(leftFloatingBound, transform.position.y);
        }
        if (transform.position.x > rightFloatingBound)
        {
            transform.position = new Vector2(rightFloatingBound, transform.position.y);
        }
        */
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
