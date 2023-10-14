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
                    itemHolding.transform.position = new Vector3(transform.position.x, transform.position.y - placeDownOffSet, 0.0f) + (Direction * 2);
                    
                }
                else
                {
                    itemHolding.transform.position = new Vector3(transform.position.x, transform.position.y - placeDownOffSet, 0.0f) + (Direction);
                }
                
                itemHolding.transform.parent = null;
                if (itemHolding.GetComponent<Rigidbody2D>())
                    itemHolding.GetComponent<Rigidbody2D>().simulated = true;
                itemHolding = null;
            }
            else
            {
                Collider2D pickUpItem = Physics2D.OverlapCircle(transform.position + Direction, pickupRadius, pickUpMask);
                if (pickUpItem)
                {
                    itemHolding = pickUpItem.gameObject;
                    itemHolding.transform.position = holdSpot.position;
                    itemHolding.transform.parent = transform;
                    if (itemHolding.GetComponent<Rigidbody2D>())
                        itemHolding.GetComponent<Rigidbody2D>().simulated = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (itemHolding)
            {
                StartCoroutine(ThrowItem(itemHolding));
                itemHolding = null;
            }
        }
    }

    IEnumerator ThrowItem(GameObject item)
    {
        Vector3 startPoint = item.transform.position;
        //Debug.Log("Start Point: " + startPoint);
        Vector3 endPoint = transform.position + (Direction * 5);
        //Debug.Log("Start Point: " + endPoint);
        item.transform.parent = null;
        

        for (int i = 0; i < 100; i++)
        {
            item.transform.position = Vector3.Lerp(startPoint, endPoint, i * 0.007f) + yOffset;
            yield return null;
        }
        if (item.GetComponent<Rigidbody2D>())
            item.GetComponent<Rigidbody2D>().simulated = true;
      
        Destroy(item);
    }

}
