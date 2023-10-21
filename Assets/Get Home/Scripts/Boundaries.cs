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
        // update horizontal bounds on player's position
        if (!freeze)
        {
            leftFloatingBound = transform.position.x - leftOffset;
            rightFloatingBound = transform.position.x + rightOffset;
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
        if(transform.position.x < leftFloatingBound)
        {
            transform.position = new Vector2(leftFloatingBound, transform.position.y);
        }
        if (transform.position.x > rightFloatingBound)
        {
            transform.position = new Vector2(rightFloatingBound, transform.position.y);
        }
    }

    public void Freeze(float upBound = 0, float lowBound = 0, float leftOffset = 0, float rightOffset = 0)
    {
        freeze = true;

        upperBound = upBound;
        lowerBound = lowBound;
        this.leftOffset = leftOffset;
        this.rightOffset = rightOffset;
    }

    public void unFreeze()
    {
        freeze = false;
    }
}
