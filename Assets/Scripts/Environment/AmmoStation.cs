using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStation : MonoBehaviour
{
    private float ammoStationRegen = 80;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="Player")
        {
            other.GetComponent<Shooting>().Ammo += ammoStationRegen * Time.deltaTime;
        }
    }
}
