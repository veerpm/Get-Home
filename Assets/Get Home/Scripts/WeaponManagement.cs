using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManagement : MonoBehaviour
{
    GameObject equippedWeapon; // current weapon
    public GameObject floorWeapon;
    public GameObject defaultWeapon;
    bool pickable;
    public Image weaponDisplay; // HUD weapon display
    public GameObject durability;
    public Animator animator;
    public Image durabilityBarFull;
    public Image durabilityBarEmpty;
    public float spacing;
    public float offset;

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
            DisplayWeapon(equippedWeapon);
            animator.SetBool(EquippedWeapon.name, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Transform colTr = collider.transform.parent;
        if (colTr != null && colTr.tag == "Weapon")
        {
            pickable = true;
            floorWeapon = collider.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Transform colTr = collider.transform.parent;
        if (colTr != null && colTr.tag == "Weapon")
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

    void DisplayWeapon(GameObject weapon)
    {
        weaponDisplay.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        DisplayWeaponDurability(weapon);
    }

    public void AdjustDurability()
    {
        EquippedWeapon.GetComponent<WeaponStats>().currentHits--;

        DisplayWeaponDurability(EquippedWeapon);

        if (EquippedWeapon.GetComponent<WeaponStats>().currentHits == 0 && EquippedWeapon != defaultWeapon)
        {
            DisplayWeapon(defaultWeapon);
        }

        if (EquippedWeapon.GetComponent<WeaponStats>().currentHits < 0 && EquippedWeapon != defaultWeapon)
        {
            animator.SetBool(EquippedWeapon.name, false);
            EquippedWeapon = defaultWeapon;
        }
        

    }

    void DisplayWeaponDurability(GameObject weapon)
    {
        //if (weapon == defaultWeapon)
        //{
        //    durability.GetComponent<Text>().text = "No limit";
        //}
        //else
        //{
        //    durability.GetComponent<Text>().text = weapon.GetComponent<WeaponStats>().currentHits.ToString();
        //}

        for (int i = 0; i < EquippedWeapon.GetComponent<WeaponStats>().maxHits; i++)
        {
            if (EquippedWeapon != defaultWeapon)
            {
                // Create a new image instance from the prefab
                Image durabilityBarFullInst = Instantiate(durabilityBarFull, durability.transform);
                Image durabilityBarEmptyInst = Instantiate(durabilityBarEmpty, durability.transform);

                durabilityBarFullInst.rectTransform.anchoredPosition = new Vector2(i * (durabilityBarFullInst.sprite.rect.width * durabilityBarFullInst.transform.localScale.x + spacing) - offset, 0);
                durabilityBarEmptyInst.rectTransform.anchoredPosition = new Vector2(i * (durabilityBarEmptyInst.sprite.rect.width * durabilityBarEmptyInst.transform.localScale.x + spacing) - offset, 0);
            }

        }
    }
}
