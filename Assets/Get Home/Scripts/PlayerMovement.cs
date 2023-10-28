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
    private PickupObjects PickUpObjects;
    public bool lookingRight = true;
    private Rigidbody2D rb;
    private GameObject displayText;

    //sound FX
    public AudioSource walkingSound1;
    public AudioSource walkingSound2;
    public AudioSource walkingSound3;


    public float acceleration;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        PickUpObjects = gameObject.GetComponent<PickupObjects>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        displayText = GameObject.FindGameObjectWithTag("DisplayText");

        if (displayText != null && !lookingRight && displayText.transform.localScale.x > 0)
        {

            displayText.transform.localScale = new Vector2(-displayText.transform.localScale.x, displayText.transform.localScale.y);
        }

        if (displayText != null && lookingRight && displayText.transform.localScale.x < 0)
        {

            displayText.transform.localScale = new Vector2(-displayText.transform.localScale.x, displayText.transform.localScale.y);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
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

            //transform.position += movement;

            rb.MovePosition(new Vector2(transform.position.x + movement.x, transform.position.y + movement.y));

            PickUpObjects.Direction = new Vector3(horizontal_movement, 0.0f, 0.0f).normalized;
            



            // align sprite to the movement's direction
            if (horizontal_movement < 0 && lookingRight)
            {
                // face left
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                lookingRight = false;
            }
            else if (horizontal_movement > 0 && !lookingRight)
            {
                // face right
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                lookingRight = true;

            }

            animator.SetFloat("SpeedX", Mathf.Abs(horizontal_movement));
            animator.SetFloat("SpeedY", Mathf.Abs(vertical_movement));
        }

    }

    bool CanMove()
    {
        return animator.GetBool("canMove");
    }

    void IsWalking()
    {
        //sound FX
        int randomSound = Random.Range(0, 2);
        if (randomSound < 1)
        {
            walkingSound1.Play();
        }
        else if (randomSound >= 2)
        {
            walkingSound2.Play();
        }
        else
        {
            walkingSound3.Play();
        }
    }

}