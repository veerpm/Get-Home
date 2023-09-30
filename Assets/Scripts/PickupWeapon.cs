﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupWeapon : MonoBehaviour
{
    public int weaponIndex = 0;
    public GameObject selectedWeapon; // current weapon
    public PlayerCombatMelee combatMelee;
    private bool inRange = false;
    public Image weaponDisplay; // HUD weapon display

    void Start()
    {
        combatMelee = GameObject.Find("Player").GetComponent<PlayerCombatMelee>();
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
                selectedWeapon = weapon.gameObject;
            }
            i++;
        }
        combatMelee.GetComponent<PlayerCombatMelee>().getWeapon(selectedWeapon);
    }

    void displayWeapon(GameObject weaponObj)
    {
        weaponDisplay.sprite = weaponObj.GetComponent<SpriteRenderer>().sprite;
    }
}
