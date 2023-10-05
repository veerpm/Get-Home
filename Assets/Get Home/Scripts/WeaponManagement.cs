using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManagement : MonoBehaviour
{
    public static GameObject equippedWeapon; // current weapon
    public GameObject floorWeapon;
    private bool inRange;
    private GameObject player;
    private PlayerCombatMelee combatMelee;
    public Image weaponDisplay; // HUD weapon display
    // Start is called before the first frame update
    void Start()
    {
        equippedWeapon = GameObject.Find("FistsGame");
        player = GameObject.Find("Player");
        combatMelee = player.GetComponent<PlayerCombatMelee>();
        //weaponDisplay = Image.Find("EquippedWeaponDisplay");
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.R))
        {
            SwapWeapon();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        inRange = false;
    }

    void SwapWeapon()
    {
        gameObject.SetActive(false);
        GameObject temp = gameObject;
        floorWeapon = equippedWeapon;
        equippedWeapon = temp;
        floorWeapon.transform.position = player.transform.position;
        floorWeapon.SetActive(true);
        Debug.Log(equippedWeapon);
        combatMelee.GetComponent<PlayerCombatMelee>().setWeapon(equippedWeapon);
        displayWeapon();
    }

    void displayWeapon()
    {
        weaponDisplay.sprite = equippedWeapon.GetComponent<SpriteRenderer>().sprite;
    }
}
