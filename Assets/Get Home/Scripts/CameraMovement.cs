using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float followSpeed;
    public Transform target;
    public float yPosition;

    public bool freeze = false;

    public bool shake = false;
    public float shakeStrength = 0.075f;
    public float shakeFrequency = 2f; // unit = frames/shaking

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // camera follows target (ex: player) with speed of 'followSpeed'
        if(!freeze)
        {
            Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
            // ancient line also followed YPos: Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
        // implements camera shake
        if (shake)
        {
            // shake at each 'shakeFrequency' frame
            if (Time.frameCount % shakeFrequency == 0)
            {
                // move camera randomly in a circle
                Vector2 shakeCoord = Random.insideUnitCircle;
                transform.position = new Vector3(transform.position.x + shakeCoord.x * shakeStrength,
                    transform.position.y + shakeCoord.y * shakeStrength,
                    transform.position.z);
            }
        }
    }

    public void setFreeze(bool isFreezed)
    {
        this.freeze = isFreezed;
    }

}
