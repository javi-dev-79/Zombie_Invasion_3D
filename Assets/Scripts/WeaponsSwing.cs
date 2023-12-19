using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsSwing : MonoBehaviour
{

    public float amount;
    public float maxAmount;
    public float time;
    public Vector3 initPosition;
    public bool setWeaponSwing;

    void Start()
    {
        initPosition = transform.localPosition;
    }

    void Update()
    {
        setWeaponSwing = true; // Balanceo activado por defecto
        float movX = Input.GetAxis("Mouse X") * amount;
        float movY = Input.GetAxis("Mouse Y") * amount;
        movX = Mathf.Clamp(movX, -maxAmount, maxAmount); // Clamp da un valor entre las dos posiciones, o devuelve el valor máximo si se pasa de este
        movY = Mathf.Clamp(movY, -maxAmount, maxAmount);

        Vector3 finalPositionMov = new Vector3(movX, movY, 0);

        if (Input.GetMouseButton(1))
        {
            setWeaponSwing = false; // Si activamos el zoom se desactiva el balanceo
        }

        if (setWeaponSwing)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPositionMov + initPosition, time * Time.deltaTime);
        }
    }
}
