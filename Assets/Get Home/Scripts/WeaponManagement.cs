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
    public Animator animator;

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

        if (!pickable && Input.GetKeyDown(KeyCode.F) && EquippedWeapon)
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
            animator.SetBool(EquippedWeapon.name, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.parent.gameObject.tag == "Weapon")
        {
            pickable = true;
            floorWeapon = collider.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.parent.gameObject.tag == "Weapon")
        {
            pickable = false;
        }
    }

    void SwapWeapon()
    {
        GameObject temp = floorWeapon;
        floorWeapon = EquippedWeapon;
        EquippedWeapon = temp;
        floorWeapon.transform.position = new Vector3(transform.position.x,transform.position.y-0.5f,0);
        EquippedWeapon.SetActive(false);
        floorWeapon.SetActive(true);
        animator.SetBool(floorWeapon.name, false);
    }

    void DropWeapon()
    {
        animator.SetBool(EquippedWeapon.name, false);
        EquippedWeapon.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
        EquippedWeapon.SetActive(true);
        EquippedWeapon = defaultWeapon;
    }

    void DisplayWeapon()
    {
        weaponDisplay.sprite = EquippedWeapon.GetComponent<SpriteRenderer>().sprite;
    }

    public void AdjustDurability()
    {
        EquippedWeapon.GetComponent<WeaponStats>().maxHits--;

        DisplayWeaponDurability();

        if (EquippedWeapon.GetComponent<WeaponStats>().maxHits <= 0 && EquippedWeapon != defaultWeapon)
        {
            //Debug.Log(EquippedWeapon);
            animator.SetBool(EquippedWeapon.name, false);
            EquippedWeapon = defaultWeapon;
        }
    }

    void DisplayWeaponDurability()
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
