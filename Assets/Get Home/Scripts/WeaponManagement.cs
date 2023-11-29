using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class WeaponManagement : MonoBehaviour
{
    public GameObject equippedWeapon; // current weapon
    private GameObject floorWeapon;
    private GameObject defaultWeapon;
    private GameObject brokenWeapon;
    private GameObject sprayCan;
    private bool pickable;
    public Image weaponDisplay; // HUD weapon display
    public GameObject durability;
    public Animator animator;
    public Image durabilityBarFull;
    public Image durabilityBarEmpty;
    public float spacing;
    public float offset;
    public ParticleSystem weaponBreakParticles;

    //SFX
    public AudioSource weaponBreakSound;
    public AudioSource loseDuribilitySound;
    public AudioSource pickupWeaponSound;

    // Start is called before the first frame update
    void Start()
    {
        defaultWeapon = GameObject.Find("Fists");
        sprayCan = GameObject.Find("SprayCan");
        EquippedWeapon = defaultWeapon;
        animator.SetBool(EquippedWeapon.name, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (equippedWeapon != sprayCan)
        {
            GetComponent<PlayerCombatMelee>().enabled = true;
            GetComponent<SprayCanBehaviour>().enabled = false;
        }
        else
        {
            GetComponent<PlayerCombatMelee>().enabled = false;
            GetComponent<SprayCanBehaviour>().enabled = true;
        }

        if (!pickable && Input.GetKeyDown(KeyCode.R) && EquippedWeapon != defaultWeapon)
        {
            DropWeapon();
        }

        if (pickable && Input.GetKeyDown(KeyCode.R))
        {
            SwapWeapon();
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
            if (equippedWeapon != sprayCan)
            {
                GetComponent<PlayerCombatMelee>().SetWeaponStats(equippedWeapon);
            }
            DisplayWeapon();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Transform colTr = collider.transform.parent;
        if (colTr != null && colTr.tag == "Weapon")
        {
            pickable = true;
            floorWeapon = collider.transform.parent.gameObject;
            colTr.Find("Light").GetComponent<Light2D>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Transform colTr = collider.transform.parent;
        if (colTr != null && colTr.tag == "Weapon")
        {
            pickable = false;
            colTr.Find("Light").GetComponent<Light2D>().enabled = false;
        }
    }

    void SwapWeapon()
    {
        //SFX
        pickupWeaponSound.Play();
        ResetDurability();
        GameObject temp = floorWeapon;
        floorWeapon = EquippedWeapon;
        animator.SetBool(floorWeapon.name, false);
        EquippedWeapon = temp;
        animator.SetBool(EquippedWeapon.name, true);
        floorWeapon.transform.position = new Vector3(transform.position.x,transform.position.y-0.5f,0);
        EquippedWeapon.SetActive(false);
        floorWeapon.SetActive(true);
    }

    void DropWeapon()
    {
        ResetDurability();
        animator.SetBool(EquippedWeapon.name, false);
        EquippedWeapon.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
        EquippedWeapon.SetActive(true);
        EquippedWeapon = defaultWeapon;
        animator.SetBool(EquippedWeapon.name, true);
    }

    void DisplayWeapon()
    {
        weaponDisplay.sprite = EquippedWeapon.GetComponent<SpriteRenderer>().sprite;
        if (EquippedWeapon != defaultWeapon && EquippedWeapon != sprayCan)
        {
            DisplayWeaponDurability(EquippedWeapon.GetComponent<WeaponStats>().maxHits, durabilityBarEmpty);
            DisplayWeaponDurability(EquippedWeapon.GetComponent<WeaponStats>().currentHits, durabilityBarFull);
        }
        else if (EquippedWeapon == sprayCan)
        {
            durability.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void AdjustDurability()
    {
        if (EquippedWeapon != sprayCan)
        {
            EquippedWeapon.GetComponent<WeaponStats>().currentHits--;

            if (EquippedWeapon.GetComponent<WeaponStats>().currentHits == 0 && EquippedWeapon != defaultWeapon)
            {
                //SFX
                weaponBreakSound.Play();

                brokenWeapon = EquippedWeapon;
                EquippedWeapon.GetComponent<WeaponStats>().currentHits++;
                ResetDurability();
                EquippedWeapon = defaultWeapon;
                weaponBreakParticles.Play();
            }

            if (EquippedWeapon != defaultWeapon)
            {
                RemoveWeaponDurability(1, durabilityBarFull);
            }
        }

        else
        {
            GetComponent<SprayCanBehaviour>().currentDurability -= Time.deltaTime;
            durability.transform.GetChild(0).GetComponent<Slider>().value = GetComponent<SprayCanBehaviour>().currentDurability / GetComponent<SprayCanBehaviour>().maxDurability;

            if (GetComponent<SprayCanBehaviour>().currentDurability <= 0)
            {
                GetComponent<SprayCanBehaviour>().ps.Stop();
                brokenWeapon = EquippedWeapon;
                ResetDurability();
                EquippedWeapon = defaultWeapon;
                weaponBreakParticles.Play();
            }
        }

    }

    // displays the number of the bars specified
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


    // removes the number of the bars specified
    void RemoveWeaponDurability(int num,Image bar)
    {
        GameObject[] durbarsObj  = GameObject.FindGameObjectsWithTag(bar.tag);

            for (int i = durbarsObj.Length-1; i >= durbarsObj.Length-num; i--)
            {
                Destroy(durbarsObj[i]);
            }

    }


    // removes all bars displayed
    void ResetDurability()
    {
        if (equippedWeapon != sprayCan)
        {
            RemoveWeaponDurability(EquippedWeapon.GetComponent<WeaponStats>().maxHits, durabilityBarEmpty);
            RemoveWeaponDurability(EquippedWeapon.GetComponent<WeaponStats>().currentHits, durabilityBarFull);
        }
        else
        {
            durability.transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    // used to fix animation after breaking
    public void ResetAnim()
    {
        if (brokenWeapon != null)
        {
            animator.SetBool(brokenWeapon.name, false);
            animator.SetBool(EquippedWeapon.name, true);
            brokenWeapon = null;
        }
    }
}
