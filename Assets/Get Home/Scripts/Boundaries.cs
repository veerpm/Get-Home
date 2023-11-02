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
    private bool wasMoved;

    // Update is called once per frame
    void Update()
    {
        float currentLeft;
        float currentRight;

        // world bounds are unfreezed
        if (!freeze)
        {
            currentLeft = leftBound;
            currentRight = rightBound;
        }
        // local bounds are freezed
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
        wasMoved = false;

        if (transform.position.x < currentLeft)
        {
            transform.position = new Vector2(currentLeft, transform.position.y);
            wasMoved = true;
        }
        if (transform.position.x > currentRight)
        {
            transform.position = new Vector2(currentRight, transform.position.y);
            wasMoved = true;
        }
        if (transform.position.y < lowerBound)
        {
            transform.position = new Vector2(transform.position.x, lowerBound);
            wasMoved = true;
        }
        if (transform.position.y > upperBound)
        {
            transform.position = new Vector2(transform.position.x, upperBound);
            wasMoved = true;
        }

        // toggle walking animation if we are entering/leaving invisible wall
        bool touchingBound = false;
        if (Mathf.Abs(transform.position.x - currentLeft) < 0.1f)
        {
            touchingBound = true;
        }
        if (Mathf.Abs(transform.position.x - currentRight) < 0.1f)
        {
            touchingBound = true;
        }
        if (Mathf.Abs(transform.position.y - lowerBound) < 0.1f)
        {
            touchingBound = true;
        }
        if (Mathf.Abs(transform.position.y - upperBound) < 0.1f)
        {
            touchingBound = true;
        }

        // toggle walking
        if (wasMoved)
        {
            gameObject.GetComponent<PlayerMovement>().SetColliding(true);
        }
        else if(!touchingBound)
        {
            gameObject.GetComponent<PlayerMovement>().SetColliding(false);
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
