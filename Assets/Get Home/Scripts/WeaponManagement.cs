using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManagement : MonoBehaviour
{
    //public static Weapon[] weaponsArray;
    private static GameObject equippedWeapon; // current weapon
    public GameObject floorWeapon;
    private GameObject defaultWeapon;
    private GameObject player;
    private bool pickable;
    private PlayerCombatMelee combatMelee;
    public Image weaponDisplay; // HUD weapon display
    private GameObject weaponHolder;
    // Start is called before the first frame update
    void Start()
    {
        weaponHolder = GameObject.Find("WeaponHolder");
        defaultWeapon = GameObject.Find("Fists");
        player = GameObject.Find("Player");
        combatMelee = player.GetComponent<PlayerCombatMelee>();
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
            combatMelee.GetComponent<PlayerCombatMelee>().SetWeaponStats(equippedWeapon);
            DisplayWeapon();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            pickable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            pickable = false;
        }
    }

    void SwapWeapon()
    {
        gameObject.SetActive(false);
        floorWeapon = EquippedWeapon;
        EquippedWeapon = gameObject;
        floorWeapon.transform.position = player.transform.position;
        floorWeapon.SetActive(true);
    }

    void DropWeapon()
    {
        EquippedWeapon.transform.position = player.transform.position;
        EquippedWeapon.SetActive(true);
        EquippedWeapon = defaultWeapon;
    }


    void DisplayWeapon()
    {
        weaponDisplay.sprite = EquippedWeapon.GetComponent<SpriteRenderer>().sprite;
    }
}
