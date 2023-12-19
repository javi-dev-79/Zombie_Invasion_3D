using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public WeaponLogic[] weapons;
    private int currentWeaponIndex;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponChangeReview();
    }

    void changeCurrentWeapon()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    void WeaponChangeReview()
    {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll > 0f)
        {
            SelectPreviousWeapon();
            weapons[currentWeaponIndex].reloading = false;
            weapons[currentWeaponIndex].TimeNoShoot = false;
            weapons[currentWeaponIndex].activeADS = false;
        }
        else if (mouseScroll < 0f)
        {
            SelectNextWeapon();
            weapons[currentWeaponIndex].reloading = false;
            weapons[currentWeaponIndex].TimeNoShoot = false;
            weapons[currentWeaponIndex].activeADS = false;
        }
    }

    void SelectPreviousWeapon()
    {
        if (currentWeaponIndex == 0)
        {
            currentWeaponIndex = weapons.Length - 1;
        }
        else
        {
            currentWeaponIndex--;
            changeCurrentWeapon();
        }
    }

    void SelectNextWeapon()
    {
        if (currentWeaponIndex >= (weapons.Length -1))
        {
            currentWeaponIndex = 0;
        }
        else
        {
            currentWeaponIndex++;
            changeCurrentWeapon();
        }
    }
}
