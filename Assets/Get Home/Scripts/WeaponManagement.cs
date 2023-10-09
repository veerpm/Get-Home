using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManagement : MonoBehaviour
{
    private GameObject equippedWeapon; // current weapon
    public GameObject floorWeapon;
    public GameObject defaultWeapon;
    private bool pickable;
    public Image weaponDisplay; // HUD weapon display
    public GameObject durability;

    // Start is called before the first frame update
    void Start()
    {
        defaultWeapon = GameObject.Find("Fists");
        EquippedWeapon = defaultWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickable && Input.GetKeyDown(KeyCode.R))
        {
            SwapWeapon();
        }

        if (Input.GetKeyDown(KeyCode.F) && EquippedWeapon != defaultWeapon)
        {
            DropWeapon();
        }
    }

    public GameObject EquippedWeapon
    {
        get
        {
            return equippedWeapon;
        }

        set
        {
            equippedWeapon = value;
            GetComponent<PlayerCombatMelee>().SetWeaponStats(equippedWeapon);
            DisplayWeapon();
            DisplayWeaponDurability();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Weapon")
        {
            pickable = true;
            floorWeapon = collider.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Weapon")
        {
            pickable = false;
        }
    }

    void SwapWeapon()
    {
        floorWeapon.SetActive(false);
        GameObject temp = floorWeapon;
        floorWeapon = EquippedWeapon;
        EquippedWeapon = temp;
        floorWeapon.transform.position = transform.position;
        floorWeapon.SetActive(true);
    }

    void DropWeapon()
    {
        EquippedWeapon.transform.position = transform.position;
        EquippedWeapon.SetActive(true);
        EquippedWeapon = defaultWeapon;
    }

    void DisplayWeapon()
    {
        weaponDisplay.sprite = EquippedWeapon.GetComponent<SpriteRenderer>().sprite;
    }

    public void DisplayWeaponDurability()
    {
        if (EquippedWeapon == defaultWeapon)
        {
            durability.GetComponent<Text>().text = "No limit";
        }
        else
        {
            durability.GetComponent<Text>().text = EquippedWeapon.GetComponent<WeaponStats>().maxHits.ToString();
        }
    }
}
