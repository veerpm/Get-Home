using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PickupObjects : MonoBehaviour
{
    public Transform holdSpot;
    public LayerMask pickUpMask;
    public float pickupRadius = 0.9f;

    public Vector3 Direction { get; set; }
    private GameObject itemHolding;
    private Collider2D pickUpItem;
    Collider2D item;
    public float placeDownOffSet = 0f;
    private Vector3 yOffset = new Vector3(0.0f, -0.3f, 0.0f);

    //sound FX
    public AudioSource pickUpTrashSound;
    public AudioSource throwTrashSound;

    // public GameObject DestroyEffect;

    // Update is called once per frame
    void Update()
    {

        // need to find if in range without any input so can glow when close
        pickUpItem = Physics2D.OverlapCircle(transform.position, pickupRadius, pickUpMask);

        if (pickUpItem && !pickUpItem.CompareTag("Football") && !pickUpItem.GetComponent<ThrownObjectsHitDetect>().thrown)
        {
            pickUpItem.transform.Find("Light").GetComponent<Light2D>().enabled = true;
            item = pickUpItem;
        }
        else if (item)
        {
            item.transform.Find("Light").GetComponent<Light2D>().enabled = false;
            item = null;
        }

        //Debug.Log(Direction);
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (itemHolding)
            {
                //sound FX
                throwTrashSound.Play();
                gameObject.GetComponent<PlayerMovement>().speed = 3;
                itemHolding.transform.parent = null;
                itemHolding.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                itemHolding.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;

                if (itemHolding.CompareTag("Trash Can"))
                {
                    itemHolding.GetComponent<ThrownObjectsHitDetect>().thrown = true;

                    if (GetComponent<PlayerMovement>().lookingRight == true)
                    {
                        itemHolding.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 200);
                    }
                    else
                    {
                        itemHolding.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 200);
                    }

                    Debug.Log("Sausage McMuffin");
                }

                if (itemHolding.CompareTag("Football"))
                {
                    if (GetComponent<PlayerMovement>().lookingRight == true)
                    {
                        itemHolding.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 200);
                    }
                    else
                    {
                        itemHolding.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 200);
                    }
                }

                GetComponent<PlayerCombatMelee>().enabled = true;
                if (itemHolding.GetComponent<Rigidbody2D>())
                    itemHolding.GetComponent<Rigidbody2D>().simulated = true;

                itemHolding.transform.parent = null;
                // Automatically destroy thrown object after 0.5 seconds
                Destroy(itemHolding, 0.5f);
                itemHolding = null;
            }

            else
            {
                if (pickUpItem)
                {
                    //sound FX
                    pickUpTrashSound.Play();
                    gameObject.GetComponent<PlayerMovement>().speed = 1.5f;
                    itemHolding = pickUpItem.gameObject;
                    itemHolding.transform.position = holdSpot.position;
                    itemHolding.transform.parent = transform;

                    // Disable attacking when carrying object
                    GetComponent<PlayerCombatMelee>().enabled = false;
                    if (itemHolding.CompareTag("Football"))
                    {
                        itemHolding.GetComponent<ThrowableObject>().caught = true;
                        itemHolding.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    }
                    if (itemHolding.GetComponent<Rigidbody2D>())
                        itemHolding.GetComponent<Rigidbody2D>().simulated = false;
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (holdSpot == null)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }

    // check if player is holding something
    public bool IsHolding()
    {
        if (itemHolding == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
