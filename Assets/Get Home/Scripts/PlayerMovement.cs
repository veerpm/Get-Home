using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public float speed;
    private float horizontal_movement;
    private float vertical_movement;
    private Vector3 movement;
    public AnimationEvents events;
    private PickupObjects PickUpObjects;

    public float acceleration;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        events = gameObject.GetComponent<AnimationEvents>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        PickUpObjects = gameObject.GetComponent<PickupObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove())
        {
            //if (Mathf.Abs(movementVector.y) > 0)
            //{
            //    currentSpeed += acceleration * Time.deltaTime;
            //}
            //else
            //{
            //    currentSpeed -= deacceleration * Time.deltaTime;
            //    7
            //currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed)

            horizontal_movement = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
            vertical_movement = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
            movement = new Vector3(horizontal_movement, vertical_movement ,0.0f);

            transform.position += movement;

         
            
           PickUpObjects.Direction = new Vector3(horizontal_movement, 0.0f, 0.0f).normalized;
            



            // align sprite to the movement's direction
            if (horizontal_movement < 0)
            {
                // face left
                sprite.flipX = true;
            }
            else if (horizontal_movement > 0)
            {
                // face right
                sprite.flipX = false;
            }


            animator.SetFloat("SpeedX", Mathf.Abs(horizontal_movement));
            animator.SetFloat("SpeedY", Mathf.Abs(vertical_movement));
        }
     

    }

    bool CanMove()
    {
        return animator.GetBool("canMove");
    }

}
