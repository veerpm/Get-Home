using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManagement : MonoBehaviour
{
    private GameObject equippedWeapon; // current weapon
    private GameObject floorWeapon;
    private GameObject defaultWeapon;
    private bool pickable;
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
        ResetDurability();
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
        ResetDurability();
        animator.SetBool(EquippedWeapon.name, false);
        EquippedWeapon.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
        EquippedWeapon.SetActive(true);
        EquippedWeapon = defaultWeapon;
    }

    void DisplayWeapon(GameObject weapon)
    {
        weaponDisplay.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        if (weapon != defaultWeapon)
        {
            DisplayWeaponDurability(EquippedWeapon.GetComponent<WeaponStats>().maxHits, durabilityBarEmpty);
            DisplayWeaponDurability(EquippedWeapon.GetComponent<WeaponStats>().currentHits, durabilityBarFull);
        }
    }

    public void AdjustDurability()
    {
        EquippedWeapon.GetComponent<WeaponStats>().currentHits--;

        if (EquippedWeapon.GetComponent<WeaponStats>().currentHits == 0 && EquippedWeapon != defaultWeapon)
        {
            EquippedWeapon.GetComponent<WeaponStats>().currentHits++;
            ResetDurability();
            animator.SetBool(EquippedWeapon.name, false);
            EquippedWeapon = defaultWeapon;
            DisplayWeapon(defaultWeapon);
        }


        //if (EquippedWeapon.GetComponent<WeaponStats>().currentHits < 0 && EquippedWeapon != defaultWeapon)
        //{
        //    animator.SetBool(EquippedWeapon.name, false);
        //    EquippedWeapon = defaultWeapon;
        //}

        if (EquippedWeapon != defaultWeapon)
        {
            RemoveWeaponDurability(1, durabilityBarFull);
        }

    }

    void DisplayWeaponDurability(int num, Image bar)
    {
        for (int i = 0; i < num; i++)
        {
            if (EquippedWeapon != defaultWeapon)
            {
                // Create a new image instance from the prefab
                Image durabilityBarFullInst = Instantiate(bar, durability.transform);

                durabilityBarFullInst.rectTransform.anchoredPosition = new Vector2(i * (durabilityBarFullInst.sprite.rect.width * durabilityBarFullInst.transform.localScale.x + spacing) - offset, 0);
            }
        }
    }

    void RemoveWeaponDurability(int num,Image bar)
    {
        GameObject[] durbarsObj  = GameObject.FindGameObjectsWithTag(bar.tag);

            for (int i = durbarsObj.Length-1; i >= durbarsObj.Length-num; i--)
            {
                Destroy(durbarsObj[i]);
            }

    }

    void ResetDurability()
    {
        RemoveWeaponDurability(EquippedWeapon.GetComponent<WeaponStats>().maxHits, durabilityBarEmpty);
        RemoveWeaponDurability(EquippedWeapon.GetComponent<WeaponStats>().currentHits, durabilityBarFull);
    }
}
