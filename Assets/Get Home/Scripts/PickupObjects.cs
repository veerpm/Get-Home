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
    public float placeDownOffSet = 1.5f;
    public GameObject DestroyEffect;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (itemHolding)
            {
                itemHolding.transform.position = new Vector3(transform.position.x, transform.position.y - placeDownOffSet, 0.0f) + (Direction * 2);
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
        Vector3 endPoint = transform.position + Direction * 2;
        item.transform.parent = null;

        for (int i = 0; i < 25; i++)
        {
            item.transform.position = Vector3.Lerp(startPoint, endPoint, i * 0.04f);
            item.transform.Rotate(0,0,1);
            yield return null;
        }
        if (item.GetComponent<Rigidbody2D>())
            item.GetComponent<Rigidbody2D>().simulated = true;
      
        Destroy(item);
    }
}
