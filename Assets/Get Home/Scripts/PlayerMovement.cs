using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public float speed;
    private float horizontal_movement;
    private float vertical_movement;
    public AnimationEvents events;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        events = gameObject.GetComponent<AnimationEvents>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove())
        {

            horizontal_movement = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
            vertical_movement = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;


            transform.position += new Vector3(horizontal_movement, vertical_movement, 0.0f);

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
