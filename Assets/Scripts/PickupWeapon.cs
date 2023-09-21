using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupWeapon : MonoBehaviour
{
    public int weaponIndex = 0;
    public GameObject selectedWeapon;
    private bool inRange = false;
    public Image weaponOnDisplay;

    void Start()
    {
        selectWeapon();
    }

    private void Update()
    {
        if (inRange)
        {
            Swap();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            inRange = false;
    }

    void Swap()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (weaponIndex >= transform.childCount - 1)
            {
                weaponIndex = 0;
            }
            else
            {
                weaponIndex++;
            }
            selectWeapon();
        }
    }

    void selectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == weaponIndex)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
                displayWeapon(weapon.gameObject);
            }
            i++;
        }
    }

    void displayWeapon(GameObject weaponObj)
    {
        weaponOnDisplay.sprite = weaponObj.GetComponent<SpriteRenderer>().sprite;
    }
}
