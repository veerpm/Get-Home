using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public float speed = 5.0f;
    private float horizontal_movement;
    private float vertical_movement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal_movement = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        vertical_movement = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

        transform.position += new Vector3(horizontal_movement, vertical_movement, 0.0f);

        animator.SetFloat("SpeedX", Mathf.Abs(horizontal_movement));
        animator.SetFloat("SpeedY", Mathf.Abs(vertical_movement));
     

    }
}
