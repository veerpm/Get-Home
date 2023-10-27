using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjects : MonoBehaviour
{
    public Transform holdSpot;
    public LayerMask pickUpMask;
    public float pickupRadius = 0.9f;

    public Vector3 Direction {get; set;}
    private GameObject itemHolding;
    public float placeDownOffSet = 0f;
    private Vector3 yOffset = new Vector3 (0.0f, -0.3f, 0.0f);
    // public GameObject DestroyEffect;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Direction);
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (itemHolding)
            {
                if (Mathf.Abs(Direction.x) > 0)
                {
                    itemHolding.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    itemHolding.transform.GetComponent<ThrownObjectsHitDetect>().thrown = true;
                    itemHolding.transform.position = new Vector3(transform.position.x, transform.position.y - placeDownOffSet, 0.0f) + (Direction * 2);
                    GetComponent<PlayerCombatMelee>().enabled = true;
                    itemHolding.transform.parent = null;
                    if (itemHolding.GetComponent<Rigidbody2D>())
                        itemHolding.GetComponent<Rigidbody2D>().simulated = true;
                    itemHolding = null;
                }
                else
                {
                    if(GetComponent<PlayerMovement>().lookingRight == true)
                    {
                        itemHolding.transform.position = new Vector3(holdSpot.position.x, holdSpot.position.y - placeDownOffSet, 0.0f) + new Vector3(0.6f, 0.3f, 0);
                        GetComponent<PlayerCombatMelee>().enabled = true;
                        itemHolding.transform.parent = null;
                        if (itemHolding.GetComponent<Rigidbody2D>())
                            itemHolding.GetComponent<Rigidbody2D>().simulated = true;
                        itemHolding = null;
                    }
                    else
                    {
                        itemHolding.transform.position = new Vector3(holdSpot.position.x, holdSpot.position.y - placeDownOffSet, 0.0f) + new Vector3(-0.6f, 0.3f, 0);
                        GetComponent<PlayerCombatMelee>().enabled = true;
                        itemHolding.transform.parent = null;
                        if (itemHolding.GetComponent<Rigidbody2D>())
                            itemHolding.GetComponent<Rigidbody2D>().simulated = true;
                        itemHolding = null;
                    }
                  
                }
                
            }
            else
            {
                Collider2D pickUpItem = Physics2D.OverlapCircle(transform.position, pickupRadius, pickUpMask);
                if (pickUpItem)
                {
                    itemHolding = pickUpItem.gameObject;
                    itemHolding.transform.position = holdSpot.position;
                    itemHolding.transform.parent = transform;
                    GetComponent<PlayerCombatMelee>().enabled = false;
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
}
