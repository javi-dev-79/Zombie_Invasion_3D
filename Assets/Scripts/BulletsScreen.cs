using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsScreen : MonoBehaviour
{

    public Text text;
    public WeaponLogic weaponLogic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = weaponLogic.bulletsInCartridges + "/" + weaponLogic.cartridgeSize + "\n" + weaponLogic.remainingBullets;
    }
}
